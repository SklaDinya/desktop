namespace SklaDinya_desktop_BL_component.Forms;

/// <summary>
/// Форма для создания тарифа
/// </summary>
public class PriceCreateForm
{
    /// <summary>Класс ячеек, для которых устанавливается тариф</summary>
    public string CellClass { get; set; } = string.Empty;

    /// <summary>Цена за единицу времени (час)</summary>
    public decimal Price { get; set; }
}
