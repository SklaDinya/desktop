using SklaDinya_desktop_DA_component.Enums;

namespace SklaDinya_desktop_DA_component.Dtos;

/// <summary>Ответ API: оператор пункта хранения</summary>
internal record OperatorDto(
    Guid            Id,
    string          Username,
    string          Name,
    string?         Email,
    OperatorRoleDto Role,
    bool            Banned,
    DateTime        CreatedAt,
    DateTime        UpdatedAt);

internal record OperatorCreateRequest(
    string          Username,
    string          Password,
    string          Name,
    string?         Email,
    OperatorRoleDto Role);

internal record OperatorUpdateRequest(
    string?          Username,
    string?          Password,
    string?          Name,
    string?          Email,
    OperatorRoleDto? Role,
    bool?            Banned);
