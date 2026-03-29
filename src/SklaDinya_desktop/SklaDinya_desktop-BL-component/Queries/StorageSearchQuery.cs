namespace SklaDinya_desktop_BL_component.Queries;

/// <summary>
/// Параметры поиска пунктов хранения
/// </summary>
public class StorageSearchQuery : PageQuery
{
    /// <summary>Фильтр по названию пункта хранения</summary>
    public string? Name { get; set; }

    /// <summary>Фильтр по адресу</summary>
    public string? Address { get; set; }
}
