using SklaDinya_desktop_BL_component.Enums;

namespace SklaDinya_desktop_BL_component.Queries;

/// <summary>
/// Параметры поиска операторов пункта хранения
/// </summary>
public class OperatorSearchQuery : PageQuery
{
    /// <summary>Фильтр по логину</summary>
    public string? Username { get; set; }

    /// <summary>Фильтр по имени</summary>
    public string? Name { get; set; }

    /// <summary>Фильтр по почте</summary>
    public string? Email { get; set; }

    /// <summary>Фильтр по роли оператора</summary>
    public OperatorRole? Role { get; set; }
}
