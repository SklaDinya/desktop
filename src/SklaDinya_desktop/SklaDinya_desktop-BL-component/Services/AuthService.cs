using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Interfaces.Services;

namespace SklaDinya_desktop_BL_component.Services;

/// <summary>
/// Сервис авторизации и регистрации.
/// После успешного входа/регистрации сохраняет токен в сессии
/// и немедленно загружает данные текущего пользователя.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IUserRepository _userRepository;
    private readonly ISessionService _session;

    public AuthService(
        IAuthRepository authRepository,
        IUserRepository userRepository,
        ISessionService session)
    {
        _authRepository = authRepository;
        _userRepository = userRepository;
        _session = session;
    }

    /// <inheritdoc/>
    public async Task LoginAsync(LoginForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));

        var token = await _authRepository.LoginAsync(form);
        _session.SetToken(token);

        // Сразу загружаем профиль, чтобы знать роль пользователя
        var me = await _userRepository.GetMeAsync();
        _session.SetCurrentUser(me);
    }

    /// <inheritdoc/>
    public async Task RegisterAsync(RegistrationForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));

        var token = await _authRepository.RegisterAsync(form);
        _session.SetToken(token);

        var me = await _userRepository.GetMeAsync();
        _session.SetCurrentUser(me);
    }

    /// <inheritdoc/>
    public void Logout()
    {
        _session.Clear();
    }

    /// <inheritdoc/>
    public bool IsAuthenticated() => _session.IsAuthenticated();
}
