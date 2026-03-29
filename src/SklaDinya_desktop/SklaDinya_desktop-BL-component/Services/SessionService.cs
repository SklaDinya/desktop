using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Interfaces.Services;
using SklaDinya_desktop_BL_component.Models;

namespace SklaDinya_desktop_BL_component.Services;

/// <summary>
/// Реализация сессии приложения.
/// Хранит JWT-токен и данные текущего пользователя в памяти процесса.
/// </summary>
public class SessionService : ISessionService
{
    private string? _token;
    private MeModel? _currentUser;

    public string? Token => _token;

    public UserRole? CurrentRole => _currentUser?.Role;

    public MeModel? CurrentUser => _currentUser;

    public void SetToken(string token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));
        _token = token;
    }

    public void SetCurrentUser(MeModel user)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        _currentUser = user;
    }

    public void Clear()
    {
        _token = null;
        _currentUser = null;
    }

    public bool IsAuthenticated() => _token is not null;
}
