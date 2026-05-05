using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_UI_component.Controls;
using SklaDinya_desktop_UI_component.Helpers;

namespace SklaDinya_desktop_UI_component.Screens.Account;

public class ProfileTab : UserControl
{
    private readonly RoundedTextBox _usernameField;
    private readonly RoundedTextBox _nameField;
    private readonly RoundedTextBox _emailField;
    private readonly RoundedTextBox _oldPasswordField;
    private readonly RoundedTextBox _newPasswordField;
    private readonly RoundedTextBox _confirmNewPasswordField;
    private readonly RoundedButton _saveButton;

    public ProfileTab()
    {
        Dock = DockStyle.Fill;
        BackColor = AppTheme.Background;
        AutoScroll = true;
        Padding = new Padding(32, 24, 32, 24);

        var title = new Label { Text = "Изменение личных данных", Font = AppTheme.FontTitle, ForeColor = AppTheme.Primary, AutoSize = true, Location = new Point(32, 24) };
        Controls.Add(title);

        int y = 68;
        const int w = 360;

        _usernameField = MakeField("Логин", ref y, w);
        _nameField = MakeField("Имя", ref y, w);
        _emailField = MakeField("Почта", ref y, w);
        _oldPasswordField = MakeField("Старый пароль", ref y, w, true);
        _newPasswordField = MakeField("Новый пароль", ref y, w, true);
        _confirmNewPasswordField = MakeField("Подтвердите новый пароль", ref y, w, true);

        _saveButton = new RoundedButton { Text = "Сохранить", BackColor = AppTheme.Secondary, Width = w, Location = new Point(32, y) };
        _saveButton.Click += OnSaveClick;
        Controls.Add(_saveButton);

        Load += async (_, _) => await LoadProfileAsync();
    }

    private RoundedTextBox MakeField(string placeholder, ref int y, int width, bool isPassword = false)
    {
        var lbl = new Label { Text = placeholder, Font = AppTheme.FontSmall, ForeColor = AppTheme.TextMuted, AutoSize = true, Location = new Point(32, y) };
        Controls.Add(lbl);
        y += 20;
        var field = new RoundedTextBox { Placeholder = placeholder, Width = width, Location = new Point(32, y) };
        if (isPassword) field.UsePasswordChar = true;
        Controls.Add(field);
        y += 48;
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
        if (!string.IsNullOrEmpty(_newPasswordField.Text) && _newPasswordField.Text != _confirmNewPasswordField.Text)
        { MessageBox.Show("Новые пароли не совпадают.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

        _saveButton.Enabled = false;
        var form = new MeUpdateForm
        {
            Username = OrNull(_usernameField.Text),
            Name = OrNull(_nameField.Text),
            Email = OrNull(_emailField.Text),
            OldPassword = OrNull(_oldPasswordField.Text),
            NewPassword = OrNull(_newPasswordField.Text),
        };
        await ErrorHelper.TryAsync(() => ServiceLocator.UserService.UpdateMeAsync(form), "Данные обновлены.");
        _saveButton.Enabled = true;
    }

    private static string? OrNull(string s) => string.IsNullOrWhiteSpace(s) ? null : s;
}
