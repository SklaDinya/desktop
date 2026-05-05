using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Interfaces.Services;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;

namespace SklaDinya_desktop_BL_component.Services;

/// <summary>
/// Сервис для работы с пользователями.
/// </summary>
public class UserService(IUserRepository userRepository, ISessionService session) : IUserService
{
    /// <inheritdoc/>
    public Task<List<UserModel>> GetUsersAsync(UserSearchQuery query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));
        return userRepository.GetUsersAsync(query, session.Token!);
    }

    /// <inheritdoc/>
    public Task<UserModel> CreateUserAsync(UserCreateForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        return userRepository.CreateUserAsync(form, session.Token!);
    }

    /// <inheritdoc/>
    public Task<UserModel> GetUserByIdAsync(Guid userId) =>
        userRepository.GetUserByIdAsync(userId, session.Token!);

    /// <inheritdoc/>
    public Task<UserModel> UpdateUserAsync(Guid userId, UserUpdateForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        return userRepository.UpdateUserAsync(userId, form, session.Token!);
    }

    /// <inheritdoc/>
    public Task<MeModel> GetMeAsync() =>
        userRepository.GetMeAsync(session.Token!);

    /// <inheritdoc/>
    public async Task UpdateMeAsync(MeUpdateForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));

        var newToken = await userRepository.UpdateMeAsync(form, session.Token!);
        session.SetToken(newToken);
    }
}
