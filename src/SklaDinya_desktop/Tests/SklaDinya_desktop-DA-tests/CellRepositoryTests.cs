using SklaDinya_desktop_BL_component.Exceptions;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_DA_component.Repositories;
using SklaDinya_desktop_DA_tests.Helpers;

namespace SklaDinya_desktop_DA_tests;

public class CellRepositoryTests
{
    private const string Token = "valid.jwt.token";

    // ── GetCellsAsync ──────────────────────────────────────────────────────

    [Fact]
    public async Task GetCellsAsync_ServerReturnsOk_ReturnsMappedList()
    {
        var id    = Guid.NewGuid();
        var repo  = new CellRepository(MockHttpFactory.CreateOk(new[] { FakeDto.Cell(id) }));
        var query = new CellSearchQuery { StartBooking = DateTime.UtcNow, TimeBooking = TimeSpan.FromHours(1), PageNumber = 1, PageSize = 10 };

        var result = await repo.GetCellsAsync(Guid.NewGuid(), query);

        Assert.Single(result);
        Assert.Equal(id,      result[0].Id);
        Assert.Equal("Small", result[0].CellClass);
    }

    [Fact]
    public async Task GetCellsAsync_NullQuery_ThrowsArgumentNullException()
    {
        var repo = new CellRepository(MockHttpFactory.CreateOk());

        await Assert.ThrowsAsync<ArgumentNullException>(() => repo.GetCellsAsync(Guid.NewGuid(), null!));
    }

    // ── GetCellClassesAsync ────────────────────────────────────────────────

    [Fact]
    public async Task GetCellClassesAsync_ServerReturnsOk_ReturnsList()
    {
        var repo = new CellRepository(MockHttpFactory.CreateOk(new[] { "Small", "Medium" }));

        var result = await repo.GetCellClassesAsync(Guid.NewGuid());

        Assert.Equal(2, result.Count);
        Assert.Contains("Small", result);
    }

    [Fact]
    public async Task GetCellClassesAsync_ServerReturnsNotFound_ThrowsNotFoundException()
    {
        var repo = new CellRepository(MockHttpFactory.CreateNotFound());

        await Assert.ThrowsAsync<NotFoundException>(() => repo.GetCellClassesAsync(Guid.NewGuid()));
    }

    // ── GetMyCellsAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task GetMyCellsAsync_ValidArgs_ReturnsMappedList()
    {
        var id    = Guid.NewGuid();
        var repo  = new CellRepository(MockHttpFactory.CreateOk(new[] { FakeDto.Cell(id) }));
        var query = new MyCellSearchQuery { PageNumber = 1, PageSize = 10 };

        var result = await repo.GetMyCellsAsync(query, Token);

        Assert.Single(result);
        Assert.Equal(id, result[0].Id);
    }

    [Fact]
    public async Task GetMyCellsAsync_NullQuery_ThrowsArgumentNullException()
    {
        var repo = new CellRepository(MockHttpFactory.CreateOk());

        await Assert.ThrowsAsync<ArgumentNullException>(() => repo.GetMyCellsAsync(null!, Token));
    }

    [Fact]
    public async Task GetMyCellsAsync_EmptyToken_ThrowsArgumentException()
    {
        var repo  = new CellRepository(MockHttpFactory.CreateOk());
        var query = new MyCellSearchQuery { PageNumber = 1, PageSize = 10 };

        await Assert.ThrowsAsync<ArgumentException>(() => repo.GetMyCellsAsync(query, ""));
    }

    // ── GetMyCellClassesAsync ──────────────────────────────────────────────

    [Fact]
    public async Task GetMyCellClassesAsync_ValidToken_ReturnsList()
    {
        var repo = new CellRepository(MockHttpFactory.CreateOk(new[] { "Small" }));

        var result = await repo.GetMyCellClassesAsync(Token);

        Assert.Single(result);
        Assert.Equal("Small", result[0]);
    }

    [Fact]
    public async Task GetMyCellClassesAsync_EmptyToken_ThrowsArgumentException()
    {
        var repo = new CellRepository(MockHttpFactory.CreateOk());

        await Assert.ThrowsAsync<ArgumentException>(() => repo.GetMyCellClassesAsync(""));
    }

    // ── CreateCellAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task CreateCellAsync_ValidArgs_ReturnsMappedCell()
    {
        var id   = Guid.NewGuid();
        var repo = new CellRepository(MockHttpFactory.CreateOk(FakeDto.Cell(id)));
        var form = new CellCreateForm { Name = "A1", CellClass = "Small" };

        var result = await repo.CreateCellAsync(form, Token);

        Assert.Equal(id,      result.Id);
        Assert.Equal("Small", result.CellClass);
    }

    [Fact]
    public async Task CreateCellAsync_NullForm_ThrowsArgumentNullException()
    {
        var repo = new CellRepository(MockHttpFactory.CreateOk());

        await Assert.ThrowsAsync<ArgumentNullException>(() => repo.CreateCellAsync(null!, Token));
    }

    [Fact]
    public async Task CreateCellAsync_EmptyToken_ThrowsArgumentException()
    {
        var repo = new CellRepository(MockHttpFactory.CreateOk());
        var form = new CellCreateForm { Name = "A1", CellClass = "Small" };

        await Assert.ThrowsAsync<ArgumentException>(() => repo.CreateCellAsync(form, ""));
    }
}
