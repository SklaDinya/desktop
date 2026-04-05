using System.Xml;

namespace SklaDinya_desktop_DA_tests.Helpers;

/// <summary>
/// Строители анонимных объектов, которые при сериализации в camelCase JSON
/// совпадают с internal DTO DA-компонента.
/// Используется вместо прямого создания internal DTO-типов, недоступных извне.
///
/// Ключевые правила:
/// - bookingTime сериализуется как ISO 8601 duration (PT2H, P1D и т.п.) — именно
///   такой формат разбирает TimeSpanIso8601Converter на стороне ApiClient.
/// - enum-значения передаются как camelCase-строки ("paid", "ordinaryOperator"),
///   что соответствует JsonStringEnumConverter(CamelCase) в ApiClient.
/// - price передаётся как строка ("99.99") — decimal128 формат бэкенда.
/// </summary>
internal static class FakeDto
{
    private static string Iso(TimeSpan ts) => XmlConvert.ToString(ts);
    private static string Now()            => DateTime.UtcNow.ToString("o");

    // ── Storage ────────────────────────────────────────────────────────────

    public static object Storage(Guid? id = null) => new
    {
        id          = id ?? Guid.NewGuid(),
        name        = "Test Storage",
        address     = "Test Address",
        description = "Test Description",
        status      = "active",
        createdAt   = Now(),
        updatedAt   = Now(),
    };

    public static object[] StorageList(int count = 1)
        => Enumerable.Range(0, count).Select(_ => Storage()).ToArray();

    // ── Cell ───────────────────────────────────────────────────────────────

    public static object Cell(Guid? id = null, Guid? storageId = null) => new
    {
        id        = id        ?? Guid.NewGuid(),
        storageId = storageId ?? Guid.NewGuid(),
        name      = "A1",
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
        price     = "99.99",   // строка — decimal128 формат
        createdAt = Now(),
    };

    public static object[] PriceList(int count = 1)
        => Enumerable.Range(0, count).Select(_ => Price()).ToArray();

    // ── User ───────────────────────────────────────────────────────────────

    public static object User(Guid? id = null) => new
    {
        id        = id ?? Guid.NewGuid(),
        username  = "testuser",
        name      = "Test User",
        email     = "test@example.com",
        role      = "client",
        banned    = false,
        createdAt = Now(),
        updatedAt = Now(),
    };

    public static object[] UserList(int count = 1)
        => Enumerable.Range(0, count).Select(_ => User()).ToArray();

    public static object Me(Guid? id = null) => new
    {
        id       = id ?? Guid.NewGuid(),
        username = "me",
        name     = "Me User",
        email    = "me@example.com",
        role     = "client",
    };

    // ── Operator ───────────────────────────────────────────────────────────

    public static object Operator(Guid? id = null) => new
    {
        id        = id ?? Guid.NewGuid(),
        username  = "operator1",
        name      = "Operator One",
        email     = "op@example.com",
        role      = "ordinaryOperator",
        banned    = false,
        createdAt = Now(),
        updatedAt = Now(),
    };

    public static object[] OperatorList(int count = 1)
        => Enumerable.Range(0, count).Select(_ => Operator()).ToArray();

    // ── Booking ────────────────────────────────────────────────────────────

    public static object BookingForUser(Guid? id = null) => new
    {
        id          = id ?? Guid.NewGuid(),
        userId      = Guid.NewGuid(),
        storageId   = Guid.NewGuid(),
        storage     = Storage(),
        cells       = CellList(1),
        startTime   = Now(),
        bookingTime = Iso(TimeSpan.FromHours(2)),   // ISO 8601 duration
        createdAt   = Now(),
        status      = "paid",
    };

    public static object[] BookingForUserList(int count = 1)
        => Enumerable.Range(0, count).Select(_ => BookingForUser()).ToArray();

    public static object BookingForOperator(Guid? id = null) => new
    {
        id          = id ?? Guid.NewGuid(),
        userId      = Guid.NewGuid(),
        user        = new { id = Guid.NewGuid(), name = "Client Name", email = "c@e.com" },
        storageId   = Guid.NewGuid(),
        cells       = CellList(1),
        startTime   = Now(),
        bookingTime = Iso(TimeSpan.FromHours(1)),
        createdAt   = Now(),
        status      = "created",
    };

    public static object[] BookingForOperatorList(int count = 1)
        => Enumerable.Range(0, count).Select(_ => BookingForOperator()).ToArray();

    public static object BookingReceipt(Guid? bookingId = null) => new
    {
        booking = BookingForUser(bookingId),
        receipt = "receipt.jwt.token",
    };
}
