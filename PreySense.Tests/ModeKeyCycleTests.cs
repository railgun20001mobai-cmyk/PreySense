using PreySense.Frontend.ViewModels;

namespace PreySense.Tests;

[TestClass]
public class ModeKeyCycleTests
{
    [TestMethod]
    public void AcCycleStaysWithinTheFourAcModes()
    {
        byte mode = 0x00; // Quiet
        var seen = new List<byte>();
        for (int i = 0; i < 4; i++)
        {
            seen.Add(mode);
            mode = MainViewModel.CyclePerformanceMode(mode, onAc: true);
        }
        CollectionAssert.AreEqual(new byte[] { 0x00, 0x01, 0x04, 0x05 }, seen);
        Assert.AreEqual((byte)0x00, mode, "AC wraps back to Quiet and must never reach Eco.");
    }

    [TestMethod]
    public void AcCycleLeavesEcoForQuiet()
    {
        // Eco has no AC state; a stale Eco reading must not become the AC cycle origin.
        Assert.AreEqual((byte)0x00, MainViewModel.CyclePerformanceMode(0x06, onAc: true));
    }

    [TestMethod]
    public void BatteryCycleCoversAllFiveModes()
    {
        byte mode = 0x06; // Eco
        var seen = new List<byte>();
        for (int i = 0; i < 5; i++)
        {
            seen.Add(mode);
            mode = MainViewModel.CyclePerformanceMode(mode, onAc: false);
        }
        CollectionAssert.AreEqual(new byte[] { 0x06, 0x00, 0x01, 0x04, 0x05 }, seen);
        Assert.AreEqual((byte)0x06, mode, "Battery wraps back to Eco.");
    }
}
