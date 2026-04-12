using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Exceptions;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_BL_DA_IntegrationalTests.Helpers;

namespace SklaDinya_desktop_BL_DA_IntegrationalTests;

/// <summary>
/// Интеграционные тесты UserService.
/// Цепочка: UserService → UserRepository → ApiClient(mock HTTP).
/// Особый случай: UpdateMeAsync возвращает новый JWT-токен, который
/// должен быть сохранён в SessionService — проверяем это сквозно.
/// </summary>
public class UserServiceIntegrationTests
{
    private static readonly string Token = FakeDto.Token();
    private static readonly string NewToken = FakeDto.Token();

    // ── GetUsersAsync ──────────────────────────────────────────────────────

    [Fact]
    public async Task GetUsersAsync_ServerReturnsOk_ReturnsMappedList()
    {
        var id = Guid.NewGuid();
        var session = ServiceFactory.Session(Token);
        var sut = ServiceFactory.User(
            MockHttpFactory.CreateOk(new[] { FakeDto.User(id) }), session);

        var result = await sut.GetUsersAsync(new UserSearchQuery { PageNumber = 1, PageSize = 10 });

        Assert.Single(result);
        Assert.Equal(id, result[0].Id);
        Assert.Equal("testuser", result[0].Username);
        Assert.Equal(UserRole.Client, result[0].Role);
    }

    [Fact]
    public async Task GetUsersAsync_ServerReturnsForbidden_ThrowsForbiddenException()
    {
        var session = ServiceFactory.Session(Token);
        var sut = ServiceFactory.User(MockHttpFactory.CreateForbidden(), session);

        await Assert.ThrowsAsync<ForbiddenException>(
            () => sut.GetUsersAsync(new UserSearchQuery()));
    }

    // ── CreateUserAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task CreateUserAsync_ServerReturnsOk_ReturnsMappedUser()
    {
        var id = Guid.NewGuid();
        var session = ServiceFactory.Session(Token);
        var sut = ServiceFactory.User(MockHttpFactory.CreateOk(FakeDto.User(id)), session);

        var result = await sut.CreateUserAsync(new UserCreateForm
        {
            Username = "u",
            Password = "p",
            Name = "N",
            Email = "e@e.com",
            Role = UserRole.Client
        });

        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task CreateUserAsync_ServerReturnsConflict_ThrowsConflictException()
    {
        var session = ServiceFactory.Session(Token);
        var sut = ServiceFactory.User(MockHttpFactory.CreateConflict(), session);

        await Assert.ThrowsAsync<ConflictException>(() => sut.CreateUserAsync(new UserCreateForm
        {
            Username = "existing",
            Password = "p",
            Name = "N",
            Email = "e@e.com",
            Role = UserRole.Client
        }));
    }

    // ── GetUserByIdAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task GetUserByIdAsync_ServerReturnsOk_ReturnsMappedUser()
    {
        var id = Guid.NewGuid();
        var session = ServiceFactory.Session(Token);
        var sut = ServiceFactory.User(MockHttpFactory.CreateOk(FakeDto.User(id)), session);

        var result = await sut.GetUserByIdAsync(id);

        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task GetUserByIdAsync_ServerReturnsNotFound_ThrowsNotFoundException()
    {
        var session = ServiceFactory.Session(Token);
        var sut = ServiceFactory.User(MockHttpFactory.CreateNotFound(), session);

        await Assert.ThrowsAsync<NotFoundException>(
            () => sut.GetUserByIdAsync(Guid.NewGuid()));
    }

    // ── UpdateUserAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateUserAsync_ServerReturnsOk_ReturnsMappedUser()
    {
        var id = Guid.NewGuid();
        var session = ServiceFactory.Session(Token);
        var sut = ServiceFactory.User(MockHttpFactory.CreateOk(FakeDto.User(id)), session);

        var result = await sut.UpdateUserAsync(id, new UserUpdateForm { Name = "Updated" });

        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task UpdateUserAsync_ServerReturnsNotFound_ThrowsNotFoundException()
    {
        var session = ServiceFactory.Session(Token);
        var sut = ServiceFactory.User(MockHttpFactory.CreateNotFound(), session);

        await Assert.ThrowsAsync<NotFoundException>(
            () => sut.UpdateUserAsync(Guid.NewGuid(), new UserUpdateForm { Name = "Updated" }));
    }

    // ── GetMeAsync ─────────────────────────────────────────────────────────

    [Fact]
    public async Task GetMeAsync_ServerReturnsOk_ReturnsMappedMe()
    {
        var id = Guid.NewGuid();
        var session = ServiceFactory.Session(Token);
        var sut = ServiceFactory.User(MockHttpFactory.CreateOk(FakeDto.Me(id)), session);

        var result = await sut.GetMeAsync();

        Assert.Equal(id, result.Id);
        Assert.Equal("me", result.Username);
        Assert.Equal(UserRole.Client, result.Role);
    }

    [Fact]
    public async Task GetMeAsync_ServerReturnsUnauthorized_ThrowsUnauthorizedException()
    {
        var session = ServiceFactory.Session(Token);
        var sut = ServiceFactory.User(MockHttpFactory.CreateUnauthorized(), session);

        await Assert.ThrowsAsync<UnauthorizedException>(() => sut.GetMeAsync());
    }

    // ── UpdateMeAsync ──────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateMeAsync_ServerReturnsNewToken_NewTokenSavedInSession()
    {
        var session = ServiceFactory.Session(Token);
        var sut = ServiceFactory.User(MockHttpFactory.CreateOk(NewToken), session);

        await sut.UpdateMeAsync(new MeUpdateForm
        {
            Username = "newname",
            OldPassword = "old",
            NewPassword = "new123",
            Name = "New Name",
            Email = "new@e.com"
        });

        // Ключевая проверка интеграции: новый токен должен быть сохранён в сессии
        Assert.Equal(NewToken, session.Token);
    }

    [Fact]
    public async Task UpdateMeAsync_ServerReturnsUnauthorized_ThrowsUnauthorizedException()
    {
        var session = ServiceFactory.Session(Token);
        var sut = ServiceFactory.User(MockHttpFactory.CreateUnauthorized(), session);

        await Assert.ThrowsAsync<UnauthorizedException>(() => sut.UpdateMeAsync(new MeUpdateForm
        {
            Username = "u",
            OldPassword = "o",
            NewPassword = "n"
        }));

        // Токен в сессии не должен измениться после ошибки
        Assert.Equal(Token, session.Token);
    }
}
