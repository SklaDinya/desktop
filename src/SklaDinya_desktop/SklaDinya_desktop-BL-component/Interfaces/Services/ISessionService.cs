using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Models;

namespace SklaDinya_desktop_BL_component.Interfaces.Services;

/// <summary>
/// Сервис сессии — хранит JWT-токен и данные из его payload.
/// </summary>
public interface ISessionService
{
    /// <summary>Текущий JWT-токен. Null, если пользователь не авторизован.</summary>
    string? Token { get; }

    /// <summary>Данные из payload текущего токена. Null, если не авторизован.</summary>
    JwtPayload? Payload { get; }

    /// <summary>Роль текущего пользователя. Null, если не авторизован.</summary>
    UserRole? CurrentRole { get; }

    /// <summary>
    /// Сохранить новый JWT-токен и распарсить его payload.
    /// </summary>
    void SetToken(string token);

    /// <summary>Очистить сессию (выход из системы)</summary>
    void Clear();

    /// <summary>Проверить, авторизован ли пользователь</summary>
    bool IsAuthenticated();
}
