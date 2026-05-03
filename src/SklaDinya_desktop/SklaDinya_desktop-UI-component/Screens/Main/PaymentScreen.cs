using SklaDinya_desktop_BL_component.Exceptions;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_UI_component.Controls;
using SklaDinya_desktop_UI_component.Helpers;

namespace SklaDinya_desktop_UI_component.Screens.Main;

/// <summary>
/// Экран оплаты уже созданного бронирования.
/// </summary>
public class PaymentScreen : UserControl
{
    private readonly BookingModel _booking;
    private readonly RadioButton _radioNoop;
    private readonly RadioButton _radioRandom;
    private readonly RoundedButton _payButton;
    private readonly RoundedButton _backButton;

    public event EventHandler? PaymentSuccess;
    public event EventHandler? PaymentFailed;
    public event EventHandler? BackRequested;

    public PaymentScreen(BookingModel booking)
    {
        _booking = booking;
        Dock = DockStyle.Fill;
        BackColor = AppTheme.Background;

        var container = new Panel { Width = 520, BackColor = AppTheme.PanelBackground };

        int y = 28;
        var title = new Label { Text = "Оплата бронирования", Font = AppTheme.FontTitle, ForeColor = AppTheme.Primary, AutoSize = true, Location = new Point(32, y) };
        y += 50;

        AddInfoLine(container, "Количество ячеек:", $"{booking.Cells.Count}", ref y);
        AddInfoLine(container, "Начало:", $"{booking.StartTime.ToLocalTime():dd.MM.yyyy HH:mm}", ref y);
        AddInfoLine(container, "Длительность:", $"{booking.BookingTime.TotalHours:F0} ч.", ref y);
        AddInfoLine(container, "Итого к оплате:", $"{booking.Price:F2} ₽", ref y, valueIsAccent: true);
        y += 12;

        var methodLabel = new Label { Text = "Способ оплаты:", Font = AppTheme.FontMedium, ForeColor = AppTheme.TextDark, AutoSize = true, Location = new Point(32, y) };
        y += 28;

        _radioNoop = new RadioButton { Text = "Гарантированная оплата", Font = AppTheme.FontRegular, AutoSize = true, Location = new Point(40, y), Checked = true };
        y += 28;
        _radioRandom = new RadioButton { Text = "Оплата с шансом 50%", Font = AppTheme.FontRegular, AutoSize = true, Location = new Point(40, y) };
        y += 44;

        _payButton = new RoundedButton { Text = "Оплатить", ButtonColor = AppTheme.Secondary, Width = 456, Height = 42, Location = new Point(32, y) };
        _payButton.Click += OnPayClick;
        y += 52;

        _backButton = new RoundedButton { Text = "← Назад", ButtonColor = AppTheme.TextMuted, Width = 456, Height = 42, Location = new Point(32, y) };
        _backButton.Click += (_, _) => BackRequested?.Invoke(this, EventArgs.Empty);
        y += 56;

        container.Height = y;
        container.Controls.AddRange([title, methodLabel, _radioNoop, _radioRandom, _payButton, _backButton]);
        Controls.Add(container);

        Resize += (_, _) =>
        {
            container.Location = new Point((Width - container.Width) / 2, (Height - container.Height) / 2);
            RoundedRenderer.ApplyRoundedRegion(container, 16);
        };
    }

    private void AddInfoLine(Panel container, string label, string value, ref int y, bool valueIsAccent = false)
    {
        container.Controls.Add(new Label { Text = label, Font = AppTheme.FontMedium, ForeColor = AppTheme.TextMuted, AutoSize = true, Location = new Point(32, y) });
        var valueLabel = new Label
        {
            Text = value,
            Font = valueIsAccent ? AppTheme.FontBold : AppTheme.FontMedium,
            ForeColor = valueIsAccent ? AppTheme.Primary : AppTheme.TextDark,
            AutoSize = true,
            Location = new Point(200, y),
        };
        container.Controls.Add(valueLabel);
        y += 26;
    }

    private async void OnPayClick(object? sender, EventArgs e)
    {
        _payButton.Enabled = false;
        _backButton.Enabled = false;

        bool success;
        if (_radioNoop.Checked)
        {
            var result = await ErrorHelper.TryAsync(() => ServiceLocator.PaymentService.PayNoopAsync());
            success = result is not null;
        }
        else
        {
            try { await ServiceLocator.PaymentService.PayRandomAsync(); success = true; }
            catch (PaymentFailedException) { success = false; }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _payButton.Enabled = true; _backButton.Enabled = true; return;
            }
        }

        if (success) PaymentSuccess?.Invoke(this, EventArgs.Empty);
        else PaymentFailed?.Invoke(this, EventArgs.Empty);
    }
}
