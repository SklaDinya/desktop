using SklaDinya_desktop_BL_component.Interfaces.Services;

namespace SklaDinya_desktop_UI_component.Helpers;

/// <summary>
/// Простой сервис-локатор для доступа к BL-сервисам из UI-компонентов.
/// Инициализируется один раз при старте приложения.
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

    public static void Initialize(
        IAuthService authService,
        ISessionService sessionService,
        IBookingService bookingService,
        ICellService cellService,
        IOperatorService operatorService,
        IPaymentService paymentService,
        IPriceService priceService,
        IStorageService storageService,
        IUserService userService)
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
    }
}
