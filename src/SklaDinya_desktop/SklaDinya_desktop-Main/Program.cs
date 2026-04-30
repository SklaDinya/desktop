using Microsoft.Extensions.Configuration;
using SklaDinya_desktop_BL_component.Interfaces.Logging;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Interfaces.Services;
using SklaDinya_desktop_BL_component.Services;
using SklaDinya_desktop_BackendMock;
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

        var useMock = bool.TryParse(config["Api:UseMock"], out var m) && m;

        IAuthRepository authRepo;
        IBookingRepository bookingRepo;
        ICellRepository cellRepo;
        IOperatorRepository operatorRepo;
        IPaymentRepository paymentRepo;
        IPriceRepository priceRepo;
        IStorageRepository storageRepo;
        IUserRepository userRepo;

        if (useMock)
        {
            // ── Заглушка (без сервера) ──────────────────────────────────
            var mockBookingRepo = new MockBookingRepository();
            authRepo = new MockAuthRepository();
            bookingRepo = mockBookingRepo;
            cellRepo = new MockCellRepository();
            operatorRepo = new MockOperatorRepository();
            paymentRepo = new MockPaymentRepository(mockBookingRepo);
            priceRepo = new MockPriceRepository();
            storageRepo = new MockStorageRepository();
            userRepo = new MockUserRepository();
        }
        else
        {
            // ── Реальный API-сервер ─────────────────────────────────────
            var baseUrl = config["Api:BaseUrl"] ?? "http://localhost";
            var port = int.TryParse(config["Api:Port"], out var p) ? p : 8080;
            var apiAddress = $"{baseUrl.TrimEnd('/')}:{port}";

            var httpClient = new HttpClient { BaseAddress = new Uri(apiAddress) };
            var apiClient = new ApiClient(httpClient);

            authRepo = new AuthRepository(apiClient);
            bookingRepo = new BookingRepository(apiClient);
            cellRepo = new CellRepository(apiClient);
            operatorRepo = new OperatorRepository(apiClient);
            paymentRepo = new PaymentRepository(apiClient);
            priceRepo = new PriceRepository(apiClient);
            storageRepo = new StorageRepository(apiClient);
            userRepo = new UserRepository(apiClient);
        }

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

        // ── Инфраструктура: логгер (реализация в DA, контракт в BL) ─────
        IAppLogger logger = new FileLoggerAdapter();

        // ── Инициализация ServiceLocator для UI ─────────────────────────
        ServiceLocator.Initialize(
            authService, session,
            bookingService, cellService, operatorService,
            paymentService, priceService, storageService, userService,
            logger);

        // ── Запуск главного окна ────────────────────────────────────────
        Application.Run(new MainForm());
    }
}
