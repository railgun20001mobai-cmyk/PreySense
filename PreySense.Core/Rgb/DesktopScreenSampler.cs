using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace PreySense.Rgb;

/// <summary>
/// Captures the primary display through GDI and reduces it to a few horizontal colour bands,
/// which is what a zone keyboard can actually display. Deliberately avoids System.Drawing.Common
/// so the lighting engine keeps working from PreySense.Core without extra packages.
/// </summary>
[SupportedOSPlatform("windows")]
internal static class DesktopScreenSampler
{
    private const int SampleWidth = 32;
    private const int SampleHeight = 32;
    private const int HorzRes = 8;
    private const int VertRes = 10;
    private const int Halftone = 4;
    private const uint SrcCopy = 0x00CC0020;
    private const uint CaptureBlt = 0x40000000;

    [DllImport("user32.dll")] private static extern IntPtr GetDC(IntPtr hWnd);
    [DllImport("user32.dll")] private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDc);
    [DllImport("gdi32.dll")] private static extern IntPtr CreateCompatibleDC(IntPtr hDc);
    [DllImport("gdi32.dll")] private static extern bool DeleteDC(IntPtr hDc);
    [DllImport("gdi32.dll")] private static extern IntPtr SelectObject(IntPtr hDc, IntPtr obj);
    [DllImport("gdi32.dll")] private static extern bool DeleteObject(IntPtr obj);
    [DllImport("gdi32.dll")] private static extern int SetStretchBltMode(IntPtr hDc, int mode);
    [DllImport("gdi32.dll")] private static extern bool SetBrushOrgEx(IntPtr hDc, int x, int y, IntPtr previous);
    [DllImport("gdi32.dll")] private static extern int GetDeviceCaps(IntPtr hDc, int index);
    [DllImport("gdi32.dll")]
    private static extern bool StretchBlt(IntPtr hdcDest, int xDest, int yDest, int widthDest, int heightDest,
        IntPtr hdcSrc, int xSrc, int ySrc, int widthSrc, int heightSrc, uint rop);
    [DllImport("gdi32.dll")]
    private static extern IntPtr CreateDIBSection(IntPtr hDc, ref BitmapInfo info, uint usage, out IntPtr bits,
        IntPtr section, uint offset);

    [StructLayout(LayoutKind.Sequential)]
    private struct BitmapInfoHeader
    {
        public uint Size;
        public int Width;
        public int Height;
        public ushort Planes;
        public ushort BitCount;
        public uint Compression;
        public uint SizeImage;
        public int XPelsPerMeter;
        public int YPelsPerMeter;
        public uint ClrUsed;
        public uint ClrImportant;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct BitmapInfo
    {
        public BitmapInfoHeader Header;
    }

    /// <summary>Fills <paramref name="bands"/> with the average colour of each horizontal slice.</summary>
    public static bool TrySampleBands(Span<Color> bands)
    {
        int count = bands.Length;
        if (count <= 0)
            return false;

        IntPtr screenDc = GetDC(IntPtr.Zero);
        if (screenDc == IntPtr.Zero)
            return false;

        IntPtr memoryDc = IntPtr.Zero;
        IntPtr dib = IntPtr.Zero;
        IntPtr previous = IntPtr.Zero;
        try
        {
            int width = GetDeviceCaps(screenDc, HorzRes);
            int height = GetDeviceCaps(screenDc, VertRes);
            if (width <= 0 || height <= 0)
                return false;

            memoryDc = CreateCompatibleDC(screenDc);
            if (memoryDc == IntPtr.Zero)
                return false;

            var info = new BitmapInfo
            {
                Header = new BitmapInfoHeader
                {
                    Size = (uint)Marshal.SizeOf<BitmapInfoHeader>(),
                    Width = SampleWidth,
                    Height = -SampleHeight, // Top-down rows so band 0 is the top of the screen.
                    Planes = 1,
                    BitCount = 32
                }
            };
            dib = CreateDIBSection(memoryDc, ref info, 0, out IntPtr bits, IntPtr.Zero, 0);
            if (dib == IntPtr.Zero || bits == IntPtr.Zero)
                return false;

            previous = SelectObject(memoryDc, dib);
            SetStretchBltMode(memoryDc, Halftone);
            SetBrushOrgEx(memoryDc, 0, 0, IntPtr.Zero);
            if (!StretchBlt(memoryDc, 0, 0, SampleWidth, SampleHeight, screenDc, 0, 0, width, height, SrcCopy | CaptureBlt))
                return false;

            var pixels = new byte[SampleWidth * SampleHeight * 4];
            Marshal.Copy(bits, pixels, 0, pixels.Length);

            int rowsPerBand = Math.Max(1, SampleHeight / count);
            for (int band = 0; band < count; band++)
            {
                int startRow = band * rowsPerBand;
                int endRow = band == count - 1 ? SampleHeight : Math.Min(SampleHeight, startRow + rowsPerBand);
                long red = 0, green = 0, blue = 0;
                int samples = 0;
                for (int y = startRow; y < endRow; y++)
                {
                    for (int x = 0; x < SampleWidth; x++)
                    {
                        int index = (y * SampleWidth + x) * 4;
                        blue += pixels[index];
                        green += pixels[index + 1];
                        red += pixels[index + 2];
                        samples++;
                    }
                }
                if (samples == 0)
                    continue;
                bands[band] = Color.FromArgb((int)(red / samples), (int)(green / samples), (int)(blue / samples));
            }
            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            if (previous != IntPtr.Zero && memoryDc != IntPtr.Zero)
                SelectObject(memoryDc, previous);
            if (dib != IntPtr.Zero)
                DeleteObject(dib);
            if (memoryDc != IntPtr.Zero)
                DeleteDC(memoryDc);
            ReleaseDC(IntPtr.Zero, screenDc);
        }
    }
}
