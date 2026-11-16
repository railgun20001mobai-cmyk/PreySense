namespace PreySense.Frontend.Services;

internal static partial class UiText
{
    static UiText()
    {
        AddLightingDirections();
        AddLightingSettings();
        AddLightingModes();
        AddLightingDevices();
        Add(Chinese, "游戏", "游戏模式", "添加游戏", "移除", "电源配置", "选择游戏可执行文件",
            "为每款游戏选择已保存的电源配置。游戏获得焦点时应用，退出时恢复。");
        Add(Hindi, "गेम", "गेम मोड", "गेम जोड़ें", "हटाएँ", "पावर प्रोफ़ाइल", "गेम की निष्पादन योग्य फ़ाइल चुनें",
            "हर गेम के लिए सहेजी गई पावर प्रोफ़ाइल चुनें। गेम पर फ़ोकस होने पर लागू होगी और बंद होने पर पिछली प्रोफ़ाइल लौटेगी।");
        Add(Spanish, "Juegos", "Modo de juego", "Añadir juego", "Eliminar", "Perfil de energía", "Seleccionar el ejecutable del juego",
            "Elige un perfil de energía guardado para cada juego. Se aplica al enfocarlo y se restaura al cerrarlo.");
        Add(French, "Jeux", "Mode jeu", "Ajouter un jeu", "Supprimer", "Profil d’alimentation", "Choisir l’exécutable du jeu",
            "Choisissez un profil d’alimentation enregistré pour chaque jeu. Il s’applique lorsque le jeu prend le focus et est restauré à sa fermeture.");
        Add(Turkish, "Oyunlar", "Oyun Modu", "Oyun Ekle", "Kaldır", "Güç Profili", "Oyun yürütülebilir dosyasını seçin",
            "Her oyun için kaydedilmiş bir güç profili seçin. Oyun odaklandığında uygulanır; kapandığında önceki profil geri yüklenir.");
        Chinese["Enable CPU Boost Tuning"] = "启用 CPU 睿频调校";
        Hindi["Enable CPU Boost Tuning"] = "CPU बूस्ट ट्यूनिंग सक्षम करें";
        Spanish["Enable CPU Boost Tuning"] = "Activar ajuste de CPU Boost";
        French["Enable CPU Boost Tuning"] = "Activer le réglage CPU Boost";
        Turkish["Enable CPU Boost Tuning"] = "CPU Boost ayarını etkinleştir";

        // Game profile validation results are shown in the Games window status line.
        AddAll("Invalid power mode.",
            "电源模式无效。", "अमान्य पावर मोड।", "Modo de energía no válido.", "Mode d’alimentation non valide.", "Geçersiz güç modu.");
        AddAll("Tuning is unavailable until factory defaults are ready.",
            "出厂默认值就绪前无法使用调校。", "फ़ैक्टरी डिफ़ॉल्ट तैयार होने तक ट्यूनिंग उपलब्ध नहीं है।",
            "El ajuste no está disponible hasta que los valores de fábrica estén listos.",
            "Le réglage n’est pas disponible tant que les valeurs d’usine ne sont pas prêtes.",
            "Fabrika ayarları hazır olana kadar ayar kullanılamaz.");
        AddAll("CPU limits are out of range.",
            "CPU 限制超出范围。", "CPU सीमाएँ सीमा से बाहर हैं।", "Los límites de CPU están fuera de rango.",
            "Les limites du CPU sont hors plage.", "CPU sınırları aralık dışında.");
        AddAll("Choose a CPU boost option.",
            "请选择一个 CPU 睿频选项。", "CPU बूस्ट विकल्प चुनें।", "Elige una opción de CPU Boost.",
            "Choisissez une option CPU Boost.", "Bir CPU Boost seçeneği seçin.");
        AddAll("GPU offsets are outside the supported Acer profile range or step.",
            "GPU 偏移超出 Acer 配置支持的范围或步进。", "GPU ऑफ़सेट समर्थित Acer प्रोफ़ाइल सीमा या स्टेप से बाहर हैं।",
            "Los desplazamientos de GPU están fuera del rango o paso del perfil Acer compatible.",
            "Les décalages GPU sont hors de la plage ou du pas du profil Acer pris en charge.",
            "GPU ofsetleri desteklenen Acer profil aralığının veya adımının dışında.");
        AddAll("GPU profile table was not found on this laptop.",
            "未在本机上找到 GPU 配置表。", "इस लैपटॉप पर GPU प्रोफ़ाइल तालिका नहीं मिली।",
            "No se encontró la tabla de perfiles de GPU en este portátil.",
            "La table des profils GPU est introuvable sur ce portable.",
            "Bu dizüstünde GPU profil tablosu bulunamadı.");
        AddAll("Enter supported GPU offsets.",
            "请输入受支持的 GPU 偏移。", "समर्थित GPU ऑफ़सेट दर्ज करें।", "Introduce desplazamientos de GPU compatibles.",
            "Saisissez des décalages GPU pris en charge.", "Desteklenen GPU ofsetlerini girin.");
        AddAll("The saved CPU profile cannot be restored.",
            "已保存的 CPU 配置无法恢复。", "सहेजी गई CPU प्रोफ़ाइल पुनर्स्थापित नहीं की जा सकती।",
            "No se puede restaurar el perfil de CPU guardado.", "Le profil CPU enregistré ne peut pas être restauré.",
            "Kaydedilen CPU profili geri yüklenemiyor.");
        AddAll("Could not read the current GPU profile.",
            "无法读取当前 GPU 配置。", "वर्तमान GPU प्रोफ़ाइल पढ़ी नहीं जा सकी।", "No se pudo leer el perfil de GPU actual.",
            "Impossible de lire le profil GPU actuel.", "Mevcut GPU profili okunamadı.");
        AddAll("Could not read the current CPU boost policy.",
            "无法读取当前 CPU 睿频策略。", "वर्तमान CPU बूस्ट नीति पढ़ी नहीं जा सकी।",
            "No se pudo leer la política de CPU Boost actual.", "Impossible de lire la stratégie CPU Boost actuelle.",
            "Mevcut CPU Boost ilkesi okunamadı.");
        AddAll("Fan RPM calibration failed: {0}",
            "风扇转速校准失败：{0}", "फ़ैन RPM कैलिब्रेशन विफल: {0}", "Error al calibrar las RPM del ventilador: {0}",
            "Échec de l’étalonnage des RPM du ventilateur : {0}", "Fan RPM kalibrasyonu başarısız: {0}");
        AddAll("Fan operation failed: {0}",
            "风扇操作失败：{0}", "फ़ैन संचालन विफल: {0}", "Error en la operación del ventilador: {0}",
            "Échec de l’opération du ventilateur : {0}", "Fan işlemi başarısız: {0}");
    }

    private static void Add(Dictionary<string, string> table, string games, string mode, string add, string remove,
        string profile, string executable, string hint)
    {
        table["Games"] = games;
        table["Game Mode"] = mode;
        table["Add"] = add;
        table["Remove"] = remove;
        table["Power Profile"] = profile;
        table["Select a game executable"] = executable;
    }
}
