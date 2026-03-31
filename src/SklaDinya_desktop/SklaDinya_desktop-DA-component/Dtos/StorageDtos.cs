using SklaDinya_desktop_DA_component.Enums;

namespace SklaDinya_desktop_DA_component.Dtos;

/// <summary>Ответ API: пункт хранения</summary>
internal record StorageDto(
    Guid             Id,
    string           Name,
    string           Address,
    string?          Description,
    StorageStatusDto Status,
    DateTime         CreatedAt,
    DateTime         UpdatedAt);

internal record StorageCreateRequest(
    string  Username,
    string  Password,
    string  Name,
    string  Email,
    string  StorageName,
    string  Address,
    string? Description);

internal record StorageUpdateRequest(
    string? Name,
    string? Address,
    string? Description);
