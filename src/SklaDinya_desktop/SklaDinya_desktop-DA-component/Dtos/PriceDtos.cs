namespace SklaDinya_desktop_DA_component.Dtos;

/// <summary>
/// Ответ API: тариф.
/// </summary>
public record PriceDto(
    Guid PriceId,
    Guid StorageId,
    string CellClass,
    decimal Price,
    DateTime CreatedAt);

/// <summary>
/// Запрос создания тарифа.
/// </summary>
public record PriceCreateRequest(
    string CellClass,
    decimal Price);
