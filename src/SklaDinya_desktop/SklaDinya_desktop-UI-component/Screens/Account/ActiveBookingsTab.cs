using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_UI_component.Controls;
using SklaDinya_desktop_UI_component.Helpers;

namespace SklaDinya_desktop_UI_component.Screens.Account;

/// <summary>
/// Вкладка «Активные бронирования» — с кнопкой отмены
/// </summary>
public class ActiveBookingsTab : UserControl
{
    private readonly FlowLayoutPanel _listPanel;

    public ActiveBookingsTab()
    {
        Dock = DockStyle.Fill;
        BackColor = AppTheme.Background;

        var title = new Label
        {
            Text = "Активные бронирования",
            Font = AppTheme.FontTitle,
            ForeColor = AppTheme.Primary,
            Dock = DockStyle.Top,
            Height = 50,
            Padding = new Padding(24, 14, 0, 0),
        };

        _listPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            Padding = new Padding(24, 8, 24, 8),
        };

        Controls.Add(_listPanel);
        Controls.Add(title);

        Load += async (_, _) => await LoadAsync();
    }

    private async Task LoadAsync()
    {
        _listPanel.Controls.Clear();

        var query = new BookingSearchQuery { PageNumber = 0, PageSize = 50 };
        var bookings = await ErrorHelper.TryAsync(
            () => ServiceLocator.BookingService.GetMyBookingsAsync(query));

        if (bookings is null) return;

        var active = bookings
            .Where(b => b.Status is BookingStatus.Created or BookingStatus.Paid or BookingStatus.InProcess)
            .ToList();

        if (active.Count == 0)
        {
            _listPanel.Controls.Add(new Label
            {
                Text = "Нет активных бронирований.",
                Font = AppTheme.FontRegular,
                ForeColor = AppTheme.TextMuted,
                AutoSize = true,
                Padding = new Padding(0, 8, 0, 0),
            });
            return;
        }

        foreach (var booking in active)
        {
            var card = new BookingCard(booking, showCancelButton: true)
            {
                Width = _listPanel.Width - 60,
            };
            card.CancelClicked += async (_, _) =>
            {
                var confirm = MessageBox.Show(
                    "Вы уверены, что хотите отменить это бронирование?",
                    "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;

                var ok = await ErrorHelper.TryAsync(
                    () => ServiceLocator.BookingService.CancelMyBookingAsync(booking.Id),
                    "Бронирование отменено.");
                if (ok) await LoadAsync();
            };
            _listPanel.Controls.Add(card);
        }
    }
}
