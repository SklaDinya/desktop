using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_UI_component.Controls;
using SklaDinya_desktop_UI_component.Helpers;

namespace SklaDinya_desktop_UI_component.Screens.Account;

/// <summary>
/// Вкладка «История бронирований» — все бронирования пользователя
/// </summary>
public class BookingHistoryTab : UserControl
{
    private readonly FlowLayoutPanel _listPanel;

    public BookingHistoryTab()
    {
        Dock = DockStyle.Fill;
        BackColor = AppTheme.Background;

        var title = new Label
        {
            Text = "История бронирований",
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

        var query = new BookingSearchQuery { PageNumber = 0, PageSize = 100 };
        var bookings = await ErrorHelper.TryAsync(
            () => ServiceLocator.BookingService.GetMyBookingsAsync(query));

        if (bookings is null) return;

        if (bookings.Count == 0)
        {
            _listPanel.Controls.Add(new Label
            {
                Text = "У вас пока нет бронирований.",
                Font = AppTheme.FontRegular,
                ForeColor = AppTheme.TextMuted,
                AutoSize = true,
            });
            return;
        }

        foreach (var booking in bookings.OrderByDescending(b => b.CreatedAt))
        {
            var card = new BookingCard(booking)
            {
                Width = _listPanel.Width - 60,
            };
            _listPanel.Controls.Add(card);
        }
    }
}
