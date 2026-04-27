using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_UI_component.Controls;
using SklaDinya_desktop_UI_component.Helpers;

namespace SklaDinya_desktop_UI_component.Screens.Admin;

public class StorageApplicationsTab : UserControl
{
    private readonly DataGridView _grid;

    public StorageApplicationsTab()
    {
        Dock = DockStyle.Fill;
        BackColor = AppTheme.Background;

        var title = new Label { Text = "Заявки на пункты хранения", Font = AppTheme.FontTitle, ForeColor = AppTheme.Primary, Dock = DockStyle.Top, Height = 50, Padding = new Padding(24, 14, 0, 0) };

        var toolbar = new Panel { Dock = DockStyle.Top, Height = 52 };
        var approveBtn = new RoundedButton { Text = "Одобрить", BackColor = AppTheme.Secondary, Size = new Size(130, 34), Location = new Point(24, 9) };
        approveBtn.Click += OnApprove;
        var rejectBtn = new RoundedButton { Text = "Отклонить", BackColor = AppTheme.Danger, Size = new Size(130, 34), Location = new Point(166, 9) };
        rejectBtn.Click += OnReject;
        var refreshBtn = new RoundedButton { Text = "Обновить", BackColor = AppTheme.Primary, Size = new Size(110, 34), Location = new Point(308, 9) };
        refreshBtn.Click += async (_, _) => await LoadAsync();
        toolbar.Controls.AddRange([approveBtn, rejectBtn, refreshBtn]);

        _grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            BackgroundColor = AppTheme.PanelBackground, BorderStyle = BorderStyle.None,
            Font = AppTheme.FontRegular, RowHeadersVisible = false,
            ColumnHeadersDefaultCellStyle = { BackColor = AppTheme.Primary, ForeColor = Color.White, Font = AppTheme.FontMedium },
            EnableHeadersVisualStyles = false,
        };
        _grid.Columns.AddRange(
            new DataGridViewTextBoxColumn { HeaderText = "ID", Visible = false },
            new DataGridViewTextBoxColumn { HeaderText = "Название" },
            new DataGridViewTextBoxColumn { HeaderText = "Адрес" },
            new DataGridViewTextBoxColumn { HeaderText = "Статус" },
            new DataGridViewTextBoxColumn { HeaderText = "Создан" }
        );

        var gridWrapper = new Panel { Dock = DockStyle.Fill, Padding = new Padding(24, 0, 24, 16) };
        gridWrapper.Controls.Add(_grid);

        Controls.Add(gridWrapper);
        Controls.Add(toolbar);
        Controls.Add(title);

        Load += async (_, _) => await LoadAsync();
    }

    private async Task LoadAsync()
    {
        var storages = await ErrorHelper.TryAsync(() => ServiceLocator.StorageService.GetStoragesAsync(new StorageSearchQuery { PageNumber = 0, PageSize = 100 }));
        if (storages is null) return;
        _grid.Rows.Clear();
        foreach (var s in storages)
            _grid.Rows.Add(s.Id.ToString(), s.Name, s.Address, s.Status == StorageStatus.Created ? "Заявка" : "Активен", s.CreatedAt.ToString("dd.MM.yyyy"));
    }

    private Guid? SelId() => _grid.CurrentRow is not null && Guid.TryParse(_grid.CurrentRow.Cells[0].Value?.ToString(), out var id) ? id : null;

    private async void OnApprove(object? sender, EventArgs e)
    {
        var id = SelId(); if (id is null) return;
        if (MessageBox.Show("Одобрить?", "Подтверждение", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
        await ErrorHelper.TryAsync(() => ServiceLocator.StorageService.ApproveStorageAsync(id.Value), "Одобрено.");
        await LoadAsync();
    }

    private async void OnReject(object? sender, EventArgs e)
    {
        var id = SelId(); if (id is null) return;
        if (MessageBox.Show("Отклонить?", "Подтверждение", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
        await ErrorHelper.TryAsync(() => ServiceLocator.StorageService.RejectStorageAsync(id.Value), "Отклонено.");
        await LoadAsync();
    }
}
