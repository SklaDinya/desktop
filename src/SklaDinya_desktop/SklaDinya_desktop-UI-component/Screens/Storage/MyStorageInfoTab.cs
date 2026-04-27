using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_UI_component.Controls;
using SklaDinya_desktop_UI_component.Helpers;

namespace SklaDinya_desktop_UI_component.Screens.Storage;

/// <summary>
/// Вкладка «Мой пункт хранения» — просмотр и редактирование информации
/// </summary>
public class MyStorageInfoTab : UserControl
{
    private readonly RoundedTextBox _nameField;
    private readonly RoundedTextBox _addressField;
    private readonly RoundedTextBox _descriptionField;
    private readonly Label _statusLabel;
    private readonly RoundedButton _saveButton;

    public MyStorageInfoTab()
    {
        Dock = DockStyle.Fill;
        BackColor = AppTheme.Background;
        AutoScroll = true;

        var title = new Label
        {
            Text = "Информация о пункте хранения",
            Font = AppTheme.FontTitle,
            ForeColor = AppTheme.Primary,
            AutoSize = true,
            Location = new Point(32, 24),
        };
        Controls.Add(title);

        _statusLabel = new Label
        {
            Font = AppTheme.FontMedium,
            AutoSize = true,
            Location = new Point(32, 60),
        };
        Controls.Add(_statusLabel);

        int y = 92;
        const int w = 400;

        _nameField = MakeField("Название", ref y, w);
        _addressField = MakeField("Адрес", ref y, w);
        _descriptionField = MakeField("Описание", ref y, w);

        _saveButton = new RoundedButton
        {
            Text = "Сохранить изменения",
            BackColor = AppTheme.Secondary,
            Width = w,
            Location = new Point(32, y),
        };
        _saveButton.Click += OnSaveClick;
        Controls.Add(_saveButton);

        Load += async (_, _) => await LoadAsync();
    }

    private RoundedTextBox MakeField(string placeholder, ref int y, int w)
    {
        var lbl = new Label { Text = placeholder, Font = AppTheme.FontSmall, ForeColor = AppTheme.TextMuted, AutoSize = true, Location = new Point(32, y) };
        Controls.Add(lbl);
        y += 22;
        var f = new RoundedTextBox { Placeholder = placeholder, Width = w, Location = new Point(32, y) };
        Controls.Add(f);
        y += 50;
        return f;
    }

    private async Task LoadAsync()
    {
        var storage = await ErrorHelper.TryAsync(() => ServiceLocator.StorageService.GetMyStorageAsync());
        if (storage is null) return;

        _nameField.Text = storage.Name;
        _addressField.Text = storage.Address;
        _descriptionField.Text = storage.Description ?? string.Empty;
        _statusLabel.Text = $"Статус: {(storage.Status == SklaDinya_desktop_BL_component.Enums.StorageStatus.Active ? "Активен" : "Создан")}";
        _statusLabel.ForeColor = storage.Status == SklaDinya_desktop_BL_component.Enums.StorageStatus.Active
            ? AppTheme.Secondary : AppTheme.Accent;
    }

    private async void OnSaveClick(object? sender, EventArgs e)
    {
        _saveButton.Enabled = false;
        var form = new StorageUpdateForm
        {
            Name = string.IsNullOrWhiteSpace(_nameField.Text) ? null : _nameField.Text,
            Address = string.IsNullOrWhiteSpace(_addressField.Text) ? null : _addressField.Text,
            Description = string.IsNullOrWhiteSpace(_descriptionField.Text) ? null : _descriptionField.Text,
        };
        await ErrorHelper.TryAsync(
            () => ServiceLocator.StorageService.UpdateMyStorageAsync(form),
            "Данные пункта обновлены.");
        _saveButton.Enabled = true;
    }
}
