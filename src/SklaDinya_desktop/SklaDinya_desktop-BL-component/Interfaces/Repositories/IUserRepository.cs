using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;

namespace SklaDinya_desktop_BL_component.Interfaces.Repositories;

/// <summary>
/// Репозиторий для работы с пользователями.
/// Все методы защищённые — требуют JWT-токен.
/// </summary>
public interface IUserRepository
{
    /// <summary>Найти пользователей по параметрам (для администратора)</summary>
    Task<List<UserModel>> GetUsersAsync(UserSearchQuery query, string token);

    /// <summary>Создать пользователя (для администратора)</summary>
    Task<UserModel> CreateUserAsync(UserCreateForm form, string token);

    /// <summary>Получить пользователя по ID (для администратора)</summary>
    Task<UserModel> GetUserByIdAsync(Guid userId, string token);

    /// <summary>Обновить данные пользователя (для администратора)</summary>
    Task<UserModel> UpdateUserAsync(Guid userId, UserUpdateForm form, string token);

    /// <summary>Получить данные своего аккаунта</summary>
    Task<MeModel> GetMeAsync(string token);

    /// <summary>Обновить данные своего аккаунта. Возвращает новый JWT-токен.</summary>
    Task<string> UpdateMeAsync(MeUpdateForm form, string token);
}
