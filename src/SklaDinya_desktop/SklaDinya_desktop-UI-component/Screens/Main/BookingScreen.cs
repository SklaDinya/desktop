using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_UI_component.Controls;
using SklaDinya_desktop_UI_component.Helpers;

namespace SklaDinya_desktop_UI_component.Screens.Main;

public class BookingScreen : UserControl
{
    private readonly StorageModel _storage;
    private readonly DataGridView _grid;
    private readonly DateTimePicker _startPicker;
    private readonly NumericUpDown _hoursUpDown;
    private readonly ComboBox _classCombo;
    private readonly RoundedButton _filterButton;
    private readonly RoundedButton _payButton;
    private List<PriceModel> _prices = [];
    private List<CellModel> _allCells = [];

    public event EventHandler<BookingProceedEventArgs>? ProceedToPayment;
    public event EventHandler? BackRequested;

    public BookingScreen(StorageModel storage)
    {
        _storage = storage;
        Dock = DockStyle.Fill;
        BackColor = AppTheme.Background;

        // ── Top panel ───────────────────────────────────────────────────
        var topPanel = new Panel { Dock = DockStyle.Top, Height = 240, Padding = new Padding(24, 8, 24, 4) };

        var backBtn = new RoundedButton { Text = "← Назад", ButtonColor = AppTheme.TextMuted, Size = new Size(100, 32), Location = new Point(24, 8) };
        backBtn.Click += (_, _) => BackRequested?.Invoke(this, EventArgs.Empty);

        var title = new Label { Text = $"Бронирование — {storage.Name}", Font = AppTheme.FontTitle, ForeColor = AppTheme.Primary, AutoSize = true, Location = new Point(24, 48) };

        // Дата начала
        topPanel.Controls.Add(new Label { Text = "Дата и время начала:", Font = AppTheme.FontMedium, AutoSize = true, Location = new Point(24, 88) });
        _startPicker = new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "dd.MM.yyyy HH:mm", Location = new Point(24, 112), Width = 220, Value = DateTime.Now.AddHours(1) };

        // Длительность
        topPanel.Controls.Add(new Label { Text = "Длительность (часов):", Font = AppTheme.FontMedium, AutoSize = true, Location = new Point(270, 88) });
        _hoursUpDown = new NumericUpDown { Minimum = 1, Maximum = 720, Value = 2, Location = new Point(270, 112), Width = 100 };

        // Фильтр по классу — выпадающий список
        topPanel.Controls.Add(new Label { Text = "Класс ячейки:", Font = AppTheme.FontMedium, AutoSize = true, Location = new Point(24, 150) });
        _classCombo = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Location = new Point(24, 174), Width = 200,
            Font = AppTheme.FontRegular,
        };
        _classCombo.Items.Add("Все классы");
        _classCombo.SelectedIndex = 0;

        _filterButton = new RoundedButton { Text = "Фильтровать", ButtonColor = AppTheme.Primary, Size = new Size(130, 36), Location = new Point(240, 172) };
        _filterButton.Click += (_, _) => ApplyFilter();

        topPanel.Controls.AddRange([backBtn, title, _startPicker, _hoursUpDown, _classCombo, _filterButton]);

        // ── Grid ────────────────────────────────────────────────────────
        _grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            AllowUserToAddRows = false, AllowUserToDeleteRows = false,
            ReadOnly = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = true,
            BackgroundColor = AppTheme.PanelBackground, BorderStyle = BorderStyle.None,
            Font = AppTheme.FontRegular, RowHeadersVisible = false,
            ColumnHeadersDefaultCellStyle = { BackColor = AppTheme.Primary, ForeColor = Color.White, Font = AppTheme.FontMedium, Alignment = DataGridViewContentAlignment.MiddleCenter },
            EnableHeadersVisualStyles = false,
            EditMode = DataGridViewEditMode.EditOnEnter,
        };

        var colCheck = new DataGridViewCheckBoxColumn
        {
            HeaderText = "Выбор", Width = 60, Name = "colSelect",
            DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter },
        };
        _grid.Columns.Add(colCheck);
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Название", Name = "colName", ReadOnly = true });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Класс", Name = "colClass", ReadOnly = true });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Цена", Name = "colPrice", ReadOnly = true });

        // Клик по любой ячейке строки — переключает чекбокс
        _grid.CellClick += (_, args) =>
        {
            if (args.RowIndex < 0) return;
            var checkCell = _grid.Rows[args.RowIndex].Cells["colSelect"];
            if (args.ColumnIndex != 0)
                checkCell.Value = checkCell.Value is true ? (object)false : true;
            _grid.InvalidateRow(args.RowIndex);
        };

        // ── Bottom bar ──────────────────────────────────────────────────
        var bottomPanel = new Panel { Dock = DockStyle.Bottom, Height = 56, Padding = new Padding(24, 8, 24, 8) };
        _payButton = new RoundedButton { Text = "К оплате", ButtonColor = AppTheme.Secondary, Size = new Size(160, 40) };
        _payButton.Click += OnPayClick;
        bottomPanel.Controls.Add(_payButton);
        bottomPanel.Resize += (_, _) => _payButton.Location = new Point(bottomPanel.Width - _payButton.Width - 24, 8);

        var gridWrapper = new Panel { Dock = DockStyle.Fill, Padding = new Padding(24, 0, 24, 0) };
        gridWrapper.Controls.Add(_grid);

        Controls.Add(gridWrapper);
        Controls.Add(bottomPanel);
        Controls.Add(topPanel);

        Load += async (_, _) => await LoadCellsAsync();
    }

    private async Task LoadCellsAsync()
    {
        _filterButton.Enabled = false;

        var query = new CellSearchQuery
        {
            StartBooking = _startPicker.Value.ToUniversalTime(),
            TimeBooking = TimeSpan.FromHours((double)_hoursUpDown.Value),
            PageNumber = 0, PageSize = 50,
        };

        _allCells = await ErrorHelper.TryAsync(() => ServiceLocator.CellService.GetCellsAsync(_storage.Id, query)) ?? [];
        _prices = await ErrorHelper.TryAsync(() => ServiceLocator.PriceService.GetPricesAsync(_storage.Id)) ?? [];

        // Заполнить ComboBox классами
        var classes = _allCells.Select(c => c.CellClass).Distinct().OrderBy(c => c).ToList();
        _classCombo.Items.Clear();
        _classCombo.Items.Add("Все классы");
        foreach (var cls in classes) _classCombo.Items.Add(cls);
        _classCombo.SelectedIndex = 0;

        ApplyFilter();
        _filterButton.Enabled = true;
    }

    private void ApplyFilter()
    {
        _grid.Rows.Clear();
        var filter = _classCombo.SelectedIndex <= 0 ? null : _classCombo.SelectedItem?.ToString();
        var filtered = filter is null ? _allCells : _allCells.Where(c => c.CellClass == filter).ToList();

        foreach (var cell in filtered)
        {
            var price = _prices.FirstOrDefault(p => p.CellClass == cell.CellClass);
            var priceStr = price is not null ? $"{price.Price:F2} ₽/час" : "—";
            var idx = _grid.Rows.Add(false, cell.Name, cell.CellClass, priceStr);
            _grid.Rows[idx].Tag = cell;
        }
    }

    private async void OnPayClick(object? sender, EventArgs e)
    {
        if (!ServiceLocator.SessionService.IsAuthenticated())
        { MessageBox.Show("Необходимо войти в систему.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

        _grid.EndEdit();

        var selectedCells = new List<CellModel>();
        foreach (DataGridViewRow row in _grid.Rows)
            if (row.Cells["colSelect"].Value is true && row.Tag is CellModel cell)
                selectedCells.Add(cell);

        if (selectedCells.Count == 0)
        { MessageBox.Show("Выберите хотя бы одну ячейку.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

        var form = new BookingCreateForm
        {
            StorageId = _storage.Id,
            CellIds = selectedCells.Select(c => c.Id).ToList(),
            StartTime = _startPicker.Value.ToUniversalTime(),
            BookingTime = TimeSpan.FromHours((double)_hoursUpDown.Value),
        };

        // Создаём бронирование прямо здесь — это даёт нам цену, посчитанную
        // на сервере (поле booking.Price), и сохранённый в BookingService
        // LastReceipt для последующего вызова оплаты. Раньше создание
        // выполнялось внутри PaymentScreen.OnPayClick, но тогда цену
        // приходилось считать на клиенте — теперь это не нужно.
        _payButton.Enabled = false;
        var booking = await ErrorHelper.TryAsync(
            () => ServiceLocator.BookingService.CreateBookingAsync(form));
        _payButton.Enabled = true;

        if (booking is null) return;

        ProceedToPayment?.Invoke(this, new BookingProceedEventArgs(booking));
    }
}

/// <summary>
/// Аргументы события «перейти к оплате»: уже созданное на сервере бронирование.
/// Содержит итоговую стоимость <see cref="BookingModel.Price"/>, посчитанную
/// бэкендом, и идентификатор — больше ничего повторно создавать не нужно.
/// </summary>
public sealed class BookingProceedEventArgs : EventArgs
{
    public BookingModel Booking { get; }

    public BookingProceedEventArgs(BookingModel booking)
    {
        Booking = booking;
    }
}
