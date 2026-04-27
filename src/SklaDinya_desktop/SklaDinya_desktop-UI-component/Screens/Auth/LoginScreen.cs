using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_UI_component.Controls;
using SklaDinya_desktop_UI_component.Helpers;

namespace SklaDinya_desktop_UI_component.Screens.Auth;

public class LoginScreen : UserControl
{
    private readonly RoundedTextBox _usernameField;
    private readonly RoundedTextBox _passwordField;
    private readonly RoundedButton _loginButton;

    public event EventHandler? LoginSuccess;
    public event EventHandler? NavigateToRegister;

    public LoginScreen()
    {
        Dock = DockStyle.Fill;
        BackColor = AppTheme.Background;

        var container = new Panel { Width = 380, BackColor = AppTheme.PanelBackground };

        int y = 24;
        var title = new Label
        {
            Text = "Вход в систему", Font = AppTheme.FontTitle, ForeColor = AppTheme.Primary,
            AutoSize = true, Location = new Point(24, y),
        };
        y += 52;

        _usernameField = new RoundedTextBox { Placeholder = "Логин", Location = new Point(24, y), Width = 332 };
        y += 52;
        _passwordField = new RoundedTextBox { Placeholder = "Пароль", UsePasswordChar = true, Location = new Point(24, y), Width = 332 };
        y += 60;

        _loginButton = new RoundedButton
        {
            Text = "Войти", BackColor = AppTheme.Primary,
            Location = new Point(24, y), Width = 332,
        };
        _loginButton.Click += OnLoginClick;
        y += 50;

        var registerLink = new Label
        {
            Text = "Ещё нет аккаунта? Зарегистрироваться",
            Font = AppTheme.FontLink, ForeColor = AppTheme.TextLink,
            AutoSize = true, Cursor = Cursors.Hand, Location = new Point(24, y),
        };
        registerLink.Click += (_, _) => NavigateToRegister?.Invoke(this, EventArgs.Empty);
        y += 30;

        container.Height = y;
        container.Controls.AddRange([title, _usernameField, _passwordField, _loginButton, registerLink]);
        Controls.Add(container);

        Resize += (_, _) =>
        {
            container.Location = new Point((Width - container.Width) / 2, (Height - container.Height) / 2);
            RoundedRenderer.ApplyRoundedRegion(container, 16);
        };
    }

    private async void OnLoginClick(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_usernameField.Text) || string.IsNullOrWhiteSpace(_passwordField.Text))
        { MessageBox.Show("Заполните все поля.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

        _loginButton.Enabled = false;
        var form = new LoginForm { Username = _usernameField.Text, Password = _passwordField.Text };
        var ok = await ErrorHelper.TryAsync(() => ServiceLocator.AuthService.LoginAsync(form));
        _loginButton.Enabled = true;
        if (ok) LoginSuccess?.Invoke(this, EventArgs.Empty);
    }
}
