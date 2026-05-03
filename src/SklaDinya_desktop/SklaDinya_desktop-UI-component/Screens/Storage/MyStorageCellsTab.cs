using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_UI_component.Controls;
using SklaDinya_desktop_UI_component.Helpers;

namespace SklaDinya_desktop_UI_component.Screens.Storage;

/// <summary>
/// Вкладка «Ячейки пункта» — просмотр и добавление ячеек
/// </summary>
public class MyStorageCellsTab : UserControl
{
    private readonly DataGridView _grid;
    private readonly RoundedTextBox _nameField;
    private readonly RoundedTextBox _classField;
    private readonly RoundedButton _addButton;
    private readonly RoundedButton _refreshButton;

    public MyStorageCellsTab()
    {
        Dock = DockStyle.Fill;
        BackColor = AppTheme.Background;
        Padding = new Padding(24, 0, 24, 24);

        var title = new Label
        {
            Text = "Ячейки пункта хранения",
            Font = AppTheme.FontTitle,
            ForeColor = AppTheme.Primary,
            Dock = DockStyle.Top,
            Height = 50,
            Padding = new Padding(0, 14, 0, 0),
        };

        // ── Панель добавления ───────────────────────────────────────────
        var addPanel = new Panel { Dock = DockStyle.Top, Height = 56, Padding = new Padding(0, 8, 0, 8) };

        _nameField = new RoundedTextBox { Placeholder = "Название ячейки", Width = 200, Location = new Point(0, 8) };
        _classField = new RoundedTextBox { Placeholder = "Класс ячейки", Width = 180, Location = new Point(212, 8) };
        _addButton = new RoundedButton
        {
            Text = "Добавить",
            BackColor = AppTheme.Secondary,
            Width = 120, Height = 36,
            Location = new Point(404, 10),
        };
        _addButton.Click += OnAddClick;

        _refreshButton = new RoundedButton
        {
            Text = "Обновить",
            BackColor = AppTheme.Primary,
            Width = 110, Height = 36,
            Location = new Point(536, 10),
        };
        _refreshButton.Click += async (_, _) => await LoadAsync();

        addPanel.Controls.AddRange([_nameField, _classField, _addButton, _refreshButton]);

        // ── Таблица ─────────────────────────────────────────────────────
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
            new DataGridViewTextBoxColumn { HeaderText = "Название", DataPropertyName = "Name" },
            new DataGridViewTextBoxColumn { HeaderText = "Класс", DataPropertyName = "CellClass" },
            new DataGridViewTextBoxColumn { HeaderText = "Создана", DataPropertyName = "CreatedAt" }
        );

        Controls.Add(_grid);
        Controls.Add(addPanel);
        Controls.Add(title);

        Load += async (_, _) => await LoadAsync();
    }

    private async Task LoadAsync()
    {
        var query = new MyCellSearchQuery { PageNumber = 0, PageSize = 100 };
        var cells = await ErrorHelper.TryAsync(
            () => ServiceLocator.CellService.GetMyCellsAsync(query));

        if (cells is not null)
        {
            _grid.Rows.Clear();
            foreach (var cell in cells)
                _grid.Rows.Add(cell.Name, cell.CellClass, cell.CreatedAt.ToString("dd.MM.yyyy HH:mm"));
        }
    }

    private async void OnAddClick(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_nameField.Text) || string.IsNullOrWhiteSpace(_classField.Text))
        {
            MessageBox.Show("Заполните название и класс ячейки.", "Внимание",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _addButton.Enabled = false;
        var form = new CellCreateForm
        {
            Name = _nameField.Text,
            CellClass = _classField.Text,
        };
        var ok = await ErrorHelper.TryAsync(
            () => ServiceLocator.CellService.CreateCellAsync(form),
            "Ячейка добавлена.");

        if (ok)
        {
            _nameField.Text = string.Empty;
            _classField.Text = string.Empty;
            await LoadAsync();
        }
        _addButton.Enabled = true;
    }
}
