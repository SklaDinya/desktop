namespace SklaDinya_desktop_BL_component.Queries;

/// <summary>
/// Параметры поиска доступных камер хранения (публичный запрос)
/// </summary>
public class CellSearchQuery : PageQuery
{
    /// <summary>Время начала желаемого бронирования</summary>
    public DateTime StartBooking { get; set; }

    /// <summary>Желаемая длительность бронирования</summary>
    public TimeSpan TimeBooking { get; set; }

    /// <summary>Фильтр по классам ячеек (необязательно)</summary>
    public List<string>? CellClasses { get; set; }
}
