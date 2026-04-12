using SklaDinya_desktop_DA_component.Dtos;
using SklaDinya_desktop_DA_component.Enums;

namespace SklaDinya_desktop_BL_DA_IntegrationalTests.Helpers;

/// <summary>
/// Строитель тестовых DTO объектов DA-компонента для интеграционных тестов.
/// Использует реальные публичные типы вместо анонимных объектов.
///
/// Token() генерирует userRole в PascalCase ("Client", "StorageOperator", "Admin") —
/// JwtHelper.ParsePayload использует Enum.TryParse без ignoreCase.
/// Поле оператора в JWT называется "OperatorRole" — совпадает с TryGetProperty("OperatorRole") в JwtHelper.
/// Кодирование — обычный Base64 без URL-замен, как в JwtHelper.DecodeBase64Url.
/// </summary>
internal static class FakeDto
{
    // ── Tokens ────────────────────────────────────────────────────────────

    /// <summary>
    /// Генерирует JWT с нужной ролью.
    /// userRole должен совпадать с именем члена enum UserRole:
    /// "Client", "StorageOperator", "Admin".
    /// </summary>
    public static string Token(string userRole = "Client")
    {
        var payloadJson = $"{{\"userId\":\"{Guid.NewGuid()}\",\"userRole\":\"{userRole}\"}}";
        return BuildJwt(payloadJson);
    }

    /// <summary>JWT оператора с полями storageId и OperatorRole.</summary>
    public static string OperatorToken()
    {
        var payloadJson =
            $"{{\"userId\":\"{Guid.NewGuid()}\"," +
            $"\"userRole\":\"StorageOperator\"," +
            $"\"storageId\":\"{Guid.NewGuid()}\"," +
            $"\"OperatorRole\":\"MainOperator\"}}";
        return BuildJwt(payloadJson);
    }

    private static string BuildJwt(string payloadJson)
    {
        var header = Base64Encode("{\"alg\":\"HS256\",\"typ\":\"JWT\"}"u8.ToArray());
        var payload = Base64Encode(System.Text.Encoding.UTF8.GetBytes(payloadJson));
        return $"{header}.{payload}.fakesig";
    }

    private static string Base64Encode(byte[] bytes)
        => Convert.ToBase64String(bytes);

    // ── Storage ────────────────────────────────────────────────────────────

    public static StorageDto Storage(Guid? id = null) => new(
        Id: id ?? Guid.NewGuid(),
        Name: "Test Storage",
        Address: "Test Address",
        Description: "Test Description",
        Status: StorageStatusDto.Active,
        CreatedAt: DateTime.UtcNow,
        UpdatedAt: DateTime.UtcNow);

    public static List<StorageDto> StorageList(int count = 1)
        => Enumerable.Range(0, count).Select(_ => Storage()).ToList();

    // ── Cell ───────────────────────────────────────────────────────────────

    public static CellDto Cell(Guid? id = null, Guid? storageId = null) => new(
        Id: id ?? Guid.NewGuid(),
        StorageId: storageId ?? Guid.NewGuid(),
        Name: "A1",
        CellClass: "Small",
        CreatedAt: DateTime.UtcNow);

    public static List<CellDto> CellList(int count = 1)
        => Enumerable.Range(0, count).Select(_ => Cell()).ToList();

    // ── Price ──────────────────────────────────────────────────────────────

    public static PriceDto Price(Guid? storageId = null) => new(
        StorageId: storageId ?? Guid.NewGuid(),
        CellClass: "Small",
        Price: "99.99",
        CreatedAt: DateTime.UtcNow);

    public static List<PriceDto> PriceList(int count = 1)
        => Enumerable.Range(0, count).Select(_ => Price()).ToList();

    // ── User ───────────────────────────────────────────────────────────────

    public static UserDto User(Guid? id = null) => new(
        Id: id ?? Guid.NewGuid(),
        Username: "testuser",
        Name: "Test User",
        Email: "test@example.com",
        Role: UserRoleDto.Client,
        Banned: false,
        CreatedAt: DateTime.UtcNow,
        UpdatedAt: DateTime.UtcNow);

    public static List<UserDto> UserList(int count = 1)
        => Enumerable.Range(0, count).Select(_ => User()).ToList();

    public static MeDto Me(Guid? id = null) => new(
        Id: id ?? Guid.NewGuid(),
        Username: "me",
        Name: "Me User",
        Email: "me@example.com",
        Role: UserRoleDto.Client);

    // ── Operator ───────────────────────────────────────────────────────────

    public static OperatorDto Operator(Guid? id = null) => new(
        Id: id ?? Guid.NewGuid(),
        Username: "operator1",
        Name: "Operator One",
        Email: "op@example.com",
        Role: OperatorRoleDto.OrdinaryOperator,
        Banned: false,
        CreatedAt: DateTime.UtcNow,
        UpdatedAt: DateTime.UtcNow);

    public static List<OperatorDto> OperatorList(int count = 1)
        => Enumerable.Range(0, count).Select(_ => Operator()).ToList();

    // ── Booking ────────────────────────────────────────────────────────────

    public static BookingUserDto BookingForUser(Guid? id = null)
    {
        var storageId = Guid.NewGuid();
        return new(
            Id: id ?? Guid.NewGuid(),
            UserId: Guid.NewGuid(),
            StorageId: storageId,
            Storage: Storage(storageId),
            Cells: CellList(1),
            StartTime: DateTime.UtcNow,
            BookingTime: TimeSpan.FromHours(2),
            CreatedAt: DateTime.UtcNow,
            Status: BookingStatusDto.Paid);
    }

    public static List<BookingUserDto> BookingForUserList(int count = 1)
        => Enumerable.Range(0, count).Select(_ => BookingForUser()).ToList();

    public static BookingOperatorDto BookingForOperator(Guid? id = null) => new(
        Id: id ?? Guid.NewGuid(),
        UserId: Guid.NewGuid(),
        User: new BookingUserInfoDto(Guid.NewGuid(), "Client Name", "c@e.com"),
        StorageId: Guid.NewGuid(),
        Cells: CellList(1),
        StartTime: DateTime.UtcNow,
        BookingTime: TimeSpan.FromHours(1),
        CreatedAt: DateTime.UtcNow,
        Status: BookingStatusDto.Created);

    public static List<BookingOperatorDto> BookingForOperatorList(int count = 1)
        => Enumerable.Range(0, count).Select(_ => BookingForOperator()).ToList();

    public static BookingReceiptDto BookingReceipt(Guid? bookingId = null) => new(
        Booking: BookingForUser(bookingId),
        Receipt: "receipt.jwt.token");
}
