using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_UI_component.Controls;
using SklaDinya_desktop_UI_component.Helpers;
using SklaDinya_desktop_UI_component.Screens.Account;
using SklaDinya_desktop_UI_component.Screens.Auth;
using SklaDinya_desktop_UI_component.Screens.Main;

namespace SklaDinya_desktop_UI_component;

/// <summary>
/// Главное окно приложения SklaDinya.
/// Заголовок: логотип, поле поиска, кнопки авторизации / ЛК.
/// Контент: переключается между экранами.
/// </summary>
public class MainForm : Form
{
    // ── Заголовок ───────────────────────────────────────────────────────
    private readonly Panel _header;
    private readonly PictureBox _logo;
    private readonly RoundedTextBox _searchField;
    private readonly RoundedButton _searchButton;
    private readonly RoundedButton _loginHeaderBtn;
    private readonly RoundedButton _registerHeaderBtn;
    private readonly RoundedButton _accountHeaderBtn;
    private readonly RoundedButton _logoutHeaderBtn;

    // ── Контент ─────────────────────────────────────────────────────────
    private readonly Panel _contentPanel;
    private HomeScreen? _homeScreen;

    public MainForm()
    {
        Text = "SklaDinya — Сервис бронирования ячеек хранения";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(1100, 720);
        MinimumSize = new Size(900, 600);
        BackColor = AppTheme.Background;

        // Попытка загрузить иконку
        TrySetIcon();

        // ── Заголовок ───────────────────────────────────────────────────
        _header = new Panel
        {
            Dock = DockStyle.Top,
            Height = AppTheme.HeaderHeight,
            BackColor = AppTheme.HeaderBackground,
            Padding = new Padding(12, 8, 12, 8),
        };

        _logo = new PictureBox
        {
            Size = new Size(130, 40),
            Location = new Point(12, 8),
            SizeMode = PictureBoxSizeMode.Zoom,
            Cursor = Cursors.Hand,
            BackColor = Color.Transparent,
        };
        TryLoadLogo();
        _logo.Click += (_, _) => NavigateHome();

        _searchField = new RoundedTextBox
        {
            Placeholder = "Название или адрес пункта...",
            Width = 280,
            Height = 36,
            Location = new Point(156, 10),
        };

        _searchButton = new RoundedButton
        {
            Text = "Найти",
            BackColor = AppTheme.Secondary,
            Width = 80,
            Height = 36,
            Location = new Point(448, 10),
        };
        _searchButton.Click += OnSearchClick;

        // Кнопки авторизации (до входа)
        _loginHeaderBtn = new RoundedButton
        {
            Text = "Вход",
            BackColor = AppTheme.White,
            ForeColor = AppTheme.Primary,
            Width = 90, Height = 36,
        };
        _loginHeaderBtn.Click += (_, _) => ShowScreen(CreateLoginScreen());

        _registerHeaderBtn = new RoundedButton
        {
            Text = "Регистрация",
            BackColor = AppTheme.Accent,
            Width = 120, Height = 36,
        };
        _registerHeaderBtn.Click += (_, _) => ShowScreen(CreateRegisterScreen());

        // Кнопки после входа
        _accountHeaderBtn = new RoundedButton
        {
            Text = "Личный кабинет",
            BackColor = AppTheme.White,
            ForeColor = AppTheme.Primary,
            Width = 140, Height = 36,
            Visible = false,
        };
        _accountHeaderBtn.Click += (_, _) => ShowScreen(new AccountScreen());

        _logoutHeaderBtn = new RoundedButton
        {
            Text = "Выйти",
            BackColor = AppTheme.Danger,
            Width = 80, Height = 36,
            Visible = false,
        };
        _logoutHeaderBtn.Click += OnLogoutClick;

        _header.Controls.AddRange([
            _logo, _searchField, _searchButton,
            _loginHeaderBtn, _registerHeaderBtn,
            _accountHeaderBtn, _logoutHeaderBtn
        ]);
        _header.Resize += (_, _) => LayoutHeaderButtons();

        // ── Контент ─────────────────────────────────────────────────────
        _contentPanel = new Panel
        {
            Dock = DockStyle.Fill,
        };

        Controls.Add(_contentPanel);
        Controls.Add(_header);

        NavigateHome();
    }

    // ── Навигация ───────────────────────────────────────────────────────

    private void NavigateHome()
    {
        _homeScreen = new HomeScreen();
        _homeScreen.BookingRequested += OnBookingRequested;
        ShowScreen(_homeScreen);
        _ = _homeScreen.LoadDefaultStoragesAsync();
    }

    private void ShowScreen(UserControl screen)
    {
        _contentPanel.Controls.Clear();
        screen.Dock = DockStyle.Fill;
        _contentPanel.Controls.Add(screen);
    }

    private LoginScreen CreateLoginScreen()
    {
        var screen = new LoginScreen();
        screen.LoginSuccess += (_, _) => { UpdateAuthButtons(); NavigateHome(); };
        screen.NavigateToRegister += (_, _) => ShowScreen(CreateRegisterScreen());
        return screen;
    }

    private RegisterScreen CreateRegisterScreen()
    {
        var screen = new RegisterScreen();
        screen.RegisterSuccess += (_, _) => { UpdateAuthButtons(); NavigateHome(); };
        screen.NavigateToLogin += (_, _) => ShowScreen(CreateLoginScreen());
        return screen;
    }

    // ── Бронирование ────────────────────────────────────────────────────

    private void OnBookingRequested(object? sender, StorageModel storage)
    {
        if (!ServiceLocator.SessionService.IsAuthenticated())
        {
            MessageBox.Show("Для бронирования необходимо войти в систему.", "Внимание",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            ShowScreen(CreateLoginScreen());
            return;
        }

        var bookingScreen = new BookingScreen(storage);
        bookingScreen.BookingCompleted += (_, _) => NavigateHome();
        bookingScreen.BackRequested += (_, _) => NavigateHome();
        ShowScreen(bookingScreen);
    }

    // ── Поиск ───────────────────────────────────────────────────────────

    private async void OnSearchClick(object? sender, EventArgs e)
    {
        var text = _searchField.Text.Trim();
        if (string.IsNullOrEmpty(text)) { NavigateHome(); return; }

        var query = new StorageSearchQuery
        {
            Name = text,
            Address = text,
            PageNumber = 0,
            PageSize = 20,
        };

        if (_homeScreen is null || !_contentPanel.Controls.Contains(_homeScreen))
        {
            _homeScreen = new HomeScreen();
            _homeScreen.BookingRequested += OnBookingRequested;
            ShowScreen(_homeScreen);
        }

        var results = await ErrorHelper.TryAsync(
            () => ServiceLocator.StorageService.GetStoragesAsync(query));

        if (results is not null)
            _homeScreen.ShowResults(results);
    }

    // ── Выход ───────────────────────────────────────────────────────────

    private void OnLogoutClick(object? sender, EventArgs e)
    {
        ServiceLocator.AuthService.Logout();
        UpdateAuthButtons();
        NavigateHome();
    }

    // ── Кнопки авторизации ──────────────────────────────────────────────

    private void UpdateAuthButtons()
    {
        var authed = ServiceLocator.SessionService.IsAuthenticated();
        _loginHeaderBtn.Visible = !authed;
        _registerHeaderBtn.Visible = !authed;
        _accountHeaderBtn.Visible = authed;
        _logoutHeaderBtn.Visible = authed;
        LayoutHeaderButtons();
    }

    private void LayoutHeaderButtons()
    {
        int right = _header.Width - 16;

        if (_logoutHeaderBtn.Visible)
        {
            _logoutHeaderBtn.Location = new Point(right - _logoutHeaderBtn.Width, 10);
            right -= _logoutHeaderBtn.Width + 8;
            _accountHeaderBtn.Location = new Point(right - _accountHeaderBtn.Width, 10);
        }
        else
        {
            _registerHeaderBtn.Location = new Point(right - _registerHeaderBtn.Width, 10);
            right -= _registerHeaderBtn.Width + 8;
            _loginHeaderBtn.Location = new Point(right - _loginHeaderBtn.Width, 10);
        }
    }

    // ── Логотип / Иконка ────────────────────────────────────────────────

    private void TryLoadLogo()
    {
        var paths = new[]
        {
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "skladinya.png"),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "skladinya.png"),
        };
        foreach (var p in paths)
        {
            if (File.Exists(p))
            {
                _logo.Image = Image.FromFile(p);
                return;
            }
        }
        // Fallback — текст вместо логотипа
        var fallbackLabel = new Label
        {
            Text = "SklaDinya",
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            ForeColor = AppTheme.TextLight,
            AutoSize = true,
            BackColor = Color.Transparent,
            Location = new Point(16, 14),
            Cursor = Cursors.Hand,
        };
        fallbackLabel.Click += (_, _) => NavigateHome();
        _header.Controls.Add(fallbackLabel);
    }

    private void TrySetIcon()
    {
        var icoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "skladinya.ico");
        if (File.Exists(icoPath))
            Icon = new Icon(icoPath);
    }
}
