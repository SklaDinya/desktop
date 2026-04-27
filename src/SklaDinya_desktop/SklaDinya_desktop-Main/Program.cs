using Microsoft.Extensions.Configuration;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Interfaces.Services;
using SklaDinya_desktop_BL_component.Services;
using SklaDinya_desktop_DA_component.Http;
using SklaDinya_desktop_DA_component.Repositories;
using SklaDinya_desktop_UI_component;
using SklaDinya_desktop_UI_component.Helpers;

namespace SklaDinya_desktop_Main;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // ── Конфигурация из appsettings.json ────────────────────────────
        var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .Build();

        var baseUrl = config["Api:BaseUrl"] ?? "http://localhost";
        var port = int.TryParse(config["Api:Port"], out var p) ? p : 8080;
        var apiAddress = $"{baseUrl.TrimEnd('/')}:{port}";

        // ── HTTP-клиент и ApiClient ─────────────────────────────────────
        var httpClient = new HttpClient { BaseAddress = new Uri(apiAddress) };
        var apiClient = new ApiClient(httpClient);

        // ── Репозитории (DA) ────────────────────────────────────────────
        IAuthRepository authRepo = new AuthRepository(apiClient);
        IBookingRepository bookingRepo = new BookingRepository(apiClient);
        ICellRepository cellRepo = new CellRepository(apiClient);
        IOperatorRepository operatorRepo = new OperatorRepository(apiClient);
        IPaymentRepository paymentRepo = new PaymentRepository(apiClient);
        IPriceRepository priceRepo = new PriceRepository(apiClient);
        IStorageRepository storageRepo = new StorageRepository(apiClient);
        IUserRepository userRepo = new UserRepository(apiClient);

        // ── Сервисы (BL) ────────────────────────────────────────────────
        ISessionService session = new SessionService();
        IAuthService authService = new AuthService(authRepo, session);
        IBookingService bookingService = new BookingService(bookingRepo, session);
        ICellService cellService = new CellService(cellRepo, session);
        IOperatorService operatorService = new OperatorService(operatorRepo, session);
        IPaymentService paymentService = new PaymentService(paymentRepo, bookingService, session);
        IPriceService priceService = new PriceService(priceRepo, session);
        IStorageService storageService = new StorageService(storageRepo, session);
        IUserService userService = new UserService(userRepo, session);

        // ── Инициализация ServiceLocator для UI ─────────────────────────
        ServiceLocator.Initialize(
            authService, session,
            bookingService, cellService, operatorService,
            paymentService, priceService, storageService, userService);

        // ── Запуск главного окна ────────────────────────────────────────
        Application.Run(new MainForm());
    }
}
