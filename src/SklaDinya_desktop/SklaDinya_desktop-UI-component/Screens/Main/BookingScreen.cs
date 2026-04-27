using SklaDinya_desktop_BL_component.Exceptions;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_UI_component.Controls;
using SklaDinya_desktop_UI_component.Helpers;

namespace SklaDinya_desktop_UI_component.Screens.Main;

/// <summary>
/// Экран бронирования: выбор ячеек, времени, способа оплаты
/// </summary>
public class BookingScreen : UserControl
{
    private readonly StorageModel _storage;
    private readonly CheckedListBox _cellList;
    private readonly DateTimePicker _startTimePicker;
    private readonly NumericUpDown _hoursUpDown;
    private readonly RoundedButton _payNoopButton;
    private readonly RoundedButton _payRandomButton;
    private readonly Label _priceInfoLabel;
    private readonly RoundedButton _searchCellsButton;
    private readonly Label _titleLabel;

    public event EventHandler? BookingCompleted;
    public event EventHandler? BackRequested;

    public BookingScreen(StorageModel storage)
    {
        _storage = storage;
        Dock = DockStyle.Fill;
        BackColor = AppTheme.Background;
        AutoScroll = true;
        Padding = new Padding(40, 20, 40, 20);

        int y = 20;

        var backBtn = new RoundedButton
        {
            Text = "← Назад",
            BackColor = AppTheme.TextMuted,
            Width = 100,
            Height = 32,
            Location = new Point(20, y),
        };
        backBtn.Click += (_, _) => BackRequested?.Invoke(this, EventArgs.Empty);
        Controls.Add(backBtn);

        _titleLabel = new Label
        {
            Text = $"Бронирование — {storage.Name}",
            Font = AppTheme.FontTitle,
            ForeColor = AppTheme.Primary,
            AutoSize = true,
            Location = new Point(20, y + 44),
        };
        Controls.Add(_titleLabel);
        y += 90;

        // Время начала
        var startLabel = new Label { Text = "Дата и время начала:", Font = AppTheme.FontMedium, AutoSize = true, Location = new Point(20, y) };
        Controls.Add(startLabel);
        y += 28;
        _startTimePicker = new DateTimePicker
        {
            Format = DateTimePickerFormat.Custom,
            CustomFormat = "dd.MM.yyyy HH:mm",
            Location = new Point(20, y),
            Width = 250,
            Value = DateTime.Now.AddHours(1),
        };
        Controls.Add(_startTimePicker);
        y += 40;

        // Длительность
        var durationLabel = new Label { Text = "Длительность (часы):", Font = AppTheme.FontMedium, AutoSize = true, Location = new Point(20, y) };
        Controls.Add(durationLabel);
        y += 28;
        _hoursUpDown = new NumericUpDown
        {
            Minimum = 1,
            Maximum = 720,
            Value = 2,
            Location = new Point(20, y),
            Width = 120,
        };
        Controls.Add(_hoursUpDown);

        _searchCellsButton = new RoundedButton
        {
            Text = "Найти свободные ячейки",
            BackColor = AppTheme.Secondary,
            Width = 220,
            Height = 36,
            Location = new Point(160, y - 2),
        };
        _searchCellsButton.Click += OnSearchCellsClick;
        Controls.Add(_searchCellsButton);
        y += 50;

        // Список ячеек
        var cellsLabel = new Label { Text = "Доступные ячейки (отметьте нужные):", Font = AppTheme.FontMedium, AutoSize = true, Location = new Point(20, y) };
        Controls.Add(cellsLabel);
        y += 28;
        _cellList = new CheckedListBox
        {
            Location = new Point(20, y),
            Width = 500,
            Height = 180,
            Font = AppTheme.FontRegular,
            BorderStyle = BorderStyle.FixedSingle,
        };
        Controls.Add(_cellList);
        y += 196;

        // Информация о ценах
        _priceInfoLabel = new Label
        {
            Text = "",
            Font = AppTheme.FontSmall,
            ForeColor = AppTheme.TextMuted,
            AutoSize = true,
            Location = new Point(20, y),
        };
        Controls.Add(_priceInfoLabel);
        y += 30;

        // Кнопки оплаты
        _payNoopButton = new RoundedButton
        {
            Text = "Оплатить (гарантированно)",
            BackColor = AppTheme.Secondary,
            Width = 240,
            Height = 40,
            Location = new Point(20, y),
            Enabled = false,
        };
        _payNoopButton.Click += OnPayNoopClick;
        Controls.Add(_payNoopButton);

        _payRandomButton = new RoundedButton
        {
            Text = "Оплатить (шанс 50%)",
            BackColor = AppTheme.Accent,
            Width = 200,
            Height = 40,
            Location = new Point(280, y),
            Enabled = false,
        };
        _payRandomButton.Click += OnPayRandomClick;
        Controls.Add(_payRandomButton);
    }

    private async void OnSearchCellsClick(object? sender, EventArgs e)
    {
        _searchCellsButton.Enabled = false;
        _cellList.Items.Clear();

        var query = new CellSearchQuery
        {
            StartBooking = _startTimePicker.Value.ToUniversalTime(),
            TimeBooking = TimeSpan.FromHours((double)_hoursUpDown.Value),
            PageNumber = 0,
            PageSize = 50,
        };

        var cells = await ErrorHelper.TryAsync(
            () => ServiceLocator.CellService.GetCellsAsync(_storage.Id, query));

        if (cells is not null)
        {
            foreach (var cell in cells)
                _cellList.Items.Add(cell, false);
            _cellList.DisplayMember = nameof(CellModel.Name);

            // Загрузить цены
            var prices = await ErrorHelper.TryAsync(
                () => ServiceLocator.PriceService.GetPricesAsync(_storage.Id));
            if (prices is not null && prices.Count > 0)
            {
                var info = string.Join(", ", prices.Select(p => $"{p.CellClass}: {p.Price}₴/ч"));
                _priceInfoLabel.Text = $"Тарифы: {info}";
            }
        }

        _searchCellsButton.Enabled = true;
        _payNoopButton.Enabled = _cellList.Items.Count > 0;
        _payRandomButton.Enabled = _cellList.Items.Count > 0;
    }

    private async Task CreateBookingAndPay(bool guaranteed)
    {
        var selectedCells = _cellList.CheckedItems.Cast<CellModel>().ToList();
        if (selectedCells.Count == 0)
        {
            MessageBox.Show("Выберите хотя бы одну ячейку.", "Внимание",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!ServiceLocator.SessionService.IsAuthenticated())
        {
            MessageBox.Show("Для бронирования необходимо войти в систему.", "Внимание",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _payNoopButton.Enabled = false;
        _payRandomButton.Enabled = false;

        var form = new BookingCreateForm
        {
            StorageId = _storage.Id,
            CellIds = selectedCells.Select(c => c.Id).ToList(),
            StartTime = _startTimePicker.Value.ToUniversalTime(),
            BookingTime = TimeSpan.FromHours((double)_hoursUpDown.Value),
        };

        var booking = await ErrorHelper.TryAsync(
            () => ServiceLocator.BookingService.CreateBookingAsync(form));
        if (booking is null)
        {
            _payNoopButton.Enabled = true;
            _payRandomButton.Enabled = true;
            return;
        }

        // Оплата
        bool payOk;
        if (guaranteed)
        {
            payOk = await ErrorHelper.TryAsync(
                () => ServiceLocator.PaymentService.PayNoopAsync(),
                "Бронирование успешно оплачено!");
        }
        else
        {
            try
            {
                await ServiceLocator.PaymentService.PayRandomAsync();
                MessageBox.Show("Бронирование успешно оплачено!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                payOk = true;
            }
            catch (PaymentFailedException)
            {
                MessageBox.Show(
                    "Оплата не прошла. Попробуйте ещё раз или выберите гарантированную оплату.",
                    "Оплата не удалась", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _payNoopButton.Enabled = true;
                _payRandomButton.Enabled = true;
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _payNoopButton.Enabled = true;
                _payRandomButton.Enabled = true;
                return;
            }
        }

        if (payOk)
            BookingCompleted?.Invoke(this, EventArgs.Empty);
        else
        {
            _payNoopButton.Enabled = true;
            _payRandomButton.Enabled = true;
        }
    }

    private async void OnPayNoopClick(object? sender, EventArgs e)
        => await CreateBookingAndPay(guaranteed: true);

    private async void OnPayRandomClick(object? sender, EventArgs e)
        => await CreateBookingAndPay(guaranteed: false);
}