using SklaDinya_desktop_DA_component.Enums;

namespace SklaDinya_desktop_DA_component.Dtos;

/// <summary>Ответ API: данные своего аккаунта</summary>
internal record MeDto(
    Guid        Id,
    string      Username,
    string      Name,
    string?     Email,
    UserRoleDto Role);

/// <summary>Ответ API: данные пользователя для администратора</summary>
internal record UserDto(
    Guid        Id,
    string      Username,
    string      Name,
    string?     Email,
    UserRoleDto Role,
    bool        Banned,
    DateTime    CreatedAt,
    DateTime    UpdatedAt);

internal record UserCreateRequest(
    string      Username,
    string      Password,
    string      Name,
    string?     Email,
    UserRoleDto Role);

internal record UserUpdateRequest(
    string?  Username,
    string?  Password,
    string?  Name,
    string?  Email,
    bool?    Banned);

internal record MeUpdateRequest(
    string? Username,
    string? OldPassword,
    string? NewPassword,
    string? Name,
    string? Email);
