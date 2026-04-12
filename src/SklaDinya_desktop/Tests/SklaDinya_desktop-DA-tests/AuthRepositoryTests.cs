using SklaDinya_desktop_BL_component.Exceptions;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_DA_component.Repositories;
using SklaDinya_desktop_DA_tests.Helpers;

namespace SklaDinya_desktop_DA_tests;

public class AuthRepositoryTests
{
    // ── LoginAsync ─────────────────────────────────────────────────────────

    [Fact]
    public async Task LoginAsync_ServerReturnsToken_ReturnsToken()
    {
        var expected = "eyJhbGciOiJIUzI1NiJ9.payload.sig";
        var repo = new AuthRepository(MockHttpFactory.CreateOk(expected));
        var form = new LoginForm { Username = "user", Password = "pass" };

        var result = await repo.LoginAsync(form);

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task LoginAsync_ServerReturnsEmptyToken_ThrowsServerException()
    {
        var repo = new AuthRepository(MockHttpFactory.CreateOk(""));
        var form = new LoginForm { Username = "user", Password = "pass" };

        await Assert.ThrowsAsync<ServerException>(() => repo.LoginAsync(form));
    }

    [Fact]
    public async Task LoginAsync_ServerReturnsUnauthorized_ThrowsUnauthorizedException()
    {
        var repo = new AuthRepository(MockHttpFactory.CreateUnauthorized());
        var form = new LoginForm { Username = "wrong", Password = "wrong" };

        await Assert.ThrowsAsync<UnauthorizedException>(() => repo.LoginAsync(form));
    }

    // ── RegisterAsync ──────────────────────────────────────────────────────

    [Fact]
    public async Task RegisterAsync_ServerReturnsToken_ReturnsToken()
    {
        var expected = "eyJhbGciOiJIUzI1NiJ9.payload.sig";
        var repo = new AuthRepository(MockHttpFactory.CreateOk(expected));
        var form = new RegistrationForm { Username = "newuser", Password = "pass123", Name = "Name", Email = "e@e.com" };

        var result = await repo.RegisterAsync(form);

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task RegisterAsync_ServerReturnsEmptyToken_ThrowsServerException()
    {
        var repo = new AuthRepository(MockHttpFactory.CreateOk(""));
        var form = new RegistrationForm { Username = "user", Password = "pass", Name = "N", Email = "e@e.com" };

        await Assert.ThrowsAsync<ServerException>(() => repo.RegisterAsync(form));
    }

    [Fact]
    public async Task RegisterAsync_ServerReturnsConflict_ThrowsConflictException()
    {
        var repo = new AuthRepository(MockHttpFactory.CreateConflict());
        var form = new RegistrationForm { Username = "existing", Password = "pass", Name = "N", Email = "e@e.com" };

        await Assert.ThrowsAsync<ConflictException>(() => repo.RegisterAsync(form));
    }
}
