namespace SklaDinya_desktop_BL_component.Models;

/// <summary>
/// Данные о тарифе пункта хранения
/// </summary>
public class PriceModel
{
    /// <summary>Идентификатор пункта хранения</summary>
    public Guid StorageId { get; set; }

    /// <summary>Класс ячеек, для которых действует тариф</summary>
    public string CellClass { get; set; } = string.Empty;

    /// <summary>Цена за единицу времени (час)</summary>
    public decimal Price { get; set; }

    /// <summary>Время установления тарифа</summary>
    public DateTime CreatedAt { get; set; }
}
