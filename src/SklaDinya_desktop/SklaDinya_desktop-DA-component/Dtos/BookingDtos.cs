using System.Text.Json.Serialization;
using SklaDinya_desktop_DA_component.Converters;
using SklaDinya_desktop_DA_component.Enums;

namespace SklaDinya_desktop_DA_component.Dtos;

/// <summary>Запрос на создание бронирования. bookingTime — ISO 8601 duration</summary>
public record BookingCreateRequest(
    Guid       StorageId,
    List<Guid> CellIds,
    DateTime   StartTime,
    [property: JsonConverter(typeof(TimeSpanIso8601Converter))]
    TimeSpan   BookingTime);

/// <summary>Ответ API: бронирование для пользователя</summary>
public record BookingUserDto(
    Guid             Id,
    Guid             UserId,
    Guid             StorageId,
    StorageDto       Storage,
    List<CellDto>    Cells,
    DateTime         StartTime,
    [property: JsonConverter(typeof(TimeSpanIso8601Converter))]
    TimeSpan         BookingTime,
    DateTime         CreatedAt,
    BookingStatusDto Status);

/// <summary>Ответ API: бронирование для оператора</summary>
public record BookingOperatorDto(
    Guid               Id,
    Guid               UserId,
    BookingUserInfoDto User,
    Guid               StorageId,
    List<CellDto>      Cells,
    DateTime           StartTime,
    [property: JsonConverter(typeof(TimeSpanIso8601Converter))]
    TimeSpan           BookingTime,
    DateTime           CreatedAt,
    BookingStatusDto   Status);

/// <summary>Краткие данные о пользователе внутри бронирования оператора</summary>
public record BookingUserInfoDto(
    Guid    Id,
    string  Name,
    string? Email);

/// <summary>Ответ API: чек созданного бронирования</summary>
public record BookingReceiptDto(
    BookingUserDto Booking,
    string         Receipt);
