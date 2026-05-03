using SklaDinya_desktop_DA_component.Converters;
using SklaDinya_desktop_DA_component.Enums;
using System.Text.Json.Serialization;

namespace SklaDinya_desktop_DA_component.Dtos;

/// <summary>Запрос на создание бронирования. bookingTime — ISO 8601 duration</summary>
public record BookingCreateRequest(
    Guid StorageId,
    List<Guid> CellIds,
    DateTime StartTime,
    [property: JsonConverter(typeof(TimeSpanIso8601Converter))]
    TimeSpan   BookingTime);

/// <summary>
/// Ответ API: бронирование для пользователя.
/// Поле <c>price</c> теперь возвращается сервером — клиент больше не считает
/// стоимость самостоятельно. Тип <see cref="decimal"/> подходит и для целых
/// (993), и для дробных значений (4.50).
/// </summary>
public record BookingUserDto(
    Guid Id,
    Guid UserId,
    Guid StorageId,
    StorageDto Storage,
    List<CellDto> Cells,
    DateTime StartTime,
    [property: JsonConverter(typeof(TimeSpanIso8601Converter))]
    TimeSpan         BookingTime,
    decimal Price,
    DateTime CreatedAt,
    BookingStatusDto Status);

/// <summary>
/// Ответ API: бронирование для оператора.
/// По swagger поле <c>price</c> здесь не помечено как required, но фактически
/// присутствует — десериализуем так же, как в <see cref="BookingUserDto"/>.
/// </summary>
public record BookingOperatorDto(
    Guid Id,
    Guid UserId,
    BookingUserInfoDto User,
    Guid StorageId,
    List<CellDto> Cells,
    DateTime StartTime,
    [property: JsonConverter(typeof(TimeSpanIso8601Converter))]
    TimeSpan           BookingTime,
    decimal Price,
    DateTime CreatedAt,
    BookingStatusDto Status);

/// <summary>Краткие данные о пользователе внутри бронирования оператора</summary>
public record BookingUserInfoDto(
    Guid Id,
    string Name,
    string? Email);

/// <summary>Ответ API: чек созданного бронирования</summary>
public record BookingReceiptDto(
    BookingUserDto Booking,
    string Receipt);
