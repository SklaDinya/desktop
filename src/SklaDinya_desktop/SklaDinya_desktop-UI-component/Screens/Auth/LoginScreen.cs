using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_UI_component.Controls;
using SklaDinya_desktop_UI_component.Helpers;

namespace SklaDinya_desktop_UI_component.Screens.Auth;

/// <summary>
/// Экран входа в систему
/// </summary>
public class LoginScreen : UserControl
{
    private readonly RoundedTextBox _usernameField;
    private readonly RoundedTextBox _passwordField;
    private readonly RoundedButton _loginButton;
    private readonly Label _registerLink;

    /// <summary>Вызывается после успешного входа</summary>
    public event EventHandler? LoginSuccess;

    /// <summary>Вызывается при нажатии на ссылку регистрации</summary>
    public event EventHandler? NavigateToRegister;

    public LoginScreen()
    {
        Dock = DockStyle.Fill;
        BackColor = AppTheme.Background;

        var container = new Panel
        {
            Width = 380,
            Height = 340,
            BackColor = AppTheme.PanelBackground,
        };

        var title = new Label
        {
            Text = "Вход в систему",
            Font = AppTheme.FontTitle,
            ForeColor = AppTheme.Primary,
            AutoSize = true,
            Location = new Point(24, 24),
        };

        _usernameField = new RoundedTextBox
        {
            Placeholder = "Логин",
            Location = new Point(24, 76),
            Width = 332,
        };

        _passwordField = new RoundedTextBox
        {
            Placeholder = "Пароль",
            UsePasswordChar = true,
            Location = new Point(24, 128),
            Width = 332,
        };

        _loginButton = new RoundedButton
        {
            Text = "Войти",
            BackColor = AppTheme.Primary,
            Location = new Point(24, 190),
            Width = 332,
        };
        _loginButton.Click += OnLoginClick;

        _registerLink = new Label
        {
            Text = "Ещё нет аккаунта? Зарегистрироваться",
            Font = AppTheme.FontLink,
            ForeColor = AppTheme.TextLink,
            AutoSize = true,
            Cursor = Cursors.Hand,
            Location = new Point(24, 248),
        };
        _registerLink.Click += (_, _) => NavigateToRegister?.Invoke(this, EventArgs.Empty);

        container.Controls.AddRange([title, _usernameField, _passwordField, _loginButton, _registerLink]);

        Controls.Add(container);

        Resize += (_, _) =>
        {
            container.Location = new Point(
                (Width - container.Width) / 2,
                (Height - container.Height) / 2);
            RoundedRenderer.ApplyRoundedRegion(container, 16);
        };
    }

    private async void OnLoginClick(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_usernameField.Text) ||
            string.IsNullOrWhiteSpace(_passwordField.Text))
        {
            MessageBox.Show("Заполните все поля.", "Внимание",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _loginButton.Enabled = false;
        var form = new LoginForm
        {
            Username = _usernameField.Text,
            Password = _passwordField.Text,
        };

        var ok = await ErrorHelper.TryAsync(() => ServiceLocator.AuthService.LoginAsync(form));
        _loginButton.Enabled = true;

        if (ok)
            LoginSuccess?.Invoke(this, EventArgs.Empty);
    }
}
