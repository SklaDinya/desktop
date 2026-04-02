using SklaDinya_desktop_BL_component.Exceptions;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_DA_component.Http;
using SklaDinya_desktop_DA_component.Mapping;

namespace SklaDinya_desktop_DA_component.Repositories;

/// <summary>
/// Репозиторий авторизации и регистрации.
/// </summary>
public class AuthRepository(ApiClient client) : IAuthRepository
{
    /// <inheritdoc/>
    public async Task<string> LoginAsync(LoginForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));

        var body  = Mapper.ToLoginRequest(form);
        var token = await client.PostAsync<string>("/api/v1/auth/login", body);

        if (string.IsNullOrWhiteSpace(token))
            throw new ServerException("Сервер вернул пустой JWT-токен при входе.");

        return token;
    }

    /// <inheritdoc/>
    public async Task<string> RegisterAsync(RegistrationForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));

        var body  = Mapper.ToRegistrationRequest(form);
        var token = await client.PostAsync<string>("/api/v1/auth/register", body);

        if (string.IsNullOrWhiteSpace(token))
            throw new ServerException("Сервер вернул пустой JWT-токен при регистрации.");

        return token;
    }
}
