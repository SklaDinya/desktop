using SklaDinya_desktop_BL_component.Exceptions;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_DA_IntegrationalTests.Helpers;

namespace SklaDinya_desktop_BL_DA_IntegrationalTests;

/// <summary>
/// Интеграционные тесты AuthService.
/// Цепочка: AuthService → AuthRepository → ApiClient(mock HTTP).
/// Проверяет, что после успешного логина/регистрации токен попадает в сессию,
/// а HTTP-ошибки корректно преобразуются в типизированные исключения BL-слоя.
/// </summary>
public class AuthServiceIntegrationTests
{
    // ── LoginAsync ─────────────────────────────────────────────────────────

    [Fact]
    public async Task LoginAsync_ServerReturnsToken_TokenSavedInSession()
    {
        var token   = FakeDto.Token();
        var session = ServiceFactory.Session();
        var sut     = ServiceFactory.Auth(MockHttpFactory.CreateOk(token), session);

        await sut.LoginAsync(new LoginForm { Username = "user", Password = "pass" });

        Assert.True(sut.IsAuthenticated());
        Assert.Equal(token, session.Token);
    }

    [Fact]
    public async Task LoginAsync_ServerReturnsUnauthorized_ThrowsUnauthorizedExceptionAndSessionEmpty()
    {
        var session = ServiceFactory.Session();
        var sut     = ServiceFactory.Auth(MockHttpFactory.CreateUnauthorized(), session);

        await Assert.ThrowsAsync<UnauthorizedException>(
            () => sut.LoginAsync(new LoginForm { Username = "wrong", Password = "wrong" }));

        Assert.False(sut.IsAuthenticated());
    }

    [Fact]
    public async Task LoginAsync_NullForm_ThrowsArgumentNullException()
    {
        var sut = ServiceFactory.Auth(MockHttpFactory.CreateOk(), ServiceFactory.Session());

        await Assert.ThrowsAsync<ArgumentNullException>(() => sut.LoginAsync(null!));
    }

    // ── RegisterAsync ──────────────────────────────────────────────────────

    [Fact]
    public async Task RegisterAsync_ServerReturnsToken_TokenSavedInSession()
    {
        var token   = FakeDto.Token();
        var session = ServiceFactory.Session();
        var sut     = ServiceFactory.Auth(MockHttpFactory.CreateOk(token), session);

        await sut.RegisterAsync(new RegistrationForm { Username = "new", Password = "pass", Name = "N", Email = "e@e.com" });

        Assert.True(sut.IsAuthenticated());
        Assert.Equal(token, session.Token);
    }

    [Fact]
    public async Task RegisterAsync_ServerReturnsConflict_ThrowsConflictExceptionAndSessionEmpty()
    {
        var session = ServiceFactory.Session();
        var sut     = ServiceFactory.Auth(MockHttpFactory.CreateConflict(), session);

        await Assert.ThrowsAsync<ConflictException>(
            () => sut.RegisterAsync(new RegistrationForm { Username = "existing", Password = "p", Name = "N", Email = "e@e.com" }));

        Assert.False(sut.IsAuthenticated());
    }

    [Fact]
    public async Task RegisterAsync_NullForm_ThrowsArgumentNullException()
    {
        var sut = ServiceFactory.Auth(MockHttpFactory.CreateOk(), ServiceFactory.Session());

        await Assert.ThrowsAsync<ArgumentNullException>(() => sut.RegisterAsync(null!));
    }

    // ── Logout ─────────────────────────────────────────────────────────────

    [Fact]
    public void Logout_AfterLogin_SessionCleared()
    {
        var session = ServiceFactory.Session(FakeDto.Token());
        var sut     = ServiceFactory.Auth(MockHttpFactory.CreateOk(), session);

        sut.Logout();

        Assert.False(sut.IsAuthenticated());
        Assert.Null(session.Token);
    }

    // ── IsAuthenticated ────────────────────────────────────────────────────

    [Fact]
    public void IsAuthenticated_WithToken_ReturnsTrue()
    {
        var session = ServiceFactory.Session(FakeDto.Token());
        var sut     = ServiceFactory.Auth(MockHttpFactory.CreateOk(), session);

        Assert.True(sut.IsAuthenticated());
    }

    [Fact]
    public void IsAuthenticated_WithoutToken_ReturnsFalse()
    {
        var session = ServiceFactory.Session();
        var sut     = ServiceFactory.Auth(MockHttpFactory.CreateOk(), session);

        Assert.False(sut.IsAuthenticated());
    }
}
