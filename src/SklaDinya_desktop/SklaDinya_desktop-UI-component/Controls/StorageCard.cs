using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_UI_component.Helpers;
using System.Drawing.Drawing2D;

namespace SklaDinya_desktop_UI_component.Controls;

/// <summary>
/// Карточка пункта хранения — фиксированный размер, рисуется полностью вручную
/// </summary>
public class StorageCard : UserControl
{
    public StorageModel Storage { get; }
    private readonly RoundedButton _bookBtn;

    public event EventHandler? BookClicked;

    public StorageCard(StorageModel storage)
    {
        Storage = storage;
        // Высота увеличена со 110 до 140, чтобы описание помещалось
        // на две строки и не обрезалось при переносе.
        Size = new Size(700, 140);
        Margin = new Padding(0, 0, 0, 12);
        BackColor = Color.Transparent;
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);

        _bookBtn = new RoundedButton
        {
            Text = "Забронировать",
            ButtonColor = AppTheme.Secondary,
            // Ширина увеличена со 130 до 144 (~10%), чтобы текст
            // выглядел просторнее, без впритык.
            Size = new Size(144, 34),
            // Кнопку центрируем по вертикали относительно новой высоты карточки.
            // Правый отступ от края сохраняем 20 px.
            Location = new Point(Width - 164, (140 - 34) / 2),
        };
        _bookBtn.Click += (_, _) => BookClicked?.Invoke(this, EventArgs.Empty);
        Controls.Add(_bookBtn);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // Фон под скруглёнными углами рисуем цветом фактического (непрозрачного)
        // предка — иначе угол будет чёрным, как у RoundedButton.
        var parentBg = ResolveOpaqueParentBackground();
        using (var bgBrush = new SolidBrush(parentBg))
            g.FillRectangle(bgBrush, ClientRectangle);

        var rect = new Rectangle(0, 0, Width - 1, Height - 1);
        using var path = RoundedRenderer.RoundedRect(rect, 14);
        using (var fill = new SolidBrush(AppTheme.PanelBackground))
            g.FillPath(fill, path);
        using (var border = new Pen(AppTheme.Border, 1f))
            g.DrawPath(border, path);

        using var titleBrush = new SolidBrush(AppTheme.Primary);
        g.DrawString(Storage.Name, AppTheme.FontBold, titleBrush, 20, 14);

        using var textBrush = new SolidBrush(AppTheme.TextDark);
        g.DrawString($"Адрес: {Storage.Address}", AppTheme.FontRegular, textBrush, 20, 40);

        if (!string.IsNullOrEmpty(Storage.Description))
        {
            using var mutedBrush = new SolidBrush(AppTheme.TextMuted);
            // Область под описание: было 36px, теперь 64px — на 2 строки.
            // Также включаем перенос слов и обрезку по символам.
            using var fmt = new StringFormat
            {
                Trimming = StringTrimming.EllipsisWord,
                FormatFlags = 0, // word wrap включён по умолчанию
            };
            g.DrawString(
                Storage.Description,
                AppTheme.FontSmall,
                mutedBrush,
                // Ширину области описания подгоняем под расширенную кнопку:
                // кнопка теперь 144 px + 20 px правый отступ = резерв 184 px.
                new RectangleF(20, 66, Width - 184, 64),
                fmt);
        }
    }

    private Color ResolveOpaqueParentBackground()
    {
        var p = Parent;
        while (p is not null)
        {
            if (p.BackColor.A != 0) return p.BackColor;
            p = p.Parent;
        }
        return AppTheme.Background;
    }
}
