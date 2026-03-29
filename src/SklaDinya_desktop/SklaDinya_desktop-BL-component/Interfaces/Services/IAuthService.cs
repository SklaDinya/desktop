using SklaDinya_desktop_BL_component.Forms;

namespace SklaDinya_desktop_BL_component.Interfaces.Services;

/// <summary>
/// Сервис авторизации и регистрации
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Войти в систему. Сохраняет токен в сессии.
    /// </summary>
    Task LoginAsync(LoginForm form);

    /// <summary>
    /// Зарегистрироваться. Сохраняет токен в сессии.
    /// </summary>
    Task RegisterAsync(RegistrationForm form);

    /// <summary>
    /// Выйти из системы — сбросить токен сессии.
    /// </summary>
    void Logout();

    /// <summary>
    /// Проверить, авторизован ли пользователь в текущей сессии.
    /// </summary>
    bool IsAuthenticated();
}
