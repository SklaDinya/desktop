namespace SklaDinya_desktop_BL_component.Queries;

/// <summary>
/// Параметры поиска камер хранения оператором своего пункта
/// </summary>
public class MyCellSearchQuery : PageQuery
{
    /// <summary>Фильтр по классам ячеек (необязательно)</summary>
    public List<string>? CellClasses { get; set; }
}
