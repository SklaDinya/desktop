namespace SklaDinya_desktop_BL_component.Forms;

/// <summary>
/// Форма для регистрации нового пользователя
/// </summary>
public class RegistrationForm
{
    /// <summary>Логин пользователя</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>Пароль пользователя</summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>Имя пользователя</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Почта пользователя (необязательно)</summary>
    public string? Email { get; set; }
}
