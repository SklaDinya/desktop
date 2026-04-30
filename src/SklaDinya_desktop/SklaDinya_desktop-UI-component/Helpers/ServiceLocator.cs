using SklaDinya_desktop_BL_component.Interfaces.Logging;
using SklaDinya_desktop_BL_component.Interfaces.Services;

namespace SklaDinya_desktop_UI_component.Helpers;

/// <summary>
/// Простой сервис-локатор для доступа к BL-сервисам и инфраструктуре из UI-компонентов.
/// Инициализируется один раз при старте приложения. UI зависит только от BL-интерфейсов
/// — конкретные реализации (репозитории, логгер) подставляет Main-проект.
/// </summary>
public static class ServiceLocator
{
    public static IAuthService AuthService { get; private set; } = null!;
    public static ISessionService SessionService { get; private set; } = null!;
    public static IBookingService BookingService { get; private set; } = null!;
    public static ICellService CellService { get; private set; } = null!;
    public static IOperatorService OperatorService { get; private set; } = null!;
    public static IPaymentService PaymentService { get; private set; } = null!;
    public static IPriceService PriceService { get; private set; } = null!;
    public static IStorageService StorageService { get; private set; } = null!;
    public static IUserService UserService { get; private set; } = null!;

    /// <summary>
    /// Логгер приложения. Реализация передаётся из Main (обычно — FileLoggerAdapter из DA).
    /// До инициализации использует «пустую» реализацию, чтобы UI не падал, если ошибка
    /// возникла раньше старта.
    /// </summary>
    public static IAppLogger Logger { get; private set; } = new NullLogger();

    public static void Initialize(
        IAuthService authService,
        ISessionService sessionService,
        IBookingService bookingService,
        ICellService cellService,
        IOperatorService operatorService,
        IPaymentService paymentService,
        IPriceService priceService,
        IStorageService storageService,
        IUserService userService,
        IAppLogger logger)
    {
        AuthService = authService;
        SessionService = sessionService;
        BookingService = bookingService;
        CellService = cellService;
        OperatorService = operatorService;
        PaymentService = paymentService;
        PriceService = priceService;
        StorageService = storageService;
        UserService = userService;
        Logger = logger;
    }

    /// <summary>
    /// Заглушка-логгер на случай, если кто-то попытается логировать
    /// до вызова <see cref="Initialize"/>. Тихо игнорирует все вызовы.
    /// </summary>
    private sealed class NullLogger : IAppLogger
    {
        public void Info(string message) { }
        public void Error(
            string message,
            Exception? exception = null,
            IReadOnlyDictionary<string, string?>? context = null) { }
    }
}
