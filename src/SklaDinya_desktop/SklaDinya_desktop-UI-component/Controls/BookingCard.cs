using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_UI_component.Helpers;
using System.Drawing.Drawing2D;

namespace SklaDinya_desktop_UI_component.Controls;

/// <summary>
/// Карточка бронирования для отображения в списках пользователя
/// </summary>
public class BookingCard : Panel
{
    public BookingModel Booking { get; }

    public event EventHandler? CancelClicked;

    public BookingCard(BookingModel booking, bool showCancelButton = false)
    {
        Booking = booking;
        Height = 110;
        Margin = new Padding(0, 0, 0, 8);
        DoubleBuffered = true;
        BackColor = Color.Transparent;

        if (showCancelButton && booking.Status is BookingStatus.Created or BookingStatus.Paid)
        {
            var cancelBtn = new RoundedButton
            {
                Text = "Отменить",
                BackColor = AppTheme.Danger,
                Width = 110,
                Height = 32,
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
            };
            cancelBtn.Location = new Point(Width - cancelBtn.Width - 20, 14);
            cancelBtn.Click += (_, _) => CancelClicked?.Invoke(this, EventArgs.Empty);
            Controls.Add(cancelBtn);
            Resize += (_, _) => cancelBtn.Location = new Point(Width - cancelBtn.Width - 20, 14);
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        e.Graphics.Clear(Parent?.BackColor ?? AppTheme.Background);

        var rect = new Rectangle(0, 0, Width - 1, Height - 1);
        using var path = RoundedRenderer.RoundedRect(rect, 14);
        using var fill = new SolidBrush(AppTheme.PanelBackground);
        e.Graphics.FillPath(fill, path);
        using var border = new Pen(AppTheme.Border);
        e.Graphics.DrawPath(border, path);

        using var titleBrush = new SolidBrush(AppTheme.Primary);
        e.Graphics.DrawString(
            $"Бронирование — {Booking.Storage?.Name ?? "Пункт хранения"}",
            AppTheme.FontBold, titleBrush, 16, 12);

        using var textBrush = new SolidBrush(AppTheme.TextDark);
        e.Graphics.DrawString(
            $"Начало: {Booking.StartTime:dd.MM.yyyy HH:mm}   |   " +
            $"Длительность: {Booking.BookingTime:hh\\:mm}   |   " +
            $"Ячеек: {Booking.Cells.Count}",
            AppTheme.FontRegular, textBrush, 16, 40);

        var statusColor = Booking.Status switch
        {
            BookingStatus.Created => AppTheme.Accent,
            BookingStatus.Paid => AppTheme.Secondary,
            BookingStatus.InProcess => AppTheme.Primary,
            BookingStatus.Finished => AppTheme.TextMuted,
            BookingStatus.Canceled => AppTheme.Danger,
            _ => AppTheme.TextDark
        };
        var statusText = Booking.Status switch
        {
            BookingStatus.Created => "Создано",
            BookingStatus.Paid => "Оплачено",
            BookingStatus.InProcess => "В процессе",
            BookingStatus.Finished => "Завершено",
            BookingStatus.Canceled => "Отменено",
            _ => Booking.Status.ToString()
        };
        using var statusBrush = new SolidBrush(statusColor);
        e.Graphics.DrawString($"Статус: {statusText}", AppTheme.FontMedium, statusBrush, 16, 68);
    }
}
