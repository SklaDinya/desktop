using SklaDinya_desktop_BL_component.Exceptions;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_BL_DA_IntegrationalTests.Helpers;

namespace SklaDinya_desktop_BL_DA_IntegrationalTests;

/// <summary>
/// Интеграционные тесты CellService.
/// Цепочка: CellService → CellRepository → ApiClient(mock HTTP).
/// </summary>
public class CellServiceIntegrationTests
{
    private static readonly string Token = FakeDto.Token();

    // ── GetCellsAsync ──────────────────────────────────────────────────────

    [Fact]
    public async Task GetCellsAsync_ServerReturnsOk_ReturnsMappedCells()
    {
        var id = Guid.NewGuid();
        var session = ServiceFactory.Session(Token);
        var sut = ServiceFactory.Cell(
            MockHttpFactory.CreateOk(new[] { FakeDto.Cell(id) }), session);

        var result = await sut.GetCellsAsync(Guid.NewGuid(), new CellSearchQuery
        {
            StartBooking = DateTime.UtcNow,
            TimeBooking = TimeSpan.FromHours(1),
            PageNumber = 1,
            PageSize = 10
        });

        Assert.Single(result);
        Assert.Equal(id, result[0].Id);
        Assert.Equal("Small", result[0].CellClass);
    }

    [Fact]
    public async Task GetCellsAsync_ServerReturnsNotFound_ThrowsNotFoundException()
    {
        var session = ServiceFactory.Session(Token);
        var sut = ServiceFactory.Cell(MockHttpFactory.CreateNotFound(), session);

        await Assert.ThrowsAsync<NotFoundException>(
            () => sut.GetCellsAsync(Guid.NewGuid(), new CellSearchQuery
            {
                StartBooking = DateTime.UtcNow,
                TimeBooking = TimeSpan.FromHours(1)
            }));
    }

    // ── GetCellClassesAsync ────────────────────────────────────────────────

    [Fact]
    public async Task GetCellClassesAsync_ServerReturnsOk_ReturnsList()
    {
        var session = ServiceFactory.Session(Token);
        var sut = ServiceFactory.Cell(
            MockHttpFactory.CreateOk(new[] { "Small", "Medium", "Large" }), session);

        var result = await sut.GetCellClassesAsync(Guid.NewGuid());

        Assert.Equal(3, result.Count);
        Assert.Contains("Small", result);
    }

    [Fact]
    public async Task GetCellClassesAsync_ServerReturnsNotFound_ThrowsNotFoundException()
    {
        var session = ServiceFactory.Session(Token);
        var sut = ServiceFactory.Cell(MockHttpFactory.CreateNotFound(), session);

        await Assert.ThrowsAsync<NotFoundException>(
            () => sut.GetCellClassesAsync(Guid.NewGuid()));
    }

    // ── GetMyCellsAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task GetMyCellsAsync_ServerReturnsOk_ReturnsMappedCells()
    {
        var id = Guid.NewGuid();
        var session = ServiceFactory.Session(Token);
        var sut = ServiceFactory.Cell(
            MockHttpFactory.CreateOk(new[] { FakeDto.Cell(id) }), session);

        var result = await sut.GetMyCellsAsync(new MyCellSearchQuery { PageNumber = 1, PageSize = 10 });

        Assert.Single(result);
        Assert.Equal(id, result[0].Id);
    }

    [Fact]
    public async Task GetMyCellsAsync_ServerReturnsUnauthorized_ThrowsUnauthorizedException()
    {
        var session = ServiceFactory.Session(Token);
        var sut = ServiceFactory.Cell(MockHttpFactory.CreateUnauthorized(), session);

        await Assert.ThrowsAsync<UnauthorizedException>(
            () => sut.GetMyCellsAsync(new MyCellSearchQuery()));
    }

    // ── GetMyCellClassesAsync ──────────────────────────────────────────────

    [Fact]
    public async Task GetMyCellClassesAsync_ServerReturnsOk_ReturnsList()
    {
        var session = ServiceFactory.Session(Token);
        var sut = ServiceFactory.Cell(
            MockHttpFactory.CreateOk(new[] { "Small" }), session);

        var result = await sut.GetMyCellClassesAsync();

        Assert.Single(result);
        Assert.Equal("Small", result[0]);
    }

    [Fact]
    public async Task GetMyCellClassesAsync_ServerReturnsForbidden_ThrowsForbiddenException()
    {
        var session = ServiceFactory.Session(Token);
        var sut = ServiceFactory.Cell(MockHttpFactory.CreateForbidden(), session);

        await Assert.ThrowsAsync<ForbiddenException>(() => sut.GetMyCellClassesAsync());
    }

    // ── CreateCellAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task CreateCellAsync_ServerReturnsOk_ReturnsMappedCell()
    {
        var id = Guid.NewGuid();
        var session = ServiceFactory.Session(Token);
        var sut = ServiceFactory.Cell(MockHttpFactory.CreateOk(FakeDto.Cell(id)), session);

        var result = await sut.CreateCellAsync(new CellCreateForm { Name = "A1", CellClass = "Small" });

        Assert.Equal(id, result.Id);
        Assert.Equal("Small", result.CellClass);
    }

    [Fact]
    public async Task CreateCellAsync_ServerReturnsConflict_ThrowsConflictException()
    {
        var session = ServiceFactory.Session(Token);
        var sut = ServiceFactory.Cell(MockHttpFactory.CreateConflict(), session);

        await Assert.ThrowsAsync<ConflictException>(
            () => sut.CreateCellAsync(new CellCreateForm { Name = "A1", CellClass = "Small" }));
    }
}
