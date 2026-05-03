namespace SklaDinya_desktop_DA_component.Dtos;

/// <summary>
/// Ответ API: тариф.
/// Бэкенд возвращает Price как обычное JSON-число (например, 400.00),
/// а не строку — поэтому десериализуем сразу в decimal.
/// PriceId возвращается сервером, но в BL-модель пока не пробрасываем —
/// храним в DTO для полноты картины при отладке.
/// </summary>
public record PriceDto(
    Guid PriceId,
    Guid StorageId,
    string CellClass,
    decimal Price,
    DateTime CreatedAt);

/// <summary>
/// Запрос создания тарифа.
/// Сервер принимает Price как обычное JSON-число.
/// </summary>
public record PriceCreateRequest(
    string CellClass,
    decimal Price);
