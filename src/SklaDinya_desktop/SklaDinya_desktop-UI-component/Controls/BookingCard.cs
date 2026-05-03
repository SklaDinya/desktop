using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_UI_component.Helpers;
using System.Drawing.Drawing2D;

namespace SklaDinya_desktop_UI_component.Controls;

/// <summary>
/// Карточка бронирования — фиксированный размер
/// </summary>
public class BookingCard : UserControl
{
    public BookingModel Booking { get; }
    public event EventHandler? CancelClicked;

    public BookingCard(BookingModel booking, bool showCancelButton = false)
    {
        Booking = booking;
        Size = new Size(700, 100);
        Margin = new Padding(0, 0, 0, 10);
        DoubleBuffered = true;
        BackColor = Color.Transparent;

        if (showCancelButton && booking.Status is BookingStatus.Created or BookingStatus.Paid)
        {
            var cancelBtn = new RoundedButton
            {
                Text = "Отменить", BackColor = AppTheme.Danger,
                Size = new Size(110, 30), Location = new Point(Width - 130, 12),
            };
            cancelBtn.Click += (_, _) => CancelClicked?.Invoke(this, EventArgs.Empty);
            Controls.Add(cancelBtn);
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        var parentBg = ResolveOpaqueParentBackground();
        using (var bgBrush = new SolidBrush(parentBg))
            g.FillRectangle(bgBrush, ClientRectangle);

        var rect = new Rectangle(0, 0, Width - 1, Height - 1);
        using var path = RoundedRenderer.RoundedRect(rect, 12);
        using (var fill = new SolidBrush(AppTheme.PanelBackground))
            g.FillPath(fill, path);
        using (var border = new Pen(AppTheme.Border))
            g.DrawPath(border, path);

        using var titleBrush = new SolidBrush(AppTheme.Primary);
        g.DrawString(Booking.Storage?.Name ?? "Пункт хранения", AppTheme.FontBold, titleBrush, 16, 10);

        using var textBrush = new SolidBrush(AppTheme.TextDark);
        g.DrawString(
            $"Начало: {Booking.StartTime.ToLocalTime():dd.MM.yyyy HH:mm}   " +
            $"Длительность: {Booking.BookingTime:hh\\:mm}   Ячеек: {Booking.Cells.Count}",
            AppTheme.FontSmall, textBrush, 16, 38);

        var statusText = Booking.Status switch
        {
            BookingStatus.Created => "Создано", BookingStatus.Paid => "Оплачено",
            BookingStatus.InProcess => "В процессе", BookingStatus.Finished => "Завершено",
            BookingStatus.Canceled => "Отменено", _ => Booking.Status.ToString()
        };
        var statusColor = Booking.Status switch
        {
            BookingStatus.Created => AppTheme.Accent, BookingStatus.Paid => AppTheme.Secondary,
            BookingStatus.InProcess => AppTheme.Primary, BookingStatus.Finished => AppTheme.TextMuted,
            BookingStatus.Canceled => AppTheme.Danger, _ => AppTheme.TextDark
        };
        using var statusBrush = new SolidBrush(statusColor);
        g.DrawString($"Статус: {statusText}", AppTheme.FontMedium, statusBrush, 16, 62);
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
