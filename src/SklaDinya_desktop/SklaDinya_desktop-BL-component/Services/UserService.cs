using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Interfaces.Services;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;

namespace SklaDinya_desktop_BL_component.Services;

/// <summary>
/// Сервис для работы с пользователями.
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ISessionService _session;

    public UserService(IUserRepository userRepository, ISessionService session)
    {
        _userRepository = userRepository;
        _session = session;
    }

    /// <inheritdoc/>
    public Task<List<UserModel>> GetUsersAsync(UserSearchQuery query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));
        return _userRepository.GetUsersAsync(query);
    }

    /// <inheritdoc/>
    public Task<UserModel> CreateUserAsync(UserCreateForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        return _userRepository.CreateUserAsync(form);
    }

    /// <inheritdoc/>
    public Task<UserModel> GetUserByIdAsync(Guid userId) =>
        _userRepository.GetUserByIdAsync(userId);

    /// <inheritdoc/>
    public Task<UserModel> UpdateUserAsync(Guid userId, UserUpdateForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        return _userRepository.UpdateUserAsync(userId, form);
    }

    /// <inheritdoc/>
    public Task<MeModel> GetMeAsync() =>
        _userRepository.GetMeAsync();

    /// <inheritdoc/>
    public async Task UpdateMeAsync(MeUpdateForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));

        // Сервер возвращает новый токен (могли измениться username/password)
        var newToken = await _userRepository.UpdateMeAsync(form);
        _session.SetToken(newToken);

        // Обновляем кешированные данные пользователя в сессии
        var me = await _userRepository.GetMeAsync();
        _session.SetCurrentUser(me);
    }
}
