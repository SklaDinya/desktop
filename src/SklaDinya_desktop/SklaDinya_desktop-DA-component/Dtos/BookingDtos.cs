namespace SklaDinya_desktop_DA_component.Dtos;

internal record BookingCreateRequest(
    string       StorageId,
    List<string> CellIds,
    string       StartTime,
    string       BookingTime);

internal record BookingUserDto(
    string?        Id,
    string?        UserId,
    string?        StorageId,
    StorageDto?    Storage,
    List<CellDto>? Cells,
    string?        StartTime,
    string?        BookingTime,
    string?        CreatedAt,
    string?        Status);

internal record BookingOperatorDto(
    string?             Id,
    string?             UserId,
    BookingUserInfoDto? User,
    string?             StorageId,
    List<CellDto>?      Cells,
    string?             StartTime,
    string?             BookingTime,
    string?             CreatedAt,
    string?             Status);

internal record BookingUserInfoDto(
    string? Id,
    string? Name,
    string? Email);

internal record BookingReceiptDto(
    BookingUserDto? Booking,
    string?         Receipt);
