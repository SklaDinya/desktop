using System.Xml;

namespace SklaDinya_desktop_BL_DA_IntegrationalTests.Helpers;

/// <summary>
/// Анонимные объекты, которые при camelCase-сериализации совпадают
/// с internal DTO DA-компонента.
///
/// ВАЖНО: Token() генерирует роль в PascalCase ("Client", "StorageOperator", "Admin"),
/// потому что JwtHelper.ParsePayload использует Enum.TryParse — регистрозависимо,
/// имена должны совпадать с именами членов enum UserRole/OperatorRole.
/// </summary>
internal static class FakeDto
{
    private static string Iso(TimeSpan ts) => XmlConvert.ToString(ts);
    private static string Now() => DateTime.UtcNow.ToString("o");

    // ── Tokens ────────────────────────────────────────────────────────────

    /// <summary>
    /// Генерирует JWT с нужной ролью.
    /// role должен совпадать с именем члена enum UserRole:
    /// "Client", "StorageOperator", "Admin".
    /// </summary>
    public static string Token(string userRole = "Client")
    {
        var payloadJson = $"{{\"userId\":\"{Guid.NewGuid()}\",\"userRole\":\"{userRole}\"}}";
        return BuildJwt(payloadJson);
    }

    /// <summary>JWT оператора с дополнительными полями storageId и role.</summary>
    public static string OperatorToken()
    {
        var payloadJson =
            $"{{\"userId\":\"{Guid.NewGuid()}\"," +
            $"\"userRole\":\"StorageOperator\"," +
            $"\"storageId\":\"{Guid.NewGuid()}\"," +
            $"\"role\":\"MainOperator\"}}";
        return BuildJwt(payloadJson);
    }

    private static string BuildJwt(string payloadJson)
    {
        var header = Base64Url("{\"alg\":\"HS256\",\"typ\":\"JWT\"}"u8.ToArray());
        var payload = Base64Url(System.Text.Encoding.UTF8.GetBytes(payloadJson));
        return $"{header}.{payload}.fakesig";
    }

    private static string Base64Url(byte[] bytes)
        => Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');

    // ── Storage ────────────────────────────────────────────────────────────

    public static object Storage(Guid? id = null) => new
    {
        id = id ?? Guid.NewGuid(),
        name = "Test Storage",
        address = "Test Address",
        description = "Test Description",
        status = "active",
        createdAt = Now(),
        updatedAt = Now(),
    };

    public static object[] StorageList(int count = 1)
        => Enumerable.Range(0, count).Select(_ => Storage()).ToArray();

    // ── Cell ───────────────────────────────────────────────────────────────

    public static object Cell(Guid? id = null, Guid? storageId = null) => new
    {
        id = id ?? Guid.NewGuid(),
        storageId = storageId ?? Guid.NewGuid(),
        name = "A1",
        cellClass = "Small",
        createdAt = Now(),
    };

    public static object[] CellList(int count = 1)
        => Enumerable.Range(0, count).Select(_ => Cell()).ToArray();

    // ── Price ──────────────────────────────────────────────────────────────

    public static object Price(Guid? storageId = null) => new
    {
        storageId = storageId ?? Guid.NewGuid(),
        cellClass = "Small",
        price = "99.99",
        createdAt = Now(),
    };

    public static object[] PriceList(int count = 1)
        => Enumerable.Range(0, count).Select(_ => Price()).ToArray();

    // ── User ───────────────────────────────────────────────────────────────

    public static object User(Guid? id = null) => new
    {
        id = id ?? Guid.NewGuid(),
        username = "testuser",
        name = "Test User",
        email = "test@example.com",
        role = "client",      // camelCase — это DA-enum, не UserRole
        banned = false,
        createdAt = Now(),
        updatedAt = Now(),
    };

    public static object[] UserList(int count = 1)
        => Enumerable.Range(0, count).Select(_ => User()).ToArray();

    public static object Me(Guid? id = null) => new
    {
        id = id ?? Guid.NewGuid(),
        username = "me",
        name = "Me User",
        email = "me@example.com",
        role = "client",       
    };

    // ── Operator ───────────────────────────────────────────────────────────

    public static object Operator(Guid? id = null) => new
    {
        id = id ?? Guid.NewGuid(),
        username = "operator1",
        name = "Operator One",
        email = "op@example.com",
        role = "ordinaryOperator",   
        banned = false,
        createdAt = Now(),
        updatedAt = Now(),
    };

    public static object[] OperatorList(int count = 1)
        => Enumerable.Range(0, count).Select(_ => Operator()).ToArray();

    // ── Booking ────────────────────────────────────────────────────────────

    public static object BookingForUser(Guid? id = null) => new
    {
        id = id ?? Guid.NewGuid(),
        userId = Guid.NewGuid(),
        storageId = Guid.NewGuid(),
        storage = Storage(),
        cells = CellList(1),
        startTime = Now(),
        bookingTime = Iso(TimeSpan.FromHours(2)),
        createdAt = Now(),
        status = "paid",              
    };

    public static object[] BookingForUserList(int count = 1)
        => Enumerable.Range(0, count).Select(_ => BookingForUser()).ToArray();

    public static object BookingForOperator(Guid? id = null) => new
    {
        id = id ?? Guid.NewGuid(),
        userId = Guid.NewGuid(),
        user = new { id = Guid.NewGuid(), name = "Client Name", email = "c@e.com" },
        storageId = Guid.NewGuid(),
        cells = CellList(1),
        startTime = Now(),
        bookingTime = Iso(TimeSpan.FromHours(1)),
        createdAt = Now(),
        status = "created",           
    };

    public static object[] BookingForOperatorList(int count = 1)
        => Enumerable.Range(0, count).Select(_ => BookingForOperator()).ToArray();

    public static object BookingReceipt(Guid? bookingId = null) => new
    {
        booking = BookingForUser(bookingId),
        receipt = "receipt.jwt.token",
    };
}