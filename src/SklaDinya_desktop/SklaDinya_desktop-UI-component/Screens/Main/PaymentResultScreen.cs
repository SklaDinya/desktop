using SklaDinya_desktop_UI_component.Controls;
using SklaDinya_desktop_UI_component.Helpers;

namespace SklaDinya_desktop_UI_component.Screens.Main;

public class PaymentResultScreen : UserControl
{
    public event EventHandler? GoHome;
    public event EventHandler? GoToBookings;
    public event EventHandler? RetryPayment;

    public PaymentResultScreen(bool success)
    {
        Dock = DockStyle.Fill;
        BackColor = AppTheme.Background;

        var container = new Panel { Width = 500, BackColor = AppTheme.PanelBackground };

        int y = 40;

        var icon = new Label
        {
            Text = success ? "✓" : "✗",
            Font = new Font("Segoe UI", 44f, FontStyle.Bold),
            ForeColor = success ? AppTheme.Secondary : AppTheme.Danger,
            TextAlign = ContentAlignment.MiddleCenter,
            Size = new Size(500, 70),
            Location = new Point(0, y),
        };
        y += 80;

        var msg = new Label
        {
            Text = success ? "Оплата прошла успешно!" : "Оплата не прошла.",
            Font = AppTheme.FontTitle,
            ForeColor = success ? AppTheme.Secondary : AppTheme.Danger,
            TextAlign = ContentAlignment.MiddleCenter,
            Size = new Size(500, 36),
            Location = new Point(0, y),
        };
        y += 56;

        var desc = new Label
        {
            Text = success
                ? "Бронирование оплачено. Вы можете вернуться на главную или посмотреть свои бронирования."
                : "Попробуйте ещё раз, возможно изменив способ оплаты.",
            Font = AppTheme.FontRegular,
            ForeColor = AppTheme.TextMuted,
            TextAlign = ContentAlignment.MiddleCenter,
            Size = new Size(440, 50),
            Location = new Point(30, y),
        };
        y += 64;

        var homeBtn = new RoundedButton
        {
            Text = "На главную", ButtonColor = AppTheme.Primary, Width = 436, Height = 42, Location = new Point(32, y),
        };
        homeBtn.Click += (_, _) => GoHome?.Invoke(this, EventArgs.Empty);
        y += 52;

        container.Controls.AddRange([icon, msg, desc, homeBtn]);

        if (success)
        {
            var bookingsBtn = new RoundedButton
            {
                Text = "Мои бронирования", ButtonColor = AppTheme.Secondary, Width = 436, Height = 42, Location = new Point(32, y),
            };
            bookingsBtn.Click += (_, _) => GoToBookings?.Invoke(this, EventArgs.Empty);
            container.Controls.Add(bookingsBtn);
            y += 52;
        }
        else
        {
            var retryBtn = new RoundedButton
            {
                Text = "Назад (повторить оплату)", ButtonColor = AppTheme.Accent, Width = 436, Height = 42, Location = new Point(32, y),
            };
            retryBtn.Click += (_, _) => RetryPayment?.Invoke(this, EventArgs.Empty);
            container.Controls.Add(retryBtn);
            y += 52;
        }

        container.Height = y + 24;
        Controls.Add(container);

        Resize += (_, _) =>
        {
            container.Location = new Point((Width - container.Width) / 2, (Height - container.Height) / 2);
            RoundedRenderer.ApplyRoundedRegion(container, 16);
        };
    }
}
