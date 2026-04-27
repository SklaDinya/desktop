using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_UI_component.Controls;
using SklaDinya_desktop_UI_component.Helpers;

namespace SklaDinya_desktop_UI_component.Screens.Account;

/// <summary>
/// Вкладка «Мои данные» — редактирование профиля
/// </summary>
public class ProfileTab : UserControl
{
    private readonly RoundedTextBox _usernameField;
    private readonly RoundedTextBox _nameField;
    private readonly RoundedTextBox _emailField;
    private readonly RoundedTextBox _oldPasswordField;
    private readonly RoundedTextBox _newPasswordField;
    private readonly RoundedButton _saveButton;

    public ProfileTab()
    {
        Dock = DockStyle.Fill;
        BackColor = AppTheme.Background;
        AutoScroll = true;
        Padding = new Padding(32, 24, 32, 24);

        var title = new Label
        {
            Text = "Изменение личных данных",
            Font = AppTheme.FontTitle,
            ForeColor = AppTheme.Primary,
            AutoSize = true,
            Location = new Point(32, 24),
        };
        Controls.Add(title);

        int y = 70;
        const int w = 360;

        _usernameField = CreateField("Логин", ref y, w);
        _nameField = CreateField("Имя", ref y, w);
        _emailField = CreateField("Почта", ref y, w);
        _oldPasswordField = CreateField("Старый пароль", ref y, w, isPassword: true);
        _newPasswordField = CreateField("Новый пароль", ref y, w, isPassword: true);

        y += 12;
        _saveButton = new RoundedButton
        {
            Text = "Сохранить",
            BackColor = AppTheme.Secondary,
            Width = w,
            Location = new Point(32, y),
        };
        _saveButton.Click += OnSaveClick;
        Controls.Add(_saveButton);

        Load += async (_, _) => await LoadProfileAsync();
    }

    private RoundedTextBox CreateField(string placeholder, ref int y, int width, bool isPassword = false)
    {
        var lbl = new Label
        {
            Text = placeholder,
            Font = AppTheme.FontSmall,
            ForeColor = AppTheme.TextMuted,
            AutoSize = true,
            Location = new Point(32, y),
        };
        Controls.Add(lbl);
        y += 22;

        var field = new RoundedTextBox
        {
            Placeholder = placeholder,
            Width = width,
            Location = new Point(32, y),
        };
        if (isPassword) field.UsePasswordChar = true;
        Controls.Add(field);
        y += 50;
        return field;
    }

    private async Task LoadProfileAsync()
    {
        var me = await ErrorHelper.TryAsync(() => ServiceLocator.UserService.GetMeAsync());
        if (me is null) return;

        _usernameField.Text = me.Username;
        _nameField.Text = me.Name;
        _emailField.Text = me.Email ?? string.Empty;
    }

    private async void OnSaveClick(object? sender, EventArgs e)
    {
        _saveButton.Enabled = false;

        var form = new MeUpdateForm
        {
            Username = string.IsNullOrWhiteSpace(_usernameField.Text) ? null : _usernameField.Text,
            Name = string.IsNullOrWhiteSpace(_nameField.Text) ? null : _nameField.Text,
            Email = string.IsNullOrWhiteSpace(_emailField.Text) ? null : _emailField.Text,
            OldPassword = string.IsNullOrWhiteSpace(_oldPasswordField.Text) ? null : _oldPasswordField.Text,
            NewPassword = string.IsNullOrWhiteSpace(_newPasswordField.Text) ? null : _newPasswordField.Text,
        };

        await ErrorHelper.TryAsync(
            () => ServiceLocator.UserService.UpdateMeAsync(form),
            "Данные успешно обновлены.");

        _saveButton.Enabled = true;
    }
}
