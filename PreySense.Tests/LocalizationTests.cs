using System.Xml.Linq;
using PreySense.Frontend.Services;

namespace PreySense.Tests;

[TestClass]
[DoNotParallelize]
public class LocalizationTests
{
    [TestMethod]
    public void CurrentXamlAndTelemetryLabels_HaveTranslationsInEveryLanguage()
    {
        string root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "PreySense.Frontend"));
        var labels = new HashSet<string>();
        foreach (string file in Directory.EnumerateFiles(root, "*.xaml", SearchOption.AllDirectories))
        {
            if (file.Contains(Path.DirectorySeparatorChar + "obj" + Path.DirectorySeparatorChar) ||
                file.Contains(Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar)) continue;
            var document = System.Xml.Linq.XDocument.Load(file);
            foreach (var attribute in document.Descendants().Attributes())
            {
                if (attribute.Name.LocalName is not ("Title" or "Text" or "Content" or "Header" or "ActionText" or "LeftText" or "RightText" or "Subtitle")) continue;
                string value = attribute.Value;
                if (value.StartsWith('{') || value.StartsWith("PreySense") || !System.Text.RegularExpressions.Regex.IsMatch(value, "[a-zA-Z]{2}")) continue;
                if (value is "CPU" or "GPU" or "—" or "MHz" or "RPM" or "W" or "V" or "GB" or "MT/s") continue;
                labels.Add(value);
            }
        }
        var window = typeof(PreySense.Frontend.Views.MetricsWindow);
        foreach (string field in new[] { "CpuChoices", "GpuChoices" })
        {
            var choices = (System.Collections.IEnumerable)window.GetField(field, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!.GetValue(null)!;
            foreach (object choice in choices)
                labels.Add((string)choice.GetType().GetProperty("Label")!.GetValue(choice)!);
        }
        labels.UnionWith(new[] { "Run on Startup", "Slot #{0}", "Include PROCHOT offset", "Applied and read back successfully.", "Could not apply." });
        foreach (string language in new[] { "zh-CN", "hi-IN", "es-ES", "fr-FR", "tr-TR" })
        {
            var table = UiText.For(language)!;
            foreach (string label in labels)
                Assert.IsTrue(table.ContainsKey(label), $"{language}: missing {label}");
            foreach (var entry in table)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(entry.Value), $"{language}: empty {entry.Key}");
                var placeholders = System.Text.RegularExpressions.Regex.Matches(entry.Key, @"\{\d+[^}]*\}").Select(m => m.Value).Order().ToArray();
                var translated = System.Text.RegularExpressions.Regex.Matches(entry.Value, @"\{\d+[^}]*\}").Select(m => m.Value).Order().ToArray();
                CollectionAssert.AreEqual(placeholders, translated, $"{language}: placeholders {entry.Key}");
            }
        }
    }

    [TestMethod]
    public void HardwareEffectNamesFromLayouts_HaveTranslationsInEveryLanguage()
    {
        using var stream = typeof(PreySense.Rgb.OemUsbLightingProtocol).Assembly
            .GetManifestResourceStream("PreySense.Core.Resources.Rgb.OemUsbKeyboardLayouts.json")!;
        using var document = System.Text.Json.JsonDocument.Parse(stream);
        string[] names = document.RootElement.GetProperty("Modes").EnumerateObject()
            .SelectMany(family => family.Value.EnumerateArray())
            .Select(mode => mode.GetProperty("name").GetString()!)
            .Distinct(StringComparer.Ordinal)
            .ToArray();
        Assert.IsNotEmpty(names);
        foreach (string language in new[] { "zh-CN", "hi-IN", "es-ES", "fr-FR", "tr-TR" })
        {
            var table = UiText.For(language)!;
            foreach (string name in names)
                Assert.IsTrue(table.ContainsKey(name), $"{language}: missing effect name {name}");
        }
        Assert.AreEqual("静态", UiText.For("zh-CN")!["STATIC"]);
        Assert.AreEqual("呼吸", UiText.For("zh-CN")!["BREATHING"]);
    }

    [TestMethod]
    public void DeviceEffectNames_HaveTranslationsInEveryLanguage()
    {
        // Mirrors the EC list in OemLightingController plus the Acer HID effect names; "Wave"
        // was missing once and leaked into the Chinese Effect dropdown as English.
        string[] names = ["Mode", "Custom colors", "Static", "Breathing", "Neon", "Wave", "Ripple", "Zoom",
            "Snake", "Disco", "Shifting", "Meteor", "Twinkling"];
        foreach (string language in new[] { "zh-CN", "hi-IN", "es-ES", "fr-FR", "tr-TR" })
        {
            var table = UiText.For(language)!;
            foreach (string name in names)
                Assert.IsTrue(table.ContainsKey(name), $"{language}: missing effect name {name}");
        }
        var chinese = UiText.For("zh-CN")!;
        Assert.AreEqual("波形", chinese["Wave"]);
        Assert.AreEqual("波纹", chinese["Ripple"]);
        Assert.AreEqual("蛇行", chinese["Snake"]);
    }

    [TestMethod]
    public void InlineComboBoxEntries_AreTranslated_AndKeepTheirTag()
    {
        Exception? failure = null;
        var thread = new Thread(() =>
        {
            string previous = LocalizationService.CurrentLanguage;
            try
            {
                var combo = new System.Windows.Controls.ComboBox();
                var lightingMode = new System.Windows.Controls.ComboBoxItem { Content = "Dynamic", Tag = "DYN:RainbowCycle" };
                combo.Items.Add(lightingMode);
                combo.Items.Add(new System.Windows.Controls.ComboBoxItem { Content = "Breathing", Tag = "HW:1" });

                LocalizationService.SetLanguage("zh-CN", persist: false);
                LocalizationService.LocalizeComboBoxItems(combo);
                Assert.AreEqual("动态", (string)lightingMode.Content);
                Assert.AreEqual("DYN:RainbowCycle", (string)lightingMode.Tag, "Tag must not be translated.");
                Assert.AreEqual("呼吸", (string)((System.Windows.Controls.ComboBoxItem)combo.Items[1]!).Content);

                LocalizationService.SetLanguage("es-ES", persist: false);
                LocalizationService.LocalizeComboBoxItems(combo);
                Assert.AreEqual("Dinámico", (string)lightingMode.Content);

                LocalizationService.SetLanguage("en-US", persist: false);
                LocalizationService.LocalizeComboBoxItems(combo);
                Assert.AreEqual("Dynamic", (string)lightingMode.Content, "Switching back must restore the original text.");
            }
            catch (Exception ex) { failure = ex; }
            finally { LocalizationService.SetLanguage(previous, persist: false); }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        Assert.IsTrue(thread.Join(TimeSpan.FromSeconds(10)), "ComboBox localization test timed out.");
        if (failure is not null) System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(failure).Throw();
    }

    [TestMethod]
    public void Test_Five_Languages_Translate_Labels_And_Dialogs()
    {
        string previous = LocalizationService.CurrentLanguage;
        try
        {
            LocalizationService.SetLanguage("en-US", persist: false);
            Assert.AreEqual("Quit", LocalizationService.Translate("Quit"));
            Assert.AreEqual("PreySense • Lighting", LocalizationService.Translate("PreySense • Lighting"));
            Assert.AreEqual(0, LocalizationService.LanguageIndex);

            LocalizationService.SetLanguage("zh-CN", persist: false);
            Assert.AreEqual("退出", LocalizationService.Translate("Quit"));
            Assert.AreEqual("电池充电限制", LocalizationService.Translate("Battery Limit"));
            Assert.AreEqual("混合", LocalizationService.Translate("Hybrid"));
            Assert.AreEqual("关闭", LocalizationService.Translate("Off"));
            Assert.AreEqual("无", LocalizationService.Translate("None"));
            Assert.AreEqual("PreySense • 灯光", LocalizationService.Translate("PreySense • Lighting"));
            Assert.AreEqual(" • 硬件显示切换", LocalizationService.Translate(" • Hardware Display MUX"));
            Assert.AreEqual("正在读取节能…", LocalizationService.Format("Reading {0}...", LocalizationService.Translate("Eco")));
            Assert.AreEqual(1, LocalizationService.LanguageIndex);

            LocalizationService.SetLanguage("hi-IN", persist: false);
            Assert.AreEqual("बाहर निकलें", LocalizationService.Translate("Quit"));
            Assert.AreNotEqual("Quit", LocalizationService.Translate("Battery Limit"));
            Assert.AreEqual(2, LocalizationService.LanguageIndex);

            LocalizationService.SetLanguage("es-ES", persist: false);
            Assert.AreEqual("Salir", LocalizationService.Translate("Quit"));
            Assert.AreEqual("Límite de batería", LocalizationService.Translate("Battery Limit"));
            Assert.AreEqual("¿Restablecer Equilibrado a los ajustes de fábrica y aplicarlos ahora?",
                LocalizationService.Format("Reset {0} to factory power settings and apply them now?", LocalizationService.Translate("Balanced")));
            Assert.AreEqual(3, LocalizationService.LanguageIndex);

            LocalizationService.SetLanguage("fr-FR", persist: false);
            Assert.AreEqual("Quitter", LocalizationService.Translate("Quit"));
            Assert.AreEqual("Limite de batterie", LocalizationService.Translate("Battery Limit"));
            Assert.AreEqual("PreySense • Éclairage", LocalizationService.Translate("PreySense • Lighting"));
            Assert.AreEqual(4, LocalizationService.LanguageIndex);

            LocalizationService.SetLanguage("tr-TR", persist: false);
            Assert.AreEqual("Çıkış", LocalizationService.Translate("Quit"));
            Assert.AreEqual("Pil Sınırı", LocalizationService.Translate("Battery Limit"));
            Assert.AreEqual("PreySense • Aydınlatma", LocalizationService.Translate("PreySense • Lighting"));
            Assert.AreEqual(5, LocalizationService.LanguageIndex);

            Assert.AreEqual("en-US", LocalizationService.Normalize("en-GB"));
            Assert.AreEqual("zh-CN", LocalizationService.Normalize("zh-CN"));
            Assert.AreEqual("hi-IN", LocalizationService.Normalize("hi"));
            Assert.AreEqual("es-ES", LocalizationService.Normalize("es-MX"));
            Assert.AreEqual("fr-FR", LocalizationService.Normalize("fr-CA"));
            Assert.AreEqual("tr-TR", LocalizationService.Normalize("tr-TR"));
            Assert.AreEqual("tr-TR", LocalizationService.Normalize("tr"));
        }
        finally
        {
            LocalizationService.SetLanguage(previous, persist: false);
        }
    }
}
