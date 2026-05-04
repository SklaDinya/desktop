using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Models;

namespace SklaDinya_desktop_BackendMock;

/// <summary>
/// Предзаданные тестовые данные для заглушки бэкенда.
/// </summary>
public static class MockData
{
    // ── Идентификаторы ──────────────────────────────────────────────────
    public static readonly Guid UserId1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid UserId2 = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public static readonly Guid AdminId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    public static readonly Guid OperatorId1 = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
    public static readonly Guid OperatorId2 = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");

    public static readonly Guid StorageId1 = Guid.Parse("10000000-0000-0000-0000-000000000001");
    public static readonly Guid StorageId2 = Guid.Parse("10000000-0000-0000-0000-000000000002");
    public static readonly Guid StorageId3 = Guid.Parse("10000000-0000-0000-0000-000000000003");

    public static readonly Guid CellId1 = Guid.Parse("c0000000-0000-0000-0000-000000000001");
    public static readonly Guid CellId2 = Guid.Parse("c0000000-0000-0000-0000-000000000002");
    public static readonly Guid CellId3 = Guid.Parse("c0000000-0000-0000-0000-000000000003");
    public static readonly Guid CellId4 = Guid.Parse("c0000000-0000-0000-0000-000000000004");
    public static readonly Guid CellId5 = Guid.Parse("c0000000-0000-0000-0000-000000000005");

    public static readonly Guid BookingId1 = Guid.Parse("b0000000-0000-0000-0000-000000000001");
    public static readonly Guid BookingId2 = Guid.Parse("b0000000-0000-0000-0000-000000000002");
    public static readonly Guid BookingId3 = Guid.Parse("b0000000-0000-0000-0000-000000000003");

    // ── Пункты хранения ─────────────────────────────────────────────────
    public static List<StorageModel> Storages =>
    [
        new()
        {
            Id = StorageId1, Name = "SklaDinya Центр", Address = "ул. Крещатик, 22",
            Description = "Центральный пункт хранения в самом сердце города. Круглосуточный доступ.",
            Status = StorageStatus.Active, CreatedAt = DateTime.Now.AddDays(-30), UpdatedAt = DateTime.Now.AddDays(-1),
        },
        new()
        {
            Id = StorageId2, Name = "SklaDinya Вокзал", Address = "пл. Вокзальная, 1",
            Description = "Удобное расположение рядом с ж/д вокзалом. Ячейки разных размеров.",
            Status = StorageStatus.Active, CreatedAt = DateTime.Now.AddDays(-20), UpdatedAt = DateTime.Now.AddDays(-5),
        },
        new()
        {
            Id = StorageId3, Name = "SklaDinya Аэропорт", Address = "Аэропорт, терминал B",
            Description = "Заявка на рассмотрении администратором.",
            Status = StorageStatus.Created, CreatedAt = DateTime.Now.AddDays(-2), UpdatedAt = DateTime.Now.AddDays(-2),
        },
    ];

    // ── Ячейки ──────────────────────────────────────────────────────────
    public static List<CellModel> Cells =>
    [
        new() { Id = CellId1, StorageId = StorageId1, Name = "A-01", CellClass = "Маленькая", CreatedAt = DateTime.Now.AddDays(-28) },
        new() { Id = CellId2, StorageId = StorageId1, Name = "A-02", CellClass = "Маленькая", CreatedAt = DateTime.Now.AddDays(-28) },
        new() { Id = CellId3, StorageId = StorageId1, Name = "B-01", CellClass = "Средняя", CreatedAt = DateTime.Now.AddDays(-28) },
        new() { Id = CellId4, StorageId = StorageId2, Name = "V-01", CellClass = "Большая", CreatedAt = DateTime.Now.AddDays(-18) },
        new() { Id = CellId5, StorageId = StorageId2, Name = "V-02", CellClass = "Маленькая", CreatedAt = DateTime.Now.AddDays(-18) },
    ];

    // ── Тарифы ──────────────────────────────────────────────────────────
    public static List<PriceModel> Prices =>
    [
        new() { StorageId = StorageId1, CellClass = "Маленькая", Price = 25m, CreatedAt = DateTime.Now.AddDays(-28) },
        new() { StorageId = StorageId1, CellClass = "Средняя", Price = 45m, CreatedAt = DateTime.Now.AddDays(-28) },
        new() { StorageId = StorageId2, CellClass = "Большая", Price = 80m, CreatedAt = DateTime.Now.AddDays(-18) },
        new() { StorageId = StorageId2, CellClass = "Маленькая", Price = 20m, CreatedAt = DateTime.Now.AddDays(-18) },
    ];

    // ── Бронирования ────────────────────────────────────────────────────
    public static List<BookingModel> Bookings =>
    [
        new()
        {
            Id = BookingId1, UserId = UserId1, StorageId = StorageId1,
            Storage = Storages[0],
            Cells = [Cells[0], Cells[2]],
            StartTime = DateTime.Now.AddHours(2), BookingTime = TimeSpan.FromHours(3),
            CreatedAt = DateTime.Now.AddMinutes(-30), Status = BookingStatus.Paid,
        },
        new()
        {
            Id = BookingId2, UserId = UserId1, StorageId = StorageId2,
            Storage = Storages[1],
            Cells = [Cells[3]],
            StartTime = DateTime.Now.AddDays(-3), BookingTime = TimeSpan.FromHours(5),
            CreatedAt = DateTime.Now.AddDays(-4), Status = BookingStatus.Finished,
        },
        new()
        {
            Id = BookingId3, UserId = UserId1, StorageId = StorageId1,
            Storage = Storages[0],
            Cells = [Cells[1]],
            StartTime = DateTime.Now.AddHours(24), BookingTime = TimeSpan.FromHours(2),
            CreatedAt = DateTime.Now.AddMinutes(-5), Status = BookingStatus.Created,
        },
    ];

    // ── Бронирования для оператора ──────────────────────────────────────
    public static List<BookingOperatorModel> OperatorBookings =>
    [
        new()
        {
            Id = BookingId1, UserId = UserId1,
            User = new BookingUserModel { Id = UserId1, Name = "Иван Петров", Email = "ivan@test.com" },
            StorageId = StorageId1,
            Cells = [Cells[0], Cells[2]],
            StartTime = DateTime.Now.AddHours(2), BookingTime = TimeSpan.FromHours(3),
            CreatedAt = DateTime.Now.AddMinutes(-30), Status = BookingStatus.Paid,
        },
        new()
        {
            Id = BookingId3, UserId = UserId2,
            User = new BookingUserModel { Id = UserId2, Name = "Мария Сидорова", Email = "maria@test.com" },
            StorageId = StorageId1,
            Cells = [Cells[1]],
            StartTime = DateTime.Now.AddHours(24), BookingTime = TimeSpan.FromHours(2),
            CreatedAt = DateTime.Now.AddMinutes(-5), Status = BookingStatus.Created,
        },
    ];

    // ── Пользователи ────────────────────────────────────────────────────
    public static List<UserModel> Users =>
    [
        new()
        {
            Id = UserId1, Username = "ivan", Name = "Иван Петров", Email = "ivan@test.com",
            Role = UserRole.Client, Banned = false, CreatedAt = DateTime.Now.AddDays(-60), UpdatedAt = DateTime.Now.AddDays(-1),
        },
        new()
        {
            Id = UserId2, Username = "maria", Name = "Мария Сидорова", Email = "maria@test.com",
            Role = UserRole.Client, Banned = false, CreatedAt = DateTime.Now.AddDays(-45), UpdatedAt = DateTime.Now.AddDays(-10),
        },
        new()
        {
            Id = AdminId, Username = "admin", Name = "Администратор", Email = "admin@skladinya.com",
            Role = UserRole.Admin, Banned = false, CreatedAt = DateTime.Now.AddDays(-90), UpdatedAt = DateTime.Now.AddDays(-1),
        },
        new()
        {
            Id = OperatorId1, Username = "operator1", Name = "Олег Операторов", Email = "oleg@skladinya.com",
            Role = UserRole.StorageOperator, Banned = false, CreatedAt = DateTime.Now.AddDays(-30), UpdatedAt = DateTime.Now.AddDays(-1),
        },
    ];

    // ── Операторы ───────────────────────────────────────────────────────
    public static List<OperatorModel> Operators =>
    [
        new()
        {
            Id = OperatorId1, Username = "operator1", Name = "Олег Операторов", Email = "oleg@skladinya.com",
            Role = OperatorRole.MainOperator, Banned = false, CreatedAt = DateTime.Now.AddDays(-30), UpdatedAt = DateTime.Now.AddDays(-1),
        },
        new()
        {
            Id = OperatorId2, Username = "operator2", Name = "Анна Помощникова", Email = "anna@skladinya.com",
            Role = OperatorRole.OrdinaryOperator, Banned = false, CreatedAt = DateTime.Now.AddDays(-15), UpdatedAt = DateTime.Now.AddDays(-3),
        },
    ];

    // ── MeModel ─────────────────────────────────────────────────────────
    public static MeModel Me(UserRole role) => role switch
    {
        UserRole.Admin => new MeModel { Id = AdminId, Username = "admin", Name = "Администратор", Email = "admin@skladinya.com", Role = UserRole.Admin },
        UserRole.StorageOperator => new MeModel { Id = OperatorId1, Username = "operator1", Name = "Олег Операторов", Email = "oleg@skladinya.com", Role = UserRole.StorageOperator },
        _ => new MeModel { Id = UserId1, Username = "ivan", Name = "Иван Петров", Email = "ivan@test.com", Role = UserRole.Client },
    };

    // ── Фейковый JWT ────────────────────────────────────────────────────
    // Формат: header.payload.signature — payload содержит нужные поля.
    // JwtHelper парсит payload без верификации подписи.

    public static string MakeJwt(UserRole role)
    {
        var header = ToBase64Url("{\"alg\":\"none\",\"typ\":\"JWT\"}");

        var payload = role switch
        {
            UserRole.Admin =>
                $"{{\"userId\":\"{AdminId}\",\"userRole\":\"Admin\"}}",
            UserRole.StorageOperator =>
                $"{{\"userId\":\"{OperatorId1}\",\"userRole\":\"StorageOperator\",\"storageId\":\"{StorageId1}\",\"operatorRole\":\"MainOperator\"}}",
            _ =>
                $"{{\"userId\":\"{UserId1}\",\"userRole\":\"Client\"}}",
        };

        return $"{header}.{ToBase64Url(payload)}.fakesignature";
    }

    private static string ToBase64Url(string input)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(input);
        return Convert.ToBase64String(bytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }
}
