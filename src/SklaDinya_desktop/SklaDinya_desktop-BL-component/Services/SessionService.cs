using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Helpers;
using SklaDinya_desktop_BL_component.Interfaces.Services;
using SklaDinya_desktop_BL_component.Models;

namespace SklaDinya_desktop_BL_component.Services;

/// <summary>
/// Реализация сессии приложения.
/// Хранит JWT-токен и распарсенный payload в памяти процесса.
/// При установке токена автоматически извлекает из него роль и остальные данные.
/// </summary>
public class SessionService : ISessionService
{
    private string? _token;
    private JwtPayload? _payload;

    public string? Token => _token;

    public JwtPayload? Payload => _payload;

    public UserRole? CurrentRole => _payload?.UserRole;

    public void SetToken(string token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));
        _token = token;
        _payload = JwtHelper.ParsePayload(token);
    }

    public void Clear()
    {
        _token = null;
        _payload = null;
    }

    public bool IsAuthenticated() => _token is not null;
}
