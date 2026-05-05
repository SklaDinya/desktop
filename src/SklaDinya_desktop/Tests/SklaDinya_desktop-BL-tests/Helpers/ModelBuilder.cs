using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Models;

namespace SklaDinya_desktop_BL_tests.Helpers;

/// <summary>
/// Строители тестовых BL-моделей.
/// Используются для настройки возвращаемых значений моков репозиториев.
/// </summary>
internal static class ModelBuilder
{
    public static BookingModel Booking(Guid? id = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        UserId = Guid.NewGuid(),
        StorageId = Guid.NewGuid(),
        Storage = Storage(),
        Cells = [Cell()],
        StartTime = DateTime.UtcNow,
        BookingTime = TimeSpan.FromHours(2),
        CreatedAt = DateTime.UtcNow,
        Status = BookingStatus.Paid,
    };

    public static BookingOperatorModel BookingOperator(Guid? id = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        UserId = Guid.NewGuid(),
        User = new BookingUserModel { Id = Guid.NewGuid(), Name = "Client", Email = "c@e.com" },
        StorageId = Guid.NewGuid(),
        Cells = [Cell()],
        StartTime = DateTime.UtcNow,
        BookingTime = TimeSpan.FromHours(1),
        CreatedAt = DateTime.UtcNow,
        Status = BookingStatus.Created,
    };

    public static BookingReceiptModel BookingReceipt(Guid? bookingId = null) => new()
    {
        Booking = Booking(bookingId),
        Receipt = "receipt.jwt.token",
    };

    public static CellModel Cell(Guid? id = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        StorageId = Guid.NewGuid(),
        Name = "A1",
        CellClass = "Small",
        CreatedAt = DateTime.UtcNow,
    };

    public static StorageModel Storage(Guid? id = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        Name = "Test Storage",
        Address = "Test Address",
        Description = "Test Description",
        Status = StorageStatus.Active,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
    };

    public static PriceModel Price(Guid? storageId = null) => new()
    {
        StorageId = storageId ?? Guid.NewGuid(),
        CellClass = "Small",
        Price = 99.99m,
        CreatedAt = DateTime.UtcNow,
    };

    public static UserModel User(Guid? id = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        Username = "testuser",
        Name = "Test User",
        Email = "test@example.com",
        Role = UserRole.Client,
        Banned = false,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
    };

    public static MeModel Me(Guid? id = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        Username = "me",
        Name = "Me User",
        Email = "me@example.com",
        Role = UserRole.Client,
    };

    public static OperatorModel Operator(Guid? id = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        Username = "operator1",
        Name = "Operator One",
        Email = "op@example.com",
        Role = OperatorRole.OrdinaryOperator,
        Banned = false,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
    };
}
