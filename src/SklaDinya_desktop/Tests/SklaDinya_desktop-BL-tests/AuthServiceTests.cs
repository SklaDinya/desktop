using Moq;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Interfaces.Services;
using SklaDinya_desktop_BL_component.Services;

namespace SklaDinya_desktop_BL_tests;

public class AuthServiceTests
{
    private readonly Mock<IAuthRepository> _authRepo = new();
    private readonly Mock<ISessionService> _session  = new();
    private readonly IAuthService          _sut;

    public AuthServiceTests()
        => _sut = new AuthService(_authRepo.Object, _session.Object);

    // ── LoginAsync ─────────────────────────────────────────────────────────

    [Fact]
    public async Task LoginAsync_ValidForm_CallsRepoAndSetsToken()
    {
        var form  = new LoginForm { Username = "user", Password = "pass" };
        var token = "jwt.token.value";

        _authRepo.Setup(r => r.LoginAsync(form)).ReturnsAsync(token);

        await _sut.LoginAsync(form);

        _authRepo.Verify(r => r.LoginAsync(form), Times.Once);
        _session.Verify(s => s.SetToken(token),   Times.Once);
    }

    // ── RegisterAsync ──────────────────────────────────────────────────────

    [Fact]
    public async Task RegisterAsync_ValidForm_CallsRepoAndSetsToken()
    {
        var form  = new RegistrationForm { Username = "user", Password = "pass", Name = "Name", Email = "e@e.com" };
        var token = "jwt.register.token";

        _authRepo.Setup(r => r.RegisterAsync(form)).ReturnsAsync(token);

        await _sut.RegisterAsync(form);

        _authRepo.Verify(r => r.RegisterAsync(form), Times.Once);
        _session.Verify(s => s.SetToken(token),      Times.Once);
    }

    // ── Logout ─────────────────────────────────────────────────────────────

    [Fact]
    public void Logout_Always_ClearsSession()
    {
        _sut.Logout();

        _session.Verify(s => s.Clear(), Times.Once);
    }
}
