namespace SklaDinya_desktop_DA_component.Dtos;

// ════════════════════════════════════════════════════════════════════════════
// Auth
// ════════════════════════════════════════════════════════════════════════════

internal record LoginRequest(
    string Username,
    string Password);

internal record RegistrationRequest(
    string Username,
    string Password,
    string Name,
    string? Email);

// ════════════════════════════════════════════════════════════════════════════
// Storage
// ════════════════════════════════════════════════════════════════════════════

internal record StorageDto(
    string? Id,
    string? Name,
    string? Address,
    string? Description,
    string? Status,
    string? CreatedAt,
    string? UpdatedAt);

internal record StorageCreateRequest(
    string Username,
    string Password,
    string Name,
    string Email,
    string StorageName,
    string Address,
    string? Description);

internal record StorageUpdateRequest(
    string? Name,
    string? Address,
    string? Description);

// ════════════════════════════════════════════════════════════════════════════
// Cell
// ════════════════════════════════════════════════════════════════════════════

internal record CellDto(
    string? Id,
    string? StorageId,
    string? Name,
    string? CellClass,
    string? CreatedAt);

internal record CellCreateRequest(
    string Name,
    string CellClass);

// ════════════════════════════════════════════════════════════════════════════
// Operator
// ════════════════════════════════════════════════════════════════════════════

internal record OperatorDto(
    string? Id,
    string? Username,
    string? Name,
    string? Email,
    string? Role,
    bool?   Banned,
    string? CreatedAt,
    string? UpdatedAt);

internal record OperatorCreateRequest(
    string  Username,
    string  Password,
    string  Name,
    string? Email,
    string  Role);

internal record OperatorUpdateRequest(
    string? Username,
    string? Password,
    string? Name,
    string? Email,
    string? Role,
    bool?   Banned);

// ════════════════════════════════════════════════════════════════════════════
// Price
// ════════════════════════════════════════════════════════════════════════════

internal record PriceDto(
    string? StorageId,
    string? CellClass,
    string? Price,
    string? CreatedAt);

internal record PriceCreateRequest(
    string CellClass,
    string Price);

// ════════════════════════════════════════════════════════════════════════════
// Booking
// ════════════════════════════════════════════════════════════════════════════

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

// ════════════════════════════════════════════════════════════════════════════
// Payment
// ════════════════════════════════════════════════════════════════════════════

internal record PaymentRequest(string Receipt);

// ════════════════════════════════════════════════════════════════════════════
// User
// ════════════════════════════════════════════════════════════════════════════

internal record MeDto(
    string? Id,
    string? Username,
    string? Name,
    string? Email,
    string? Role);

internal record UserDto(
    string? Id,
    string? Username,
    string? Name,
    string? Email,
    string? Role,
    bool?   Banned,
    string? CreatedAt,
    string? UpdatedAt);

internal record UserCreateRequest(
    string  Username,
    string  Password,
    string  Name,
    string? Email,
    string  Role);

internal record UserUpdateRequest(
    string? Username,
    string? Password,
    string? Name,
    string? Email,
    bool?   Banned);

internal record MeUpdateRequest(
    string? Username,
    string? OldPassword,
    string? NewPassword,
    string? Name,
    string? Email);
