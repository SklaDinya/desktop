namespace SklaDinya_desktop_DA_component.Dtos;

internal record CellDto(
    string? Id,
    string? StorageId,
    string? Name,
    string? CellClass,
    string? CreatedAt);

internal record CellCreateRequest(
    string Name,
    string CellClass);
