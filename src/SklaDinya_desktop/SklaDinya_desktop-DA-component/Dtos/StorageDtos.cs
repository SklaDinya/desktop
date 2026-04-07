using SklaDinya_desktop_DA_component.Enums;

namespace SklaDinya_desktop_DA_component.Dtos;

/// <summary>Ответ API: пункт хранения</summary>
public record StorageDto(
    Guid Id,
    string Name,
    string Address,
    string? Description,
    StorageStatusDto Status,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public record StorageCreateRequest(
    string Username,
    string Password,
    string Name,
    string Email,
    string StorageName,
    string Address,
    string? Description);

public record StorageUpdateRequest(
    string? Name,
    string? Address,
    string? Description);
