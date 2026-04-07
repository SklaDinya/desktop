using SklaDinya_desktop_BL_component.Services;
using SklaDinya_desktop_DA_component.Http;
using SklaDinya_desktop_DA_component.Repositories;

namespace SklaDinya_desktop_BL_DA_IntegrationalTests.Helpers;

/// <summary>
/// Фабрика для сборки полной цепочки BL-сервис → DA-репозиторий → ApiClient(mock).
/// Каждый Create-метод возвращает реальный BL-сервис, подключённый к реальному
/// DA-репозиторию, который работает с замоканным HttpMessageHandler.
/// Сессия разделяется между всеми сервисами в рамках одного теста.
/// </summary>
internal static class ServiceFactory
{
    /// <summary>
    /// Создаёт SessionService и устанавливает переданный токен.
    /// Используется как shared-зависимость между сервисами.
    /// </summary>
    public static SessionService Session(string? token = null)
    {
        var session = new SessionService();
        if (token is not null)
            session.SetToken(token);
        return session;
    }

    public static AuthService Auth(ApiClient client, SessionService session)
        => new(new AuthRepository(client), session);

    public static BookingService Booking(ApiClient client, SessionService session)
        => new(new BookingRepository(client), session);

    public static CellService Cell(ApiClient client, SessionService session)
        => new(new CellRepository(client), session);

    public static OperatorService Operator(ApiClient client, SessionService session)
        => new(new OperatorRepository(client), session);

    public static PriceService Price(ApiClient client, SessionService session)
        => new(new PriceRepository(client), session);

    public static StorageService Storage(ApiClient client, SessionService session)
        => new(new StorageRepository(client), session);

    public static UserService User(ApiClient client, SessionService session)
        => new(new UserRepository(client), session);

    /// <summary>
    /// PaymentService требует IBookingService для получения LastReceipt.
    /// Передаётся уже созданный BookingService из той же цепочки.
    /// </summary>
    public static PaymentService Payment(ApiClient client, BookingService bookingService, SessionService session)
        => new(new PaymentRepository(client), bookingService, session);
}
