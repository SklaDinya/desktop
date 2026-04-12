namespace SklaDinya_desktop_DA_component.Dtos;

/// <summary>Ответ API: камера хранения</summary>
public record CellDto(
    Guid Id,
    Guid StorageId,
    string Name,
    string CellClass,
    DateTime CreatedAt);

public record CellCreateRequest(
    string Name,
    string CellClass);
