using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;

namespace SklaDinya_desktop_BL_component.Interfaces.Services;

/// <summary>
/// Сервис для работы с пользователями
/// </summary>
public interface IUserService
{
    /// <summary>Найти пользователей (для администратора)</summary>
    Task<List<UserModel>> GetUsersAsync(UserSearchQuery query);

    /// <summary>Создать пользователя (для администратора)</summary>
    Task<UserModel> CreateUserAsync(UserCreateForm form);

    /// <summary>Получить пользователя по ID (для администратора)</summary>
    Task<UserModel> GetUserByIdAsync(Guid userId);

    /// <summary>Обновить данные пользователя (для администратора)</summary>
    Task<UserModel> UpdateUserAsync(Guid userId, UserUpdateForm form);

    /// <summary>Получить данные своего аккаунта</summary>
    Task<MeModel> GetMeAsync();

    /// <summary>Обновить данные своего аккаунта</summary>
    Task UpdateMeAsync(MeUpdateForm form);
}
