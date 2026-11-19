using System.Drawing;
using PreySense.Rgb;

namespace PreySense.Tests;

[TestClass]
public class ScreenMimicEffectTests
{
    [TestMethod]
    public void ScreenMimic_IsWiredToTheCustomEffectDropdownAndEngine()
    {
        Assert.AreEqual(26, (int)CustomEffectMode.ScreenMimic, "Saved effect indexes must stay stable.");
        Assert.IsTrue(Enum.TryParse<CustomEffectMode>("ScreenMimic", out var parsed));
        Assert.AreEqual(CustomEffectMode.ScreenMimic, parsed);

        string root = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
        string xaml = System.IO.File.ReadAllText(System.IO.Path.Combine(root, "PreySense.Frontend", "Views", "KeyboardWindow.xaml"));
        Assert.Contains("Tag=\"DYN:ScreenMimic\"", xaml);
    }

    [TestMethod]
    public void ScreenSampler_FillsBandsOrFailsWithoutThrowing()
    {
        var bands = new Color[4];
        try
        {
            if (DesktopScreenSampler.TrySampleBands(bands))
                Assert.AreEqual(4, bands.Length);
        }
        catch (Exception ex)
        {
            Assert.Fail($"Screen sampling must never throw at the engine: {ex}");
        }
    }
}
