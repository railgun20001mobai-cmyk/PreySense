namespace PreySense.Tests;

[TestClass]
public class MainWindowModeTileTests
{
    private static string TileHeader(string xaml, string automationId)
    {
        int idIndex = xaml.IndexOf($"AutomationProperties.AutomationId=\"{automationId}\"", StringComparison.Ordinal);
        Assert.IsGreaterThanOrEqualTo(0, idIndex, $"{automationId} must exist");
        int start = xaml.LastIndexOf("<Button", idIndex, StringComparison.Ordinal);
        return xaml.Substring(start, idIndex - start);
    }

    [TestMethod]
    public void ModeTiles_OnlyShowModesTheActivePowerSourceSupports()
    {
        string root = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
        string xaml = System.IO.File.ReadAllText(System.IO.Path.Combine(root, "PreySense.Frontend", "MainWindow.xaml"));

        // Eco is battery-only; Quiet/Performance/Turbo need AC. Balanced is always offered.
        Assert.Contains("Converter={StaticResource BoolToVis}", TileHeader(xaml, "BtnModeEco"));
        Assert.Contains("Binding ShowEcoModeTile", TileHeader(xaml, "BtnModeEco"));
        foreach (string id in new[] { "BtnModeQuiet", "BtnModePerformance", "BtnModeTurbo" })
        {
            Assert.Contains("Converter={StaticResource BoolToVis}", TileHeader(xaml, id));
            Assert.Contains("Binding ShowAcOnlyModeTiles", TileHeader(xaml, id));
        }
        Assert.DoesNotContain("Visibility=", TileHeader(xaml, "BtnModeBalanced"));
    }

    [TestMethod]
    public void MainWindow_WarnsWhilePredatorSenseOwnsTheModeKey()
    {
        string root = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
        string xaml = System.IO.File.ReadAllText(System.IO.Path.Combine(root, "PreySense.Frontend", "MainWindow.xaml"));
        Assert.Contains("Binding IsOemModeAppRunning", xaml);
    }

    [TestMethod]
    public void HiddenModeTiles_GiveTheirColumnBackInsteadOfLeavingAGap()
    {
        string root = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
        string xaml = System.IO.File.ReadAllText(System.IO.Path.Combine(root, "PreySense.Frontend", "MainWindow.xaml"));
        Assert.Contains("Width=\"{Binding ShowEcoModeTile, Converter={StaticResource StarWhenTrueConverter}}\"", xaml);
        Assert.Contains("Width=\"{Binding ShowAcOnlyModeTiles, Converter={StaticResource StarWhenTrueConverter}}\"", xaml);
        // A collapsed child keeps its cell in UniformGrid, so the tile row must not use one.
        Assert.DoesNotContain("<UniformGrid Columns=\"5\"", xaml);
    }
}
