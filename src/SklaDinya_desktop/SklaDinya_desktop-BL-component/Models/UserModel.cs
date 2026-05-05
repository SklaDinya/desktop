using SklaDinya_desktop_BL_component.Enums;

namespace SklaDinya_desktop_BL_component.Models;

/// <summary>
/// Данные о пользователе (для администратора)
/// </summary>
public class UserModel
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

    /// <summary>Заблокирован ли аккаунт</summary>
    public bool Banned { get; set; }

    /// <summary>Время создания аккаунта</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Время последнего обновления аккаунта</summary>
    public DateTime UpdatedAt { get; set; }
}
