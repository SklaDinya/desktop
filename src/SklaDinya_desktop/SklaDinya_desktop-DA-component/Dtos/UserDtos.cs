namespace SklaDinya_desktop_DA_component.Dtos;

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
