namespace SklaDinya_desktop_UI_component.Helpers;

/// <summary>
/// Централизованные цвета, шрифты и настройки стиля приложения
/// </summary>
public static class AppTheme
{
    // ── Основные цвета ──────────────────────────────────────────────────
    public static readonly Color Primary = ColorTranslator.FromHtml("#444D7D");
    public static readonly Color Secondary = ColorTranslator.FromHtml("#5A723B");
    public static readonly Color White = Color.White;
    public static readonly Color Background = ColorTranslator.FromHtml("#F5F5F5");

    // ── Дополнительные цвета ────────────────────────────────────────────
    public static readonly Color Accent = ColorTranslator.FromHtml("#CD8D48");
    public static readonly Color Danger = ColorTranslator.FromHtml("#C65945");

    // ── Текстовые цвета ─────────────────────────────────────────────────
    public static readonly Color TextDark = ColorTranslator.FromHtml("#2D2D2D");
    public static readonly Color TextLight = Color.White;
    public static readonly Color TextMuted = ColorTranslator.FromHtml("#888888");
    public static readonly Color TextLink = ColorTranslator.FromHtml("#444D7D");

    // ── Граница / фон полей ─────────────────────────────────────────────
    public static readonly Color Border = ColorTranslator.FromHtml("#CCCCCC");
    public static readonly Color FieldBackground = Color.White;
    public static readonly Color PanelBackground = Color.White;
    public static readonly Color HeaderBackground = ColorTranslator.FromHtml("#444D7D");

    // ── Скругление ──────────────────────────────────────────────────────
    public const int CornerRadius = 20;

    // ── Шрифты ──────────────────────────────────────────────────────────
    public static readonly Font FontRegular = new("Segoe UI", 10f);
    public static readonly Font FontMedium = new("Segoe UI Semibold", 10f);
    public static readonly Font FontBold = new("Segoe UI", 11f, FontStyle.Bold);
    public static readonly Font FontTitle = new("Segoe UI", 16f, FontStyle.Bold);
    public static readonly Font FontHeader = new("Segoe UI", 13f, FontStyle.Bold);
    public static readonly Font FontSmall = new("Segoe UI", 9f);
    public static readonly Font FontLink = new("Segoe UI", 9.5f, FontStyle.Underline);

    // ── Размеры ─────────────────────────────────────────────────────────
    public const int HeaderHeight = 56;
    public const int SideMenuWidth = 220;
    public const int ButtonHeight = 40;
    public const int FieldHeight = 38;
    public const int Spacing = 12;
}
