using SklaDinya_desktop_DA_component.Enums;

namespace SklaDinya_desktop_DA_component.Dtos;

/// <summary>Ответ API: оператор пункта хранения</summary>
public record OperatorDto(
    Guid Id,
    string Username,
    string Name,
    string? Email,
    OperatorRoleDto Role,
    bool Banned,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public record OperatorCreateRequest(
    string Username,
    string Password,
    string Name,
    string? Email,
    OperatorRoleDto Role);

public record OperatorUpdateRequest(
    string? Username,
    string? Password,
    string? Name,
    string? Email,
    OperatorRoleDto? Role,
    bool? Banned);
