using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_UI_component.Controls;
using SklaDinya_desktop_UI_component.Helpers;

namespace SklaDinya_desktop_UI_component.Screens.Account;

public class BookingHistoryTab : UserControl
{
    private readonly FlowLayoutPanel _listPanel;

    public BookingHistoryTab()
    {
        Dock = DockStyle.Fill;
        BackColor = AppTheme.Background;

        var title = new Label
        {
            Text = "История бронирований", Font = AppTheme.FontTitle, ForeColor = AppTheme.Primary,
            Dock = DockStyle.Top, Height = 50, Padding = new Padding(24, 14, 0, 0),
        };

        _listPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill, AutoScroll = true,
            FlowDirection = FlowDirection.TopDown, WrapContents = false,
            BackColor = AppTheme.Background, Padding = new Padding(0, 8, 0, 8),
        };
        _listPanel.Resize += (_, _) => CenterCards();

        Controls.Add(_listPanel);
        Controls.Add(title);

        Load += async (_, _) => await LoadAsync();
    }

    private void CenterCards()
    {
        foreach (Control c in _listPanel.Controls)
        {
            if (c is BookingCard card)
            {
                var left = Math.Max(24, (_listPanel.ClientSize.Width - card.Width) / 2);
                card.Margin = new Padding(left, 0, 0, 10);
            }
        }
    }

    private async Task LoadAsync()
    {
        _listPanel.Controls.Clear();
        var bookings = await ErrorHelper.TryAsync(() => ServiceLocator.BookingService.GetMyBookingsAsync(new BookingSearchQuery { PageNumber = 0, PageSize = 100 }));
        if (bookings is null) return;

        if (bookings.Count == 0)
        {
            _listPanel.Controls.Add(new Label { Text = "Нет бронирований.", Font = AppTheme.FontRegular, ForeColor = AppTheme.TextMuted, AutoSize = true, Margin = new Padding(24, 8, 0, 0) });
            return;
        }

        foreach (var b in bookings.OrderByDescending(b => b.CreatedAt))
        {
            var card = new BookingCard(b);
            _listPanel.Controls.Add(card);
        }
        CenterCards();
    }
}
