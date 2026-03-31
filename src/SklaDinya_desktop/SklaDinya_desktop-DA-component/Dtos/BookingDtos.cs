using System.Text.Json.Serialization;
using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_DA_component.Converters;

namespace SklaDinya_desktop_DA_component.Dtos;

/// <summary>Запрос на создание бронирования. bookingTime — ISO 8601 duration</summary>
internal record BookingCreateRequest(
    Guid       StorageId,
    List<Guid> CellIds,
    DateTime   StartTime,
    [property: JsonConverter(typeof(TimeSpanIso8601Converter))]
    TimeSpan   BookingTime);

/// <summary>Ответ API: бронирование для пользователя</summary>
internal record BookingUserDto(
    Guid          Id,
    Guid          UserId,
    Guid          StorageId,
    StorageDto    Storage,
    List<CellDto> Cells,
    DateTime      StartTime,
    [property: JsonConverter(typeof(TimeSpanIso8601Converter))]
    TimeSpan      BookingTime,
    DateTime      CreatedAt,
    BookingStatus Status);

/// <summary>Ответ API: бронирование для оператора</summary>
internal record BookingOperatorDto(
    Guid               Id,
    Guid               UserId,
    BookingUserInfoDto User,
    Guid               StorageId,
    List<CellDto>      Cells,
    DateTime           StartTime,
    [property: JsonConverter(typeof(TimeSpanIso8601Converter))]
    TimeSpan           BookingTime,
    DateTime           CreatedAt,
    BookingStatus      Status);

/// <summary>Краткие данные о пользователе внутри бронирования оператора</summary>
internal record BookingUserInfoDto(
    Guid    Id,
    string  Name,
    string? Email);

/// <summary>Ответ API: чек созданного бронирования</summary>
internal record BookingReceiptDto(
    BookingUserDto Booking,
    string         Receipt);
