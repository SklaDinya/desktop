using SklaDinya_desktop_BL_component.Forms;

namespace SklaDinya_desktop_BL_component.Interfaces.Repositories;

/// <summary>
/// Репозиторий для авторизации и регистрации
/// </summary>
public interface IAuthRepository
{
    /// <summary>
    /// Войти в систему. Возвращает JWT-токен.
    /// </summary>
    Task<string> LoginAsync(LoginForm form);

    /// <summary>
    /// Зарегистрироваться. Возвращает JWT-токен.
    /// </summary>
    Task<string> RegisterAsync(RegistrationForm form);
}
