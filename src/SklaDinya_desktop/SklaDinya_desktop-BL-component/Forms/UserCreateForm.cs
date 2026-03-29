using SklaDinya_desktop_BL_component.Enums;

namespace SklaDinya_desktop_BL_component.Forms;

/// <summary>
/// Форма для создания пользователя (администратором)
/// </summary>
public class UserCreateForm
{
    /// <summary>Логин пользователя</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>Пароль пользователя</summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>Имя пользователя</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Почта пользователя (необязательно)</summary>
    public string? Email { get; set; }

    /// <summary>Роль пользователя</summary>
    public UserRole Role { get; set; }
}
