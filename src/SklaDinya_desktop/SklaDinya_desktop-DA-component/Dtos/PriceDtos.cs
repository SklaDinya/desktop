namespace SklaDinya_desktop_DA_component.Dtos;

internal record PriceDto(
    string? StorageId,
    string? CellClass,
    string? Price,
    string? CreatedAt);

internal record PriceCreateRequest(
    string CellClass,
    string Price);
