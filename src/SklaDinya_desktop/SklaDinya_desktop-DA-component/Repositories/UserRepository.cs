using SklaDinya_desktop_BL_component.Exceptions;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_DA_component.Dtos;
using SklaDinya_desktop_DA_component.Http;
using SklaDinya_desktop_DA_component.Mapping;

namespace SklaDinya_desktop_DA_component.Repositories;

/// <summary>
/// Репозиторий для работы с пользователями.
/// </summary>
public class UserRepository(ApiClient client) : IUserRepository
{
    /// <inheritdoc/>
    public async Task<List<UserModel>> GetUsersAsync(UserSearchQuery query, string token)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        var roleDto = query.Role.HasValue
            ? Mapper.ToUserRoleDto(query.Role.Value)
            : (Enums.UserRoleDto?)null;

        var url = new QueryBuilder("/api/v1/users")
            .Add("username",   query.Username)
            .Add("name",       query.Name)
            .Add("email",      query.Email)
            .AddEnum("role",   roleDto)
            .Add("pageNumber", query.PageNumber)
            .Add("pageSize",   query.PageSize)
            .Build();

        var dtos = await client.GetAsync<List<UserDto>>(url, token);
        return Mapper.ToUserList(dtos);
    }

    /// <inheritdoc/>
    public async Task<UserModel> CreateUserAsync(UserCreateForm form, string token)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        var body = new UserCreateRequest(
            form.Username,
            form.Password,
            form.Name,
            form.Email,
            Mapper.ToUserRoleDto(form.Role));

        var dto = await client.PostAsync<UserDto>("/api/v1/users", body, token);
        return Mapper.ToUser(dto);
    }

    /// <inheritdoc/>
    public async Task<UserModel> GetUserByIdAsync(Guid userId, string token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        var dto = await client.GetAsync<UserDto>($"/api/v1/users/{userId}", token);
        return Mapper.ToUser(dto);
    }

    /// <inheritdoc/>
    public async Task<UserModel> UpdateUserAsync(Guid userId, UserUpdateForm form, string token)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        var body = new UserUpdateRequest(
            form.Username, form.Password, form.Name, form.Email, form.Banned);

        var dto = await client.PatchAsync<UserDto>($"/api/v1/users/{userId}", body, token);
        return Mapper.ToUser(dto);
    }

    /// <inheritdoc/>
    public async Task<MeModel> GetMeAsync(string token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        var dto = await client.GetAsync<MeDto>("/api/v1/users/me", token);
        return Mapper.ToMe(dto);
    }

    /// <inheritdoc/>
    public async Task<string> UpdateMeAsync(MeUpdateForm form, string token)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        var body = new MeUpdateRequest(
            form.Username, form.OldPassword, form.NewPassword, form.Name, form.Email);

        var newToken = await client.PatchAsync<string>("/api/v1/users/me", body, token);

        if (string.IsNullOrWhiteSpace(newToken))
            throw new ServerException(
                "Сервер вернул пустой JWT-токен при обновлении профиля.");

        return newToken;
    }
}
