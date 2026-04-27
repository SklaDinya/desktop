using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_UI_component.Controls;
using SklaDinya_desktop_UI_component.Helpers;

namespace SklaDinya_desktop_UI_component.Screens.Storage;

/// <summary>
/// Вкладка «Бронирования пункта» — бронирования в пункте оператора
/// </summary>
public class MyStorageBookingsTab : UserControl
{
    private readonly DataGridView _grid;
    private readonly DateTimePicker _startPicker;
    private readonly DateTimePicker _endPicker;
    private readonly RoundedButton _searchButton;

    public MyStorageBookingsTab()
    {
        Dock = DockStyle.Fill;
        BackColor = AppTheme.Background;

        var title = new Label
        {
            Text = "Бронирования пункта",
            Font = AppTheme.FontTitle,
            ForeColor = AppTheme.Primary,
            Dock = DockStyle.Top,
            Height = 50,
            Padding = new Padding(24, 14, 0, 0),
        };

        // Фильтры
        var filterPanel = new Panel { Dock = DockStyle.Top, Height = 50, Padding = new Padding(24, 8, 24, 8) };
        var fromLabel = new Label { Text = "С:", AutoSize = true, Location = new Point(0, 14), Font = AppTheme.FontMedium };
        _startPicker = new DateTimePicker
        {
            Format = DateTimePickerFormat.Short,
            Location = new Point(24, 10), Width = 130,
            Value = DateTime.Today,
        };
        var toLabel = new Label { Text = "По:", AutoSize = true, Location = new Point(168, 14), Font = AppTheme.FontMedium };
        _endPicker = new DateTimePicker
        {
            Format = DateTimePickerFormat.Short,
            Location = new Point(200, 10), Width = 130,
            Value = DateTime.Today.AddDays(30),
        };
        _searchButton = new RoundedButton
        {
            Text = "Найти",
            BackColor = AppTheme.Primary,
            Width = 100, Height = 34,
            Location = new Point(350, 8),
        };
        _searchButton.Click += async (_, _) => await LoadAsync();
        filterPanel.Controls.AddRange([fromLabel, _startPicker, toLabel, _endPicker, _searchButton]);

        // Таблица
        _grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            BackgroundColor = AppTheme.PanelBackground,
            BorderStyle = BorderStyle.None,
            Font = AppTheme.FontRegular,
            RowHeadersVisible = false,
        };
        _grid.Columns.AddRange(
            new DataGridViewTextBoxColumn { HeaderText = "Пользователь" },
            new DataGridViewTextBoxColumn { HeaderText = "Начало" },
            new DataGridViewTextBoxColumn { HeaderText = "Длительность" },
            new DataGridViewTextBoxColumn { HeaderText = "Ячеек" },
            new DataGridViewTextBoxColumn { HeaderText = "Статус" }
        );

        Controls.Add(_grid);
        Controls.Add(filterPanel);
        Controls.Add(title);

        Load += async (_, _) => await LoadAsync();
    }

    private async Task LoadAsync()
    {
        var query = new OperatorBookingSearchQuery
        {
            StartBooking = _startPicker.Value.ToUniversalTime(),
            EndBooking = _endPicker.Value.AddDays(1).ToUniversalTime(),
            PageNumber = 0,
            PageSize = 100,
        };

        var bookings = await ErrorHelper.TryAsync(
            () => ServiceLocator.BookingService.GetStorageBookingsAsync(query));

        if (bookings is null) return;

        _grid.Rows.Clear();
        foreach (var b in bookings)
        {
            var statusText = b.Status switch
            {
                BookingStatus.Created => "Создано",
                BookingStatus.Paid => "Оплачено",
                BookingStatus.InProcess => "В процессе",
                BookingStatus.Finished => "Завершено",
                BookingStatus.Canceled => "Отменено",
                _ => b.Status.ToString()
            };
            _grid.Rows.Add(
                b.User?.Name ?? "—",
                b.StartTime.ToLocalTime().ToString("dd.MM.yyyy HH:mm"),
                b.BookingTime.ToString(@"hh\:mm"),
                b.Cells.Count,
                statusText);
        }
    }
}
