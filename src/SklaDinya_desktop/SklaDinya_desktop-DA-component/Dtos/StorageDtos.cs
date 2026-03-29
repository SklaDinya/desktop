namespace SklaDinya_desktop_DA_component.Dtos;

internal record StorageDto(
    string? Id,
    string? Name,
    string? Address,
    string? Description,
    string? Status,
    string? CreatedAt,
    string? UpdatedAt);

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
