namespace SklaDinya_desktop_BL_component.Queries;

/// <summary>
/// Базовые параметры пагинации
/// </summary>
public class PageQuery
{
    /// <summary>Номер страницы (начиная с 0)</summary>
    public int? PageNumber { get; set; }

    /// <summary>Размер страницы</summary>
    public int? PageSize { get; set; }
}
