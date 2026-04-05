using Moq;
using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Interfaces.Services;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_BL_component.Services;
using SklaDinya_desktop_BL_tests.Helpers;

namespace SklaDinya_desktop_BL_tests;

public class OperatorServiceTests
{
    private readonly Mock<IOperatorRepository> _repo    = new();
    private readonly Mock<ISessionService>     _session = new();
    private readonly OperatorService           _sut;

    private const string Token = "test.jwt.token";

    public OperatorServiceTests()
    {
        _session.Setup(s => s.Token).Returns(Token);
        _sut = new OperatorService(_repo.Object, _session.Object);
    }

    // ── GetOperatorsAsync ──────────────────────────────────────────────────

    [Fact]
    public async Task GetOperatorsAsync_ValidQuery_ReturnsOperators()
    {
        var query    = new OperatorSearchQuery { PageNumber = 1, PageSize = 10 };
        var expected = new List<SklaDinya_desktop_BL_component.Models.OperatorModel> { ModelBuilder.Operator() };

        _repo.Setup(r => r.GetOperatorsAsync(query, Token)).ReturnsAsync(expected);

        var result = await _sut.GetOperatorsAsync(query);

        Assert.Equal(expected, result);
        _repo.Verify(r => r.GetOperatorsAsync(query, Token), Times.Once);
    }

    [Fact]
    public async Task GetOperatorsAsync_NullQuery_ThrowsArgumentNullException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.GetOperatorsAsync(null!));

        _repo.Verify(r => r.GetOperatorsAsync(It.IsAny<OperatorSearchQuery>(), It.IsAny<string>()), Times.Never);
    }

    // ── CreateOperatorAsync ────────────────────────────────────────────────

    [Fact]
    public async Task CreateOperatorAsync_ValidForm_ReturnsCreatedOperator()
    {
        var form     = new OperatorCreateForm { Username = "op1", Password = "p", Name = "Op", Email = "op@e.com", Role = OperatorRole.OrdinaryOperator };
        var expected = ModelBuilder.Operator();

        _repo.Setup(r => r.CreateOperatorAsync(form, Token)).ReturnsAsync(expected);

        var result = await _sut.CreateOperatorAsync(form);

        Assert.Equal(expected, result);
        _repo.Verify(r => r.CreateOperatorAsync(form, Token), Times.Once);
    }

    [Fact]
    public async Task CreateOperatorAsync_NullForm_ThrowsArgumentNullException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.CreateOperatorAsync(null!));

        _repo.Verify(r => r.CreateOperatorAsync(It.IsAny<OperatorCreateForm>(), It.IsAny<string>()), Times.Never);
    }

    // ── GetOperatorByIdAsync ───────────────────────────────────────────────

    [Fact]
    public async Task GetOperatorByIdAsync_ValidId_ReturnsOperator()
    {
        var expected = ModelBuilder.Operator();

        _repo.Setup(r => r.GetOperatorByIdAsync(expected.Id, Token)).ReturnsAsync(expected);

        var result = await _sut.GetOperatorByIdAsync(expected.Id);

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetOperatorByIdAsync_RepoThrows_PropagatesException()
    {
        var id = Guid.NewGuid();

        _repo.Setup(r => r.GetOperatorByIdAsync(id, Token))
             .ThrowsAsync(new InvalidOperationException("Not found"));

        await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.GetOperatorByIdAsync(id));
    }

    // ── UpdateOperatorAsync ────────────────────────────────────────────────

    [Fact]
    public async Task UpdateOperatorAsync_ValidArgs_ReturnsUpdatedOperator()
    {
        var id       = Guid.NewGuid();
        var form     = new OperatorUpdateForm { Name = "New Name" };
        var expected = ModelBuilder.Operator(id);

        _repo.Setup(r => r.UpdateOperatorAsync(id, form, Token)).ReturnsAsync(expected);

        var result = await _sut.UpdateOperatorAsync(id, form);

        Assert.Equal(expected, result);
        _repo.Verify(r => r.UpdateOperatorAsync(id, form, Token), Times.Once);
    }

    [Fact]
    public async Task UpdateOperatorAsync_NullForm_ThrowsArgumentNullException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.UpdateOperatorAsync(Guid.NewGuid(), null!));

        _repo.Verify(r => r.UpdateOperatorAsync(It.IsAny<Guid>(), It.IsAny<OperatorUpdateForm>(), It.IsAny<string>()), Times.Never);
    }
}
