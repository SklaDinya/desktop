using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_UI_component.Helpers;
using System.Drawing.Drawing2D;

namespace SklaDinya_desktop_UI_component.Controls;

/// <summary>
/// Карточка пункта хранения для отображения в списке
/// </summary>
public class StorageCard : Panel
{
    public StorageModel Storage { get; }

    public event EventHandler? BookClicked;

    public StorageCard(StorageModel storage)
    {
        Storage = storage;
        Height = 120;
        Margin = new Padding(0, 0, 0, AppTheme.Spacing);
        Cursor = Cursors.Hand;
        DoubleBuffered = true;
        BackColor = Color.Transparent;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        e.Graphics.Clear(Parent?.BackColor ?? AppTheme.Background);

        var rect = new Rectangle(0, 0, Width - 1, Height - 1);
        using var path = RoundedRenderer.RoundedRect(rect, 16);
        using var fill = new SolidBrush(AppTheme.PanelBackground);
        e.Graphics.FillPath(fill, path);
        using var border = new Pen(AppTheme.Border, 1f);
        e.Graphics.DrawPath(border, path);

        using var titleBrush = new SolidBrush(AppTheme.Primary);
        e.Graphics.DrawString(Storage.Name, AppTheme.FontBold, titleBrush, 20, 16);

        using var textBrush = new SolidBrush(AppTheme.TextDark);
        e.Graphics.DrawString($"Адрес: {Storage.Address}", AppTheme.FontRegular, textBrush, 20, 42);

        if (!string.IsNullOrEmpty(Storage.Description))
        {
            using var mutedBrush = new SolidBrush(AppTheme.TextMuted);
            var descRect = new RectangleF(20, 66, Width - 180, 40);
            e.Graphics.DrawString(Storage.Description, AppTheme.FontSmall, mutedBrush, descRect);
        }

        // Кнопка «Забронировать»
        var btnRect = new Rectangle(Width - 160, Height / 2 - 18, 130, 36);
        using var btnPath = RoundedRenderer.RoundedRect(btnRect, 18);
        using var btnFill = new SolidBrush(AppTheme.Secondary);
        e.Graphics.FillPath(btnFill, btnPath);
        var btnText = "Забронировать";
        var btnSize = e.Graphics.MeasureString(btnText, AppTheme.FontSmall);
        using var btnTextBrush = new SolidBrush(AppTheme.TextLight);
        e.Graphics.DrawString(btnText, AppTheme.FontSmall, btnTextBrush,
            btnRect.X + (btnRect.Width - btnSize.Width) / 2,
            btnRect.Y + (btnRect.Height - btnSize.Height) / 2);
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        base.OnMouseClick(e);
        var btnRect = new Rectangle(Width - 160, Height / 2 - 18, 130, 36);
        if (btnRect.Contains(e.Location))
            BookClicked?.Invoke(this, EventArgs.Empty);
    }
}
