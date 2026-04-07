using SklaDinya_desktop_DA_component.Dtos;
using SklaDinya_desktop_DA_component.Enums;
using System.Xml;

namespace SklaDinya_desktop_DA_tests.Helpers;

/// <summary>
/// Строитель тестовых DTO объектов DA-компонента.
/// Использует реальные публичные типы вместо анонимных объектов.
///
/// Ключевые правила:
/// - bookingTime сериализуется как ISO 8601 duration (PT2H, P1D и т.п.) — именно
///   такой формат разбирает TimeSpanIso8601Converter на стороне ApiClient.
/// - enum-значения в DA слое (UserRoleDto, OperatorRoleDto и т.д.) сериализуются
///   через JsonStringEnumConverter(CamelCase) в ApiClient: Client → "client".
/// - price передаётся как строка ("99.99") — decimal128 формат бэкенда.
/// </summary>
internal static class FakeDto
{
    private static string Iso(TimeSpan ts) => XmlConvert.ToString(ts);

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
