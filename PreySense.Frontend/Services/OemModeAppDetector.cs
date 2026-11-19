using System;
using System.Diagnostics;

namespace PreySense.Frontend.Services;

/// <summary>
/// Predator Sense / Nitro Sense keep their own cached operating mode and also react to the
/// chassis mode key. While one of them is running, both apps would write the same EC field on
/// every press, so PreySense steps aside and mirrors whatever the OEM app applies.
/// </summary>
public static class OemModeAppDetector
{
    private static readonly string[] ProcessNames =
        ["PredatorSense", "NitroSense", "PredatorSenseApp", "AcerPredatorSense", "NitroSenseApp"];

    private static readonly TimeSpan CacheLifetime = TimeSpan.FromSeconds(5);
    private static readonly object Gate = new();
    private static DateTime _checkedAt = DateTime.MinValue;
    private static bool _running;

    public static bool IsRunning()
    {
        lock (Gate)
        {
            if (DateTime.UtcNow - _checkedAt < CacheLifetime)
                return _running;

            _checkedAt = DateTime.UtcNow;
            _running = false;
            foreach (string name in ProcessNames)
            {
                Process[] matches;
                try { matches = Process.GetProcessesByName(name); }
                catch (Exception ex)
                {
                    AppLogger.Error($"OEM mode app detection failed for '{name}': {ex.Message}");
                    continue;
                }
                if (matches.Length == 0)
                    continue;
                foreach (Process match in matches)
                    match.Dispose();
                _running = true;
                break;
            }
            return _running;
        }
    }
}
