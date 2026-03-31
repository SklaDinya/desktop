namespace SklaDinya_desktop_DA_component.Dtos;

/// <summary>Ответ API: камера хранения</summary>
internal record CellDto(
    Guid     Id,
    Guid     StorageId,
    string   Name,
    string   CellClass,
    DateTime CreatedAt);

internal record CellCreateRequest(
    string Name,
    string CellClass);
