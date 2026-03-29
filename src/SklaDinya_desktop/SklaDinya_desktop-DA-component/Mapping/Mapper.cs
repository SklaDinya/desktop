using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Exceptions;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_DA_component.Dtos;

namespace SklaDinya_desktop_DA_component.Mapping;

/// <summary>
/// Маппер DTO → Model.
/// Каждый метод проверяет обязательные поля и выбрасывает <see cref="ServerException"/>,
/// если данные от API невалидны (null, пустые строки, нераспознанные enum и т.п.).
/// </summary>
internal static class Mapper
{
    // ════════════════════════════════════════════════════════════════════════
    // Storage
    // ════════════════════════════════════════════════════════════════════════

    public static StorageModel ToStorage(StorageDto? dto)
    {
        EnsureNotNull(dto, "StorageDto");
        return new StorageModel
        {
            Id          = ParseGuid(dto!.Id,        "storage.id"),
            Name        = RequireString(dto.Name,   "storage.name"),
            Address     = RequireString(dto.Address,"storage.address"),
            Description = dto.Description,
            Status      = ParseEnum<StorageStatus>(dto.Status,     "storage.status"),
            CreatedAt   = ParseDateTime(dto.CreatedAt, "storage.createdAt"),
            UpdatedAt   = ParseDateTime(dto.UpdatedAt, "storage.updatedAt"),
        };
    }

    public static List<StorageModel> ToStorageList(List<StorageDto>? list)
    {
        EnsureNotNull(list, "Storage[]");
        return list!.Select(ToStorage).ToList();
    }

    // ════════════════════════════════════════════════════════════════════════
    // Cell
    // ════════════════════════════════════════════════════════════════════════

    public static CellModel ToCell(CellDto? dto)
    {
        EnsureNotNull(dto, "CellDto");
        return new CellModel
        {
            Id        = ParseGuid(dto!.Id,          "cell.id"),
            StorageId = ParseGuid(dto.StorageId,    "cell.storageId"),
            Name      = RequireString(dto.Name,     "cell.name"),
            CellClass = RequireString(dto.CellClass,"cell.cellClass"),
            CreatedAt = ParseDateTime(dto.CreatedAt,"cell.createdAt"),
        };
    }

    public static List<CellModel> ToCellList(List<CellDto>? list)
    {
        EnsureNotNull(list, "Cell[]");
        return list!.Select(ToCell).ToList();
    }

    // ════════════════════════════════════════════════════════════════════════
    // Operator
    // ════════════════════════════════════════════════════════════════════════

    public static OperatorModel ToOperator(OperatorDto? dto)
    {
        EnsureNotNull(dto, "OperatorDto");
        return new OperatorModel
        {
            Id        = ParseGuid(dto!.Id,            "operator.id"),
            Username  = RequireString(dto.Username,   "operator.username"),
            Name      = RequireString(dto.Name,       "operator.name"),
            Email     = dto.Email,
            Role      = ParseEnum<OperatorRole>(dto.Role, "operator.role"),
            Banned    = RequireBool(dto.Banned,           "operator.banned"),
            CreatedAt = ParseDateTime(dto.CreatedAt,      "operator.createdAt"),
            UpdatedAt = ParseDateTime(dto.UpdatedAt,      "operator.updatedAt"),
        };
    }

    public static List<OperatorModel> ToOperatorList(List<OperatorDto>? list)
    {
        EnsureNotNull(list, "Operator[]");
        return list!.Select(ToOperator).ToList();
    }

    // ════════════════════════════════════════════════════════════════════════
    // Price
    // ════════════════════════════════════════════════════════════════════════

    public static PriceModel ToPrice(PriceDto? dto)
    {
        EnsureNotNull(dto, "PriceDto");
        return new PriceModel
        {
            StorageId = ParseGuid(dto!.StorageId,    "price.storageId"),
            CellClass = RequireString(dto.CellClass, "price.cellClass"),
            Price     = ParseDecimal(dto.Price,      "price.price"),
            CreatedAt = ParseDateTime(dto.CreatedAt, "price.createdAt"),
        };
    }

    public static List<PriceModel> ToPriceList(List<PriceDto>? list)
    {
        EnsureNotNull(list, "Price[]");
        return list!.Select(ToPrice).ToList();
    }

    // ════════════════════════════════════════════════════════════════════════
    // Booking (user view)
    // ════════════════════════════════════════════════════════════════════════

    public static BookingModel ToBooking(BookingUserDto? dto)
    {
        EnsureNotNull(dto, "BookingUserDto");
        return new BookingModel
        {
            Id          = ParseGuid(dto!.Id,         "booking.id"),
            UserId      = ParseGuid(dto.UserId,      "booking.userId"),
            StorageId   = ParseGuid(dto.StorageId,   "booking.storageId"),
            Storage     = ToStorage(dto.Storage),
            Cells       = ToCellList(dto.Cells),
            StartTime   = ParseDateTime(dto.StartTime,   "booking.startTime"),
            BookingTime = ParseDuration(dto.BookingTime, "booking.bookingTime"),
            CreatedAt   = ParseDateTime(dto.CreatedAt,   "booking.createdAt"),
            Status      = ParseEnum<BookingStatus>(dto.Status, "booking.status"),
        };
    }

    public static List<BookingModel> ToBookingList(List<BookingUserDto>? list)
    {
        EnsureNotNull(list, "Booking[]");
        return list!.Select(ToBooking).ToList();
    }

    // ════════════════════════════════════════════════════════════════════════
    // Booking (operator view)
    // ════════════════════════════════════════════════════════════════════════

    public static BookingOperatorModel ToBookingOperator(BookingOperatorDto? dto)
    {
        EnsureNotNull(dto, "BookingOperatorDto");
        return new BookingOperatorModel
        {
            Id          = ParseGuid(dto!.Id,       "bookingOperator.id"),
            UserId      = ParseGuid(dto.UserId,    "bookingOperator.userId"),
            User        = ToBookingUser(dto.User),
            StorageId   = ParseGuid(dto.StorageId, "bookingOperator.storageId"),
            Cells       = ToCellList(dto.Cells),
            StartTime   = ParseDateTime(dto.StartTime,   "bookingOperator.startTime"),
            BookingTime = ParseDuration(dto.BookingTime, "bookingOperator.bookingTime"),
            CreatedAt   = ParseDateTime(dto.CreatedAt,   "bookingOperator.createdAt"),
            Status      = ParseEnum<BookingStatus>(dto.Status, "bookingOperator.status"),
        };
    }

    public static List<BookingOperatorModel> ToBookingOperatorList(List<BookingOperatorDto>? list)
    {
        EnsureNotNull(list, "BookingOperator[]");
        return list!.Select(ToBookingOperator).ToList();
    }

    private static BookingUserModel ToBookingUser(BookingUserInfoDto? dto)
    {
        EnsureNotNull(dto, "BookingUserInfoDto");
        return new BookingUserModel
        {
            Id    = ParseGuid(dto!.Id,     "bookingUser.id"),
            Name  = RequireString(dto.Name,"bookingUser.name"),
            Email = dto.Email,
        };
    }

    // ════════════════════════════════════════════════════════════════════════
    // BookingReceipt
    // ════════════════════════════════════════════════════════════════════════

    public static BookingReceiptModel ToBookingReceipt(BookingReceiptDto? dto)
    {
        EnsureNotNull(dto, "BookingReceiptDto");

        if (dto!.Booking is null)
            throw new ServerException(
                "Поле 'booking' в ответе на создание бронирования равно null.");

        return new BookingReceiptModel
        {
            Booking = ToBooking(dto.Booking),
            Receipt = RequireString(dto.Receipt, "bookingReceipt.receipt"),
        };
    }

    // ════════════════════════════════════════════════════════════════════════
    // User (self)
    // ════════════════════════════════════════════════════════════════════════

    public static MeModel ToMe(MeDto? dto)
    {
        EnsureNotNull(dto, "MeDto");
        return new MeModel
        {
            Id       = ParseGuid(dto!.Id,          "me.id"),
            Username = RequireString(dto.Username,  "me.username"),
            Name     = RequireString(dto.Name,      "me.name"),
            Email    = dto.Email,
            Role     = ParseEnum<UserRole>(dto.Role,"me.role"),
        };
    }

    // ════════════════════════════════════════════════════════════════════════
    // User (admin view)
    // ════════════════════════════════════════════════════════════════════════

    public static UserModel ToUser(UserDto? dto)
    {
        EnsureNotNull(dto, "UserDto");
        return new UserModel
        {
            Id        = ParseGuid(dto!.Id,          "user.id"),
            Username  = RequireString(dto.Username,  "user.username"),
            Name      = RequireString(dto.Name,      "user.name"),
            Email     = dto.Email,
            Role      = ParseEnum<UserRole>(dto.Role,"user.role"),
            Banned    = RequireBool(dto.Banned,      "user.banned"),
            CreatedAt = ParseDateTime(dto.CreatedAt, "user.createdAt"),
            UpdatedAt = ParseDateTime(dto.UpdatedAt, "user.updatedAt"),
        };
    }

    public static List<UserModel> ToUserList(List<UserDto>? list)
    {
        EnsureNotNull(list, "User[]");
        return list!.Select(ToUser).ToList();
    }

    // ════════════════════════════════════════════════════════════════════════
    // Cell classes
    // ════════════════════════════════════════════════════════════════════════

    public static List<string> ToCellClasses(List<string>? list)
    {
        EnsureNotNull(list, "CellClasses[]");
        return list!;
    }

    // ════════════════════════════════════════════════════════════════════════
    // Primitives & guards
    // ════════════════════════════════════════════════════════════════════════

    private static void EnsureNotNull<T>(T? value, string name) where T : class
    {
        if (value is null)
            throw new ServerException($"Сервер вернул null вместо {name}.");
    }

    private static string RequireString(string? value, string field)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ServerException($"Поле '{field}' пустое или null в ответе API.");
        return value;
    }

    private static bool RequireBool(bool? value, string field)
    {
        if (!value.HasValue)
            throw new ServerException($"Поле '{field}' равно null в ответе API.");
        return value.Value;
    }

    private static Guid ParseGuid(string? value, string field)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ServerException($"Поле '{field}' пустое или null в ответе API.");

        if (!Guid.TryParse(value, out var guid))
            throw new ServerException(
                $"Поле '{field}' содержит некорректный GUID: '{value}'.");

        return guid;
    }

    private static DateTime ParseDateTime(string? value, string field)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ServerException($"Поле '{field}' пустое или null в ответе API.");

        if (!DateTime.TryParse(value, null,
                System.Globalization.DateTimeStyles.RoundtripKind, out var dt))
            throw new ServerException(
                $"Поле '{field}' содержит некорректную дату: '{value}'.");

        return dt;
    }

    private static TimeSpan ParseDuration(string? value, string field)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ServerException($"Поле '{field}' пустое или null в ответе API.");

        try { return System.Xml.XmlConvert.ToTimeSpan(value); }
        catch
        {
            throw new ServerException(
                $"Поле '{field}' содержит некорректную ISO 8601 длительность: '{value}'.");
        }
    }

    private static decimal ParseDecimal(string? value, string field)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ServerException($"Поле '{field}' пустое или null в ответе API.");

        if (!decimal.TryParse(value,
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out var dec))
            throw new ServerException(
                $"Поле '{field}' содержит некорректное число: '{value}'.");

        return dec;
    }

    private static TEnum ParseEnum<TEnum>(string? value, string field)
        where TEnum : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ServerException($"Поле '{field}' пустое или null в ответе API.");

        if (!Enum.TryParse<TEnum>(value, out var result))
            throw new ApiException(0,
                $"Поле '{field}' содержит неизвестное значение '{value}'. " +
                "Возможно, API обновился — обновите приложение.");

        return result;
    }
}
