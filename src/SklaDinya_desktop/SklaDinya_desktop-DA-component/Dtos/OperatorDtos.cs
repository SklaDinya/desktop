using SklaDinya_desktop_BL_component.Enums;

namespace SklaDinya_desktop_DA_component.Dtos;

/// <summary>Ответ API: оператор пункта хранения</summary>
internal record OperatorDto(
    Guid         Id,
    string       Username,
    string       Name,
    string?      Email,
    OperatorRole Role,
    bool         Banned,
    DateTime     CreatedAt,
    DateTime     UpdatedAt);

internal record OperatorCreateRequest(
    string       Username,
    string       Password,
    string       Name,
    string?      Email,
    OperatorRole Role);

internal record OperatorUpdateRequest(
    string?      Username,
    string?      Password,
    string?      Name,
    string?      Email,
    OperatorRole? Role,
    bool?        Banned);
