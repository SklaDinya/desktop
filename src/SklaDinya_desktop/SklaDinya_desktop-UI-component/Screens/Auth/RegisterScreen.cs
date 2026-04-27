using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_UI_component.Controls;
using SklaDinya_desktop_UI_component.Helpers;

namespace SklaDinya_desktop_UI_component.Screens.Auth;

/// <summary>
/// Экран регистрации нового пользователя
/// </summary>
public class RegisterScreen : UserControl
{
    private readonly RoundedTextBox _usernameField;
    private readonly RoundedTextBox _nameField;
    private readonly RoundedTextBox _emailField;
    private readonly RoundedTextBox _passwordField;
    private readonly RoundedTextBox _confirmPasswordField;
    private readonly RoundedButton _registerButton;

    /// <summary>Вызывается после успешной регистрации</summary>
    public event EventHandler? RegisterSuccess;

    /// <summary>Переход на экран входа</summary>
    public event EventHandler? NavigateToLogin;

    public RegisterScreen()
    {
        Dock = DockStyle.Fill;
        BackColor = AppTheme.Background;

        var container = new Panel
        {
            Width = 380,
            Height = 480,
            BackColor = AppTheme.PanelBackground,
        };

        var title = new Label
        {
            Text = "Регистрация",
            Font = AppTheme.FontTitle,
            ForeColor = AppTheme.Primary,
            AutoSize = true,
            Location = new Point(24, 20),
        };

        int y = 64;
        const int step = 50;

        _usernameField = new RoundedTextBox { Placeholder = "Логин", Location = new Point(24, y), Width = 332 };
        y += step;
        _nameField = new RoundedTextBox { Placeholder = "ФИО", Location = new Point(24, y), Width = 332 };
        y += step;
        _emailField = new RoundedTextBox { Placeholder = "Почта", Location = new Point(24, y), Width = 332 };
        y += step;
        _passwordField = new RoundedTextBox { Placeholder = "Пароль", UsePasswordChar = true, Location = new Point(24, y), Width = 332 };
        y += step;
        _confirmPasswordField = new RoundedTextBox { Placeholder = "Повторите пароль", UsePasswordChar = true, Location = new Point(24, y), Width = 332 };
        y += step + 12;

        _registerButton = new RoundedButton
        {
            Text = "Зарегистрироваться",
            BackColor = AppTheme.Secondary,
            Location = new Point(24, y),
            Width = 332,
        };
        _registerButton.Click += OnRegisterClick;
        y += 52;

        var loginLink = new Label
        {
            Text = "Уже есть аккаунт? Войти",
            Font = AppTheme.FontLink,
            ForeColor = AppTheme.TextLink,
            AutoSize = true,
            Cursor = Cursors.Hand,
            Location = new Point(24, y),
        };
        loginLink.Click += (_, _) => NavigateToLogin?.Invoke(this, EventArgs.Empty);

        container.Controls.AddRange([
            title, _usernameField, _nameField, _emailField,
            _passwordField, _confirmPasswordField, _registerButton, loginLink
        ]);
        Controls.Add(container);

        Resize += (_, _) =>
        {
            container.Location = new Point(
                (Width - container.Width) / 2,
                (Height - container.Height) / 2);
            RoundedRenderer.ApplyRoundedRegion(container, 16);
        };
    }

    private async void OnRegisterClick(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_usernameField.Text) ||
            string.IsNullOrWhiteSpace(_nameField.Text) ||
            string.IsNullOrWhiteSpace(_passwordField.Text))
        {
            MessageBox.Show("Заполните обязательные поля.", "Внимание",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_passwordField.Text != _confirmPasswordField.Text)
        {
            MessageBox.Show("Пароли не совпадают.", "Внимание",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _registerButton.Enabled = false;
        var form = new RegistrationForm
        {
            Username = _usernameField.Text,
            Password = _passwordField.Text,
            Name = _nameField.Text,
            Email = string.IsNullOrWhiteSpace(_emailField.Text) ? null : _emailField.Text,
        };

        var ok = await ErrorHelper.TryAsync(() => ServiceLocator.AuthService.RegisterAsync(form));
        _registerButton.Enabled = true;

        if (ok)
            RegisterSuccess?.Invoke(this, EventArgs.Empty);
    }
}
