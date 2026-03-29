using SklaDinya_desktop_BL_component.Enums;

namespace SklaDinya_desktop_BL_component.Models;

/// <summary>
/// Данные о текущем авторизованном пользователе
/// </summary>
public class MeModel
{
    /// <summary>Идентификатор пользователя</summary>
    public Guid Id { get; set; }

    /// <summary>Логин пользователя</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>Имя пользователя</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Почта пользователя</summary>
    public string? Email { get; set; }

    /// <summary>Роль пользователя</summary>
    public UserRole Role { get; set; }
}
