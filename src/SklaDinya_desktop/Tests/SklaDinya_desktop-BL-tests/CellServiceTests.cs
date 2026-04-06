using Moq;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Interfaces.Services;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_BL_component.Services;
using SklaDinya_desktop_BL_tests.Helpers;

namespace SklaDinya_desktop_BL_tests;

public class CellServiceTests
{
    private readonly Mock<ICellRepository> _repo    = new();
    private readonly Mock<ISessionService> _session = new();
    private readonly ICellService          _sut;

    private const string Token = "test.jwt.token";

    public CellServiceTests()
    {
        _session.Setup(s => s.Token).Returns(Token);
        _sut = new CellService(_repo.Object, _session.Object);
    }

    // ── GetCellsAsync ──────────────────────────────────────────────────────

    [Fact]
    public async Task GetCellsAsync_ValidArgs_ReturnsCells()
    {
        var storageId = Guid.NewGuid();
        var query     = new CellSearchQuery { StartBooking = DateTime.UtcNow, TimeBooking = TimeSpan.FromHours(1), PageNumber = 1, PageSize = 20 };
        var expected  = new List<SklaDinya_desktop_BL_component.Models.CellModel> { ModelBuilder.Cell() };

        _repo.Setup(r => r.GetCellsAsync(storageId, query)).ReturnsAsync(expected);

        var result = await _sut.GetCellsAsync(storageId, query);

        Assert.Equal(expected, result);
    }

    // ── GetCellClassesAsync ────────────────────────────────────────────────

    [Fact]
    public async Task GetCellClassesAsync_ValidStorageId_ReturnsClasses()
    {
        var storageId = Guid.NewGuid();
        var expected  = new List<string> { "Small", "Medium", "Large" };

        _repo.Setup(r => r.GetCellClassesAsync(storageId)).ReturnsAsync(expected);

        var result = await _sut.GetCellClassesAsync(storageId);

        Assert.Equal(expected, result);
    }

    // ── GetMyCellsAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task GetMyCellsAsync_ValidQuery_ReturnsCells()
    {
        var query    = new MyCellSearchQuery { PageNumber = 1, PageSize = 10 };
        var expected = new List<SklaDinya_desktop_BL_component.Models.CellModel> { ModelBuilder.Cell() };

        _repo.Setup(r => r.GetMyCellsAsync(query, Token)).ReturnsAsync(expected);

        var result = await _sut.GetMyCellsAsync(query);

        Assert.Equal(expected, result);
        _repo.Verify(r => r.GetMyCellsAsync(query, Token), Times.Once);
    }

    // ── GetMyCellClassesAsync ──────────────────────────────────────────────

    [Fact]
    public async Task GetMyCellClassesAsync_ValidToken_ReturnsClasses()
    {
        var expected = new List<string> { "Small", "Large" };

        _repo.Setup(r => r.GetMyCellClassesAsync(Token)).ReturnsAsync(expected);

        var result = await _sut.GetMyCellClassesAsync();

        Assert.Equal(expected, result);
        _repo.Verify(r => r.GetMyCellClassesAsync(Token), Times.Once);
    }

    // ── CreateCellAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task CreateCellAsync_ValidForm_ReturnsCreatedCell()
    {
        var form     = new CellCreateForm { Name = "A1", CellClass = "Small" };
        var expected = ModelBuilder.Cell();

        _repo.Setup(r => r.CreateCellAsync(form, Token)).ReturnsAsync(expected);

        var result = await _sut.CreateCellAsync(form);

        Assert.Equal(expected, result);
        _repo.Verify(r => r.CreateCellAsync(form, Token), Times.Once);
    }
}
