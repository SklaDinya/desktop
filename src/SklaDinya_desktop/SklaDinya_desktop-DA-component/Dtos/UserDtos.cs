using SklaDinya_desktop_DA_component.Enums;

namespace SklaDinya_desktop_DA_component.Dtos;

/// <summary>Ответ API: данные своего аккаунта</summary>
public record MeDto(
    Guid Id,
    string Username,
    string Name,
    string? Email,
    UserRoleDto Role);

/// <summary>Ответ API: данные пользователя для администратора</summary>
public record UserDto(
    Guid Id,
    string Username,
    string Name,
    string? Email,
    UserRoleDto Role,
    bool Banned,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public record UserCreateRequest(
    string Username,
    string Password,
    string Name,
    string? Email,
    UserRoleDto Role);

public record UserUpdateRequest(
    string? Username,
    string? Password,
    string? Name,
    string? Email,
    bool? Banned);

public record MeUpdateRequest(
    string? Username,
    string? OldPassword,
    string? NewPassword,
    string? Name,
    string? Email);
