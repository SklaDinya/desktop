namespace SklaDinya_desktop_BL_component.Forms;

/// <summary>
/// Форма для обновления пользователя (администратором)
/// </summary>
public class UserUpdateForm
{
    /// <summary>Логин пользователя (необязательно)</summary>
    public string? Username { get; set; }

    /// <summary>Пароль пользователя (необязательно)</summary>
    public string? Password { get; set; }

    /// <summary>Имя пользователя (необязательно)</summary>
    public string? Name { get; set; }

    /// <summary>Почта пользователя (необязательно)</summary>
    public string? Email { get; set; }

    /// <summary>Статус блокировки (необязательно)</summary>
    public bool? Banned { get; set; }
}
