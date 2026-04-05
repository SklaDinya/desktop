using Moq;
using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Interfaces.Services;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_BL_component.Services;
using SklaDinya_desktop_BL_tests.Helpers;

namespace SklaDinya_desktop_BL_tests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _repo    = new();
    private readonly Mock<ISessionService> _session = new();
    private readonly UserService           _sut;

    private const string Token    = "test.jwt.token";
    private const string NewToken = "new.jwt.token";

    public UserServiceTests()
    {
        _session.Setup(s => s.Token).Returns(Token);
        _sut = new UserService(_repo.Object, _session.Object);
    }

    // ── GetUsersAsync ──────────────────────────────────────────────────────

    [Fact]
    public async Task GetUsersAsync_ValidQuery_ReturnsUsers()
    {
        var query    = new UserSearchQuery { PageNumber = 1, PageSize = 10 };
        var expected = new List<SklaDinya_desktop_BL_component.Models.UserModel> { ModelBuilder.User() };

        _repo.Setup(r => r.GetUsersAsync(query, Token)).ReturnsAsync(expected);

        var result = await _sut.GetUsersAsync(query);

        Assert.Equal(expected, result);
        _repo.Verify(r => r.GetUsersAsync(query, Token), Times.Once);
    }

    [Fact]
    public async Task GetUsersAsync_NullQuery_ThrowsArgumentNullException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.GetUsersAsync(null!));

        _repo.Verify(r => r.GetUsersAsync(It.IsAny<UserSearchQuery>(), It.IsAny<string>()), Times.Never);
    }

    // ── CreateUserAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task CreateUserAsync_ValidForm_ReturnsCreatedUser()
    {
        var form     = new UserCreateForm { Username = "u", Password = "p", Name = "N", Email = "e@e.com", Role = UserRole.Client };
        var expected = ModelBuilder.User();

        _repo.Setup(r => r.CreateUserAsync(form, Token)).ReturnsAsync(expected);

        var result = await _sut.CreateUserAsync(form);

        Assert.Equal(expected, result);
        _repo.Verify(r => r.CreateUserAsync(form, Token), Times.Once);
    }

    [Fact]
    public async Task CreateUserAsync_NullForm_ThrowsArgumentNullException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.CreateUserAsync(null!));

        _repo.Verify(r => r.CreateUserAsync(It.IsAny<UserCreateForm>(), It.IsAny<string>()), Times.Never);
    }

    // ── GetUserByIdAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task GetUserByIdAsync_ValidId_ReturnsUser()
    {
        var expected = ModelBuilder.User();

        _repo.Setup(r => r.GetUserByIdAsync(expected.Id, Token)).ReturnsAsync(expected);

        var result = await _sut.GetUserByIdAsync(expected.Id);

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetUserByIdAsync_RepoThrows_PropagatesException()
    {
        _repo.Setup(r => r.GetUserByIdAsync(It.IsAny<Guid>(), Token))
             .ThrowsAsync(new InvalidOperationException("Not found"));

        await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.GetUserByIdAsync(Guid.NewGuid()));
    }

    // ── UpdateUserAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateUserAsync_ValidArgs_ReturnsUpdatedUser()
    {
        var id       = Guid.NewGuid();
        var form     = new UserUpdateForm { Name = "New Name" };
        var expected = ModelBuilder.User(id);

        _repo.Setup(r => r.UpdateUserAsync(id, form, Token)).ReturnsAsync(expected);

        var result = await _sut.UpdateUserAsync(id, form);

        Assert.Equal(expected, result);
        _repo.Verify(r => r.UpdateUserAsync(id, form, Token), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_NullForm_ThrowsArgumentNullException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.UpdateUserAsync(Guid.NewGuid(), null!));

        _repo.Verify(r => r.UpdateUserAsync(It.IsAny<Guid>(), It.IsAny<UserUpdateForm>(), It.IsAny<string>()), Times.Never);
    }

    // ── GetMeAsync ─────────────────────────────────────────────────────────

    [Fact]
    public async Task GetMeAsync_ValidToken_ReturnsMe()
    {
        var expected = ModelBuilder.Me();

        _repo.Setup(r => r.GetMeAsync(Token)).ReturnsAsync(expected);

        var result = await _sut.GetMeAsync();

        Assert.Equal(expected, result);
        _repo.Verify(r => r.GetMeAsync(Token), Times.Once);
    }

    [Fact]
    public async Task GetMeAsync_RepoThrows_PropagatesException()
    {
        _repo.Setup(r => r.GetMeAsync(Token))
             .ThrowsAsync(new UnauthorizedAccessException());

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _sut.GetMeAsync());
    }

    // ── UpdateMeAsync ──────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateMeAsync_ValidForm_SetsNewTokenInSession()
    {
        var form = new MeUpdateForm { Username = "new", OldPassword = "old", NewPassword = "new123", Name = "Name", Email = "e@e.com" };

        _repo.Setup(r => r.UpdateMeAsync(form, Token)).ReturnsAsync(NewToken);

        await _sut.UpdateMeAsync(form);

        _session.Verify(s => s.SetToken(NewToken), Times.Once);
        _repo.Verify(r => r.UpdateMeAsync(form, Token), Times.Once);
    }

    [Fact]
    public async Task UpdateMeAsync_NullForm_ThrowsArgumentNullException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.UpdateMeAsync(null!));

        _repo.Verify(r => r.UpdateMeAsync(It.IsAny<MeUpdateForm>(), It.IsAny<string>()), Times.Never);
        _session.Verify(s => s.SetToken(It.IsAny<string>()),                              Times.Never);
    }
}
