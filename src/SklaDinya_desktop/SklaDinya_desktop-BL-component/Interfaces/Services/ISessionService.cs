using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Models;

namespace SklaDinya_desktop_BL_component.Interfaces.Services;

/// <summary>
/// Сервис сессии — хранит JWT-токен и данные текущего авторизованного пользователя.
/// Используется всеми остальными сервисами и репозиториями для получения токена.
/// </summary>
public interface ISessionService
{
    /// <summary>Текущий JWT-токен. Null, если пользователь не авторизован.</summary>
    string? Token { get; }

    /// <summary>Роль текущего пользователя. Null, если не авторизован.</summary>
    UserRole? CurrentRole { get; }

    /// <summary>Данные текущего пользователя. Null, если не авторизован.</summary>
    MeModel? CurrentUser { get; }

    /// <summary>Сохранить новый JWT-токен (вызывается после логина/регистрации/обновления профиля)</summary>
    void SetToken(string token);

    /// <summary>Сохранить данные текущего пользователя</summary>
    void SetCurrentUser(MeModel user);

    /// <summary>Очистить сессию (выход из системы)</summary>
    void Clear();

    /// <summary>Проверить, авторизован ли пользователь</summary>
    bool IsAuthenticated();
}
