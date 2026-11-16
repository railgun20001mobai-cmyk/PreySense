namespace PreySense.Frontend.Services;

internal static partial class UiText
{
    /// <summary>
    /// Hardware effect names are read verbatim from OemUsbKeyboardLayouts.json and shown in
    /// the device Effect dropdown, so they need their own entries. They are uppercase in the
    /// JSON, which is why they cannot reuse the title-case preset names above.
    /// </summary>
    private static void AddLightingModes()
    {
        AddAll("OFF", "关闭", "बंद", "Apagado", "Éteint", "Kapalı");
        AddAll("STATIC", "常亮", "स्थिर", "Fijo", "Fixe", "Sabit");
        AddAll("BREATHING", "呼吸", "ब्रीदिंग", "Respiración", "Respiration", "Nefes");
        AddAll("WAVE", "波浪", "वेव", "Onda", "Vague", "Dalga");
        AddAll("SNAKE", "贪吃蛇", "स्नेक", "Serpiente", "Serpent", "Yılan");
        AddAll("RIPPLE", "涟漪", "रिप्पल", "Ondulación", "Ondulation", "Dalgalanma");
        AddAll("NEON", "霓虹", "नियॉन", "Neón", "Néon", "Neon");
        AddAll("RAINDROP", "雨滴", "बारिश की बूँद", "Gota de lluvia", "Goutte de pluie", "Yağmur damlası");
        AddAll("LIGHTNING", "闪电", "बिजली", "Relámpago", "Éclair", "Şimşek");
        AddAll("SPOT", "聚光灯", "स्पॉट", "Foco", "Projecteur", "Spot");
        AddAll("STAR", "星光", "तारा", "Estrella", "Étoile", "Yıldız");
        AddAll("FIREBALL", "火球", "आग का गोला", "Bola de fuego", "Boule de feu", "Ateş topu");
        AddAll("SNOW", "雪花", "बर्फ़", "Nieve", "Neige", "Kar");
        AddAll("HEARTBEAT", "心跳", "धड़कन", "Latido", "Battement", "Kalp atışı");
        AddAll("DAZZLING", "炫彩", "चमकदार", "Deslumbrante", "Éblouissant", "Göz alıcı");
        AddAll("MATRIX", "矩阵", "मैट्रिक्स", "Matriz", "Matrice", "Matris");
        AddAll("SWIPING", "滑动", "स्वाइप", "Deslizamiento", "Glissement", "Kaydırma");
        AddAll("RACING", "竞速", "रेसिंग", "Carrera", "Course", "Yarış");
        AddAll("SPROUTING", "萌芽", "अंकुरण", "Brote", "Germination", "Filizlenme");
        AddAll("DISCO", "迪斯科", "डिस्को", "Discoteca", "Disco", "Disko");
        AddAll("PINGPONG", "乒乓", "पिंग पॉन्ग", "Ping pong", "Ping-pong", "Ping pong");
        AddAll("ROWWAVE", "行波", "पंक्ति तरंग", "Onda por filas", "Vague par rangées", "Sıra dalgası");
        AddAll("Light Show", "灯光秀", "लाइट शो", "Espectáculo de luz", "Spectacle lumineux", "Işık gösterisi");
        AddAll("Custom colors", "自定义颜色", "कस्टम रंग", "Colores personalizados", "Couleurs personnalisées", "Özel renkler");
    }

    /// <summary>
    /// Device tab labels are built in LightingDeviceInfo from the HID device names, so they
    /// also arrive as plain English strings.
    /// </summary>
    private static void AddLightingDevices()
    {
        AddAll("Turbo Key", "Turbo 键", "टर्बो कुंजी", "Tecla Turbo", "Touche Turbo", "Turbo tuşu");
        AddAll("Mode Key", "模式键", "मोड कुंजी", "Tecla de modo", "Touche Mode", "Mod tuşu");
        AddAll("Cover Logo", "上盖徽标", "कवर लोगो", "Logotipo de la tapa", "Logo du capot", "Kapak logosu");
        AddAll("Base Logo", "底盖徽标", "बेस लोगो", "Logotipo inferior", "Logo inférieur", "Taban logosu");
        AddAll("Acer lighting", "Acer 灯效", "Acer लाइटिंग", "Iluminación Acer", "Éclairage Acer", "Acer aydınlatma");
        AddAll("Light bar", "灯条", "लाइट बार", "Barra de luz", "Barre lumineuse", "Işık çubuğu");
        AddAll("Front Lightbar", "前侧灯条", "सामने का लाइटबार", "Barra luminosa frontal", "Barre lumineuse avant", "Ön ışık çubuğu");
        AddAll("Left Lightbar", "左侧灯条", "बायाँ लाइटबार", "Barra luminosa izquierda", "Barre lumineuse gauche", "Sol ışık çubuğu");
        AddAll("Right Lightbar", "右侧灯条", "दायाँ लाइटबार", "Barra luminosa derecha", "Barre lumineuse droite", "Sağ ışık çubuğu");
        AddAll("Rear Lightbar", "后侧灯条", "पीछे का लाइटबार", "Barra luminosa trasera", "Barre lumineuse arrière", "Arka ışık çubuğu");
    }

    private static void AddAll(string key, string chinese, string hindi, string spanish, string french, string turkish)
    {
        Chinese[key] = chinese;
        Hindi[key] = hindi;
        Spanish[key] = spanish;
        French[key] = french;
        Turkish[key] = turkish;
    }
}
