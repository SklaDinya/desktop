using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_UI_component.Controls;
using SklaDinya_desktop_UI_component.Helpers;

namespace SklaDinya_desktop_UI_component.Screens.Admin;

/// <summary>
/// Вкладка «Заявки на пункты» — одобрение/отклонение заявок администратором
/// </summary>
public class StorageApplicationsTab : UserControl
{
    private readonly DataGridView _grid;
    private readonly RoundedButton _approveButton;
    private readonly RoundedButton _rejectButton;
    private readonly RoundedButton _refreshButton;

    public StorageApplicationsTab()
    {
        Dock = DockStyle.Fill;
        BackColor = AppTheme.Background;

        var title = new Label
        {
            Text = "Заявки на создание пунктов хранения",
            Font = AppTheme.FontTitle,
            ForeColor = AppTheme.Primary,
            Dock = DockStyle.Top,
            Height = 50,
            Padding = new Padding(24, 14, 0, 0),
        };

        var toolbar = new Panel { Dock = DockStyle.Top, Height = 50, Padding = new Padding(24, 8, 24, 8) };

        _approveButton = new RoundedButton
        {
            Text = "Одобрить",
            BackColor = AppTheme.Secondary,
            Width = 130, Height = 34,
            Location = new Point(0, 8),
        };
        _approveButton.Click += OnApproveClick;

        _rejectButton = new RoundedButton
        {
            Text = "Отклонить",
            BackColor = AppTheme.Danger,
            Width = 130, Height = 34,
            Location = new Point(142, 8),
        };
        _rejectButton.Click += OnRejectClick;

        _refreshButton = new RoundedButton
        {
            Text = "Обновить",
            BackColor = AppTheme.Primary,
            Width = 110, Height = 34,
            Location = new Point(284, 8),
        };
        _refreshButton.Click += async (_, _) => await LoadAsync();

        toolbar.Controls.AddRange([_approveButton, _rejectButton, _refreshButton]);

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
            new DataGridViewTextBoxColumn { HeaderText = "ID", Visible = false },
            new DataGridViewTextBoxColumn { HeaderText = "Название" },
            new DataGridViewTextBoxColumn { HeaderText = "Адрес" },
            new DataGridViewTextBoxColumn { HeaderText = "Описание" },
            new DataGridViewTextBoxColumn { HeaderText = "Статус" },
            new DataGridViewTextBoxColumn { HeaderText = "Создан" }
        );

        Controls.Add(_grid);
        Controls.Add(toolbar);
        Controls.Add(title);

        Load += async (_, _) => await LoadAsync();
    }

    private async Task LoadAsync()
    {
        var query = new StorageSearchQuery { PageNumber = 0, PageSize = 100 };
        var storages = await ErrorHelper.TryAsync(
            () => ServiceLocator.StorageService.GetStoragesAsync(query));

        if (storages is null) return;

        _grid.Rows.Clear();
        foreach (var s in storages)
        {
            var statusText = s.Status == StorageStatus.Created ? "Заявка" : "Активен";
            _grid.Rows.Add(
                s.Id.ToString(),
                s.Name,
                s.Address,
                s.Description ?? "—",
                statusText,
                s.CreatedAt.ToString("dd.MM.yyyy"));
        }
    }

    private Guid? GetSelectedId()
    {
        if (_grid.CurrentRow is null) return null;
        var idStr = _grid.CurrentRow.Cells[0].Value?.ToString();
        return Guid.TryParse(idStr, out var id) ? id : null;
    }

    private async void OnApproveClick(object? sender, EventArgs e)
    {
        var id = GetSelectedId();
        if (id is null) return;

        var confirm = MessageBox.Show("Одобрить заявку?", "Подтверждение",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        await ErrorHelper.TryAsync(
            () => ServiceLocator.StorageService.ApproveStorageAsync(id.Value),
            "Заявка одобрена.");
        await LoadAsync();
    }

    private async void OnRejectClick(object? sender, EventArgs e)
    {
        var id = GetSelectedId();
        if (id is null) return;

        var confirm = MessageBox.Show("Отклонить заявку?", "Подтверждение",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        await ErrorHelper.TryAsync(
            () => ServiceLocator.StorageService.RejectStorageAsync(id.Value),
            "Заявка отклонена.");
        await LoadAsync();
    }
}
