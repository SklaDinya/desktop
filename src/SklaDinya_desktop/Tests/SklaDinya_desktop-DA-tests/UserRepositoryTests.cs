using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Exceptions;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_DA_component.Repositories;
using SklaDinya_desktop_DA_tests.Helpers;

namespace SklaDinya_desktop_DA_tests;

public class UserRepositoryTests
{
    private const string Token = "valid.jwt.token";
    private const string NewToken = "eyJhbGciOiJIUzI1NiJ9.newpayload.sig";

    // ── GetUsersAsync ──────────────────────────────────────────────────────

    [Fact]
    public async Task GetUsersAsync_ServerReturnsOk_ReturnsMappedList()
    {
        var id = Guid.NewGuid();
        var repo = new UserRepository(MockHttpFactory.CreateOk(new[] { FakeDto.User(id) }));
        var query = new UserSearchQuery { PageNumber = 1, PageSize = 10 };

        var result = await repo.GetUsersAsync(query, Token);

        Assert.Single(result);
        Assert.Equal(id, result[0].Id);
        Assert.Equal("testuser", result[0].Username);
    }

    [Fact]
    public async Task GetUsersAsync_ServerReturnsForbidden_ThrowsForbiddenException()
    {
        var repo = new UserRepository(MockHttpFactory.CreateForbidden());
        var query = new UserSearchQuery { PageNumber = 1, PageSize = 10 };

        await Assert.ThrowsAsync<ForbiddenException>(() => repo.GetUsersAsync(query, Token));
    }

    // ── CreateUserAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task CreateUserAsync_ValidArgs_ReturnsMappedUser()
    {
        var id = Guid.NewGuid();
        var repo = new UserRepository(MockHttpFactory.CreateOk(FakeDto.User(id)));
        var form = new UserCreateForm { Username = "u", Password = "p", Name = "N", Email = "e@e.com", Role = UserRole.Client };

        var result = await repo.CreateUserAsync(form, Token);

        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task CreateUserAsync_ServerReturnsConflict_ThrowsConflictException()
    {
        var repo = new UserRepository(MockHttpFactory.CreateConflict());
        var form = new UserCreateForm { Username = "existing", Password = "p", Name = "N", Email = "e@e.com", Role = UserRole.Client };

        await Assert.ThrowsAsync<ConflictException>(() => repo.CreateUserAsync(form, Token));
    }

    // ── GetUserByIdAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task GetUserByIdAsync_ValidArgs_ReturnsMappedUser()
    {
        var id = Guid.NewGuid();
        var repo = new UserRepository(MockHttpFactory.CreateOk(FakeDto.User(id)));

        var result = await repo.GetUserByIdAsync(id, Token);

        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task GetUserByIdAsync_ServerReturnsNotFound_ThrowsNotFoundException()
    {
        var repo = new UserRepository(MockHttpFactory.CreateNotFound());

        await Assert.ThrowsAsync<NotFoundException>(() => repo.GetUserByIdAsync(Guid.NewGuid(), Token));
    }

    [Fact]
    public async Task GetUserByIdAsync_EmptyToken_ThrowsArgumentException()
    {
        var repo = new UserRepository(MockHttpFactory.CreateOk());

        await Assert.ThrowsAsync<ArgumentException>(() => repo.GetUserByIdAsync(Guid.NewGuid(), ""));
    }

    // ── UpdateUserAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateUserAsync_ValidArgs_ReturnsMappedUser()
    {
        var id = Guid.NewGuid();
        var repo = new UserRepository(MockHttpFactory.CreateOk(FakeDto.User(id)));
        var form = new UserUpdateForm { Name = "Updated" };

        var result = await repo.UpdateUserAsync(id, form, Token);

        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task UpdateUserAsync_ServerReturnsNotFound_ThrowsNotFoundException()
    {
        var repo = new UserRepository(MockHttpFactory.CreateNotFound());
        var form = new UserUpdateForm { Name = "Updated" };

        await Assert.ThrowsAsync<NotFoundException>(() => repo.UpdateUserAsync(Guid.NewGuid(), form, Token));
    }

    // ── GetMeAsync ─────────────────────────────────────────────────────────

    [Fact]
    public async Task GetMeAsync_ValidToken_ReturnsMappedMe()
    {
        var id = Guid.NewGuid();
        var repo = new UserRepository(MockHttpFactory.CreateOk(FakeDto.Me(id)));

        var result = await repo.GetMeAsync(Token);

        Assert.Equal(id, result.Id);
        Assert.Equal("me", result.Username);
    }

    [Fact]
    public async Task GetMeAsync_ServerReturnsUnauthorized_ThrowsUnauthorizedException()
    {
        var repo = new UserRepository(MockHttpFactory.CreateUnauthorized());

        await Assert.ThrowsAsync<UnauthorizedException>(() => repo.GetMeAsync(Token));
    }

    [Fact]
    public async Task GetMeAsync_EmptyToken_ThrowsArgumentException()
    {
        var repo = new UserRepository(MockHttpFactory.CreateOk());

        await Assert.ThrowsAsync<ArgumentException>(() => repo.GetMeAsync(""));
    }

    // ── UpdateMeAsync ──────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateMeAsync_ValidArgs_ReturnsNewToken()
    {
        var repo = new UserRepository(MockHttpFactory.CreateOk(NewToken));
        var form = new MeUpdateForm { Username = "u", OldPassword = "o", NewPassword = "n", Name = "N", Email = "e@e.com" };

        var result = await repo.UpdateMeAsync(form, Token);

        Assert.Equal(NewToken, result);
    }

    [Fact]
    public async Task UpdateMeAsync_ServerReturnsEmptyToken_ThrowsServerException()
    {
        var repo = new UserRepository(MockHttpFactory.CreateOk(""));
        var form = new MeUpdateForm { Username = "u", OldPassword = "o", NewPassword = "n", Name = "N", Email = "e@e.com" };

        await Assert.ThrowsAsync<ServerException>(() => repo.UpdateMeAsync(form, Token));
    }
}
