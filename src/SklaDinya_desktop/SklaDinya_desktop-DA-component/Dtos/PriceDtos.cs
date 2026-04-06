namespace SklaDinya_desktop_DA_component.Dtos;

/// <summary>
/// Ответ API: тариф.
/// Price приходит как decimal128 (строка), парсинг — в маппере.
/// </summary>
public record PriceDto(
    Guid     StorageId,
    string   CellClass,
    string   Price,
    DateTime CreatedAt);

/// <summary>
/// Запрос создания тарифа.
/// Price отправляется как строка в формате decimal128.
/// </summary>
public record PriceCreateRequest(
    string CellClass,
    string Price);
