using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_UI_component.Controls;
using SklaDinya_desktop_UI_component.Helpers;
using SklaDinya_desktop_UI_component.Screens.Account;
using SklaDinya_desktop_UI_component.Screens.Auth;
using SklaDinya_desktop_UI_component.Screens.Main;

namespace SklaDinya_desktop_UI_component;

public class MainForm : Form
{
    private readonly Panel _header;
    private readonly PictureBox _logo;
    private readonly RoundedTextBox _searchField;
    private readonly RoundedButton _searchButton;
    private readonly RoundedButton _loginHeaderBtn;
    private readonly RoundedButton _registerHeaderBtn;
    private readonly RoundedButton _accountHeaderBtn;
    private readonly RoundedButton _logoutHeaderBtn;
    private readonly Panel _contentPanel;
    private HomeScreen? _homeScreen;

    public MainForm()
    {
        Text = "SklaDinya — Сервис бронирования ячеек хранения";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(1100, 720);
        MinimumSize = new Size(900, 600);
        BackColor = AppTheme.Background;
        TrySetIcon();

        _header = new Panel { Dock = DockStyle.Top, Height = AppTheme.HeaderHeight, BackColor = AppTheme.HeaderBackground };

        _logo = new PictureBox { Size = new Size(130, 40), Location = new Point(12, 8), SizeMode = PictureBoxSizeMode.Zoom, Cursor = Cursors.Hand, BackColor = Color.Transparent };
        TryLoadLogo();
        _logo.Click += (_, _) => NavigateHome();

        _searchField = new RoundedTextBox { Placeholder = "Название или адрес пункта...", Width = 280, Height = 36, Location = new Point(156, 10) };
        _searchButton = new RoundedButton { Text = "Найти", BackColor = AppTheme.Secondary, Width = 80, Height = 36, Location = new Point(448, 10) };
        _searchButton.Click += OnSearchClick;

        _loginHeaderBtn = new RoundedButton { Text = "Вход", BackColor = AppTheme.White, ForeColor = AppTheme.Primary, Width = 90, Height = 36 };
        _loginHeaderBtn.Click += (_, _) => ShowScreen(CreateLoginScreen());
        _registerHeaderBtn = new RoundedButton { Text = "Регистрация", BackColor = AppTheme.Accent, Width = 120, Height = 36 };
        _registerHeaderBtn.Click += (_, _) => ShowScreen(CreateRegisterScreen());

        _accountHeaderBtn = new RoundedButton { Text = "Личный кабинет", BackColor = AppTheme.White, ForeColor = AppTheme.Primary, Width = 155, Height = 36, Visible = false };
        _accountHeaderBtn.Click += (_, _) => ShowScreen(new AccountScreen());
        _logoutHeaderBtn = new RoundedButton { Text = "Выйти", BackColor = AppTheme.Danger, Width = 80, Height = 36, Visible = false };
        _logoutHeaderBtn.Click += OnLogoutClick;

        _header.Controls.AddRange([_logo, _searchField, _searchButton, _loginHeaderBtn, _registerHeaderBtn, _accountHeaderBtn, _logoutHeaderBtn]);
        _header.Resize += (_, _) => LayoutHeaderButtons();

        _contentPanel = new Panel { Dock = DockStyle.Fill };

        Controls.Add(_contentPanel);
        Controls.Add(_header);

        NavigateHome();
    }

    private void NavigateHome()
    {
        _homeScreen = new HomeScreen();
        _homeScreen.BookingRequested += OnBookingRequested;
        ShowScreen(_homeScreen);
        _ = _homeScreen.LoadDefaultStoragesAsync();
    }

    private void ShowScreen(UserControl screen) { _contentPanel.Controls.Clear(); screen.Dock = DockStyle.Fill; _contentPanel.Controls.Add(screen); }

    private LoginScreen CreateLoginScreen()
    {
        var s = new LoginScreen();
        s.LoginSuccess += (_, _) => { UpdateAuthButtons(); NavigateHome(); };
        s.NavigateToRegister += (_, _) => ShowScreen(CreateRegisterScreen());
        return s;
    }

    private RegisterScreen CreateRegisterScreen()
    {
        var s = new RegisterScreen();
        s.RegisterSuccess += (_, _) => { UpdateAuthButtons(); NavigateHome(); };
        s.NavigateToLogin += (_, _) => ShowScreen(CreateLoginScreen());
        return s;
    }

    // ── Booking flow ────────────────────────────────────────────────────

    private void OnBookingRequested(object? sender, StorageModel storage)
    {
        if (!ServiceLocator.SessionService.IsAuthenticated())
        {
            MessageBox.Show("Для бронирования необходимо войти.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            ShowScreen(CreateLoginScreen());
            return;
        }
        var bookingScreen = new BookingScreen(storage);
        bookingScreen.BackRequested += (_, _) => NavigateHome();
        bookingScreen.ProceedToPayment += (_, args) =>
            ShowPaymentScreen(args.Form, args.TotalPrice, storage);
        ShowScreen(bookingScreen);
    }

    private void ShowPaymentScreen(BookingCreateForm form, decimal totalPrice, StorageModel storage)
    {
        var payScreen = new PaymentScreen(form, totalPrice);
        payScreen.BackRequested += (_, _) => OnBookingRequested(this, storage);
        payScreen.PaymentSuccess += (_, _) =>
        {
            var result = new PaymentResultScreen(true);
            result.GoHome += (_, _) => NavigateHome();
            result.GoToBookings += (_, _) => ShowScreen(new AccountScreen("Активные"));
            ShowScreen(result);
        };
        payScreen.PaymentFailed += (_, _) =>
        {
            var result = new PaymentResultScreen(false);
            result.GoHome += (_, _) => NavigateHome();
            result.RetryPayment += (_, _) => ShowPaymentScreen(form, totalPrice, storage);
            ShowScreen(result);
        };
        ShowScreen(payScreen);
    }

    // ── Search ──────────────────────────────────────────────────────────

    private async void OnSearchClick(object? sender, EventArgs e)
    {
        var text = _searchField.Text.Trim();
        if (string.IsNullOrEmpty(text)) { NavigateHome(); return; }
        if (_homeScreen is null || !_contentPanel.Controls.Contains(_homeScreen))
        { _homeScreen = new HomeScreen(); _homeScreen.BookingRequested += OnBookingRequested; ShowScreen(_homeScreen); }
        try
        {
            // Бэкенд не умеет «name OR address» одним запросом — реализация
            // SearchStoragesAsync шлёт два запроса и объединяет результаты
            // с дедупом по названию пункта.
            var results = await ServiceLocator.StorageService.SearchStoragesAsync(text, pageNumber: 0, pageSize: 20);
            _homeScreen.ShowResults(results);
        }
        catch { _homeScreen.ShowResults([]); }
    }

    private void OnLogoutClick(object? sender, EventArgs e) { ServiceLocator.AuthService.Logout(); UpdateAuthButtons(); NavigateHome(); }

    private void UpdateAuthButtons()
    {
        var a = ServiceLocator.SessionService.IsAuthenticated();
        _loginHeaderBtn.Visible = !a; _registerHeaderBtn.Visible = !a;
        _accountHeaderBtn.Visible = a; _logoutHeaderBtn.Visible = a;
        LayoutHeaderButtons();
    }

    private void LayoutHeaderButtons()
    {
        int r = _header.Width - 16;
        if (_logoutHeaderBtn.Visible)
        {
            _logoutHeaderBtn.Location = new Point(r - _logoutHeaderBtn.Width, 10); r -= _logoutHeaderBtn.Width + 8;
            _accountHeaderBtn.Location = new Point(r - _accountHeaderBtn.Width, 10);
        }
        else
        {
            _registerHeaderBtn.Location = new Point(r - _registerHeaderBtn.Width, 10); r -= _registerHeaderBtn.Width + 8;
            _loginHeaderBtn.Location = new Point(r - _loginHeaderBtn.Width, 10);
        }
    }

    private void TryLoadLogo()
    {
        foreach (var p in new[] { Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "skladinya.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "skladinya.png") })
        {
            if (!File.Exists(p)) continue;
            _logo.Image = Image.FromFile(p);
            return;
        }
        var lbl = new Label { Text = "SklaDinya", Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = AppTheme.TextLight, AutoSize = true, BackColor = Color.Transparent, Location = new Point(16, 14), Cursor = Cursors.Hand };
        lbl.Click += (_, _) => NavigateHome();
        _header.Controls.Add(lbl);
    }

    private void TrySetIcon()
    {
        var p = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "skladinya.ico");
        if (File.Exists(p)) Icon = new Icon(p);
    }
}
