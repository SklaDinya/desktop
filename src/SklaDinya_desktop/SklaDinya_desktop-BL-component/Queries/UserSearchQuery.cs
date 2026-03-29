using SklaDinya_desktop_BL_component.Enums;

namespace SklaDinya_desktop_BL_component.Queries;

/// <summary>
/// Параметры поиска пользователей
/// </summary>
public class UserSearchQuery : PageQuery
{
    /// <summary>Фильтр по логину</summary>
    public string? Username { get; set; }

    /// <summary>Фильтр по имени</summary>
    public string? Name { get; set; }

    /// <summary>Фильтр по почте</summary>
    public string? Email { get; set; }

    /// <summary>Фильтр по роли</summary>
    public UserRole? Role { get; set; }
}
