using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Interfaces.Services;

namespace SklaDinya_desktop_BL_component.Services;

/// <summary>
/// Сервис авторизации и регистрации.
/// После успешного входа/регистрации сохраняет токен в сессии.
/// Роль и данные пользователя извлекаются из payload токена — лишний запрос к API не нужен.
/// </summary>
public class AuthService(IAuthRepository authRepository, ISessionService session) : IAuthService
{
    /// <inheritdoc/>
    public async Task LoginAsync(LoginForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));

        var token = await authRepository.LoginAsync(form);
        session.SetToken(token);
    }

    /// <inheritdoc/>
    public async Task RegisterAsync(RegistrationForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));

        var token = await authRepository.RegisterAsync(form);
        session.SetToken(token);
    }

    /// <inheritdoc/>
    public void Logout() => session.Clear();

    /// <inheritdoc/>
    public bool IsAuthenticated() => session.IsAuthenticated();
}
