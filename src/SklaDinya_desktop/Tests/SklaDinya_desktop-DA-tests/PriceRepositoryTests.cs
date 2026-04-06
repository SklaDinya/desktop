using SklaDinya_desktop_BL_component.Exceptions;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_DA_component.Repositories;
using SklaDinya_desktop_DA_tests.Helpers;

namespace SklaDinya_desktop_DA_tests;

public class PriceRepositoryTests
{
    private const string Token = "valid.jwt.token";

    // ── GetPricesAsync ─────────────────────────────────────────────────────

    [Fact]
    public async Task GetPricesAsync_ServerReturnsOk_ReturnsMappedList()
    {
        var repo = new PriceRepository(MockHttpFactory.CreateOk(FakeDto.PriceList(1)));

        var result = await repo.GetPricesAsync(Guid.NewGuid());

        Assert.Single(result);
        Assert.Equal("Small", result[0].CellClass);
        Assert.Equal(99.99m,  result[0].Price);
    }

    [Fact]
    public async Task GetPricesAsync_ServerReturnsNotFound_ThrowsNotFoundException()
    {
        var repo = new PriceRepository(MockHttpFactory.CreateNotFound());

        await Assert.ThrowsAsync<NotFoundException>(() => repo.GetPricesAsync(Guid.NewGuid()));
    }

    // ── GetMyPricesAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task GetMyPricesAsync_ValidToken_ReturnsMappedList()
    {
        var repo = new PriceRepository(MockHttpFactory.CreateOk(FakeDto.PriceList(2)));

        var result = await repo.GetMyPricesAsync(Token);

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetMyPricesAsync_ServerReturnsUnauthorized_ThrowsUnauthorizedException()
    {
        var repo = new PriceRepository(MockHttpFactory.CreateUnauthorized());

        await Assert.ThrowsAsync<UnauthorizedException>(() => repo.GetMyPricesAsync(Token));
    }

    [Fact]
    public async Task GetMyPricesAsync_EmptyToken_ThrowsArgumentException()
    {
        var repo = new PriceRepository(MockHttpFactory.CreateOk());

        await Assert.ThrowsAsync<ArgumentException>(() => repo.GetMyPricesAsync(""));
    }

    // ── CreatePriceAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task CreatePriceAsync_ValidArgs_CompletesSuccessfully()
    {
        var repo = new PriceRepository(MockHttpFactory.CreateOk());
        var form = new PriceCreateForm { CellClass = "Large", Price = 200m };

        var ex = await Record.ExceptionAsync(() => repo.CreatePriceAsync(form, Token));

        Assert.Null(ex);
    }

    [Fact]
    public async Task CreatePriceAsync_EmptyToken_ThrowsArgumentException()
    {
        var repo = new PriceRepository(MockHttpFactory.CreateOk());
        var form = new PriceCreateForm { CellClass = "Small", Price = 100m };

        await Assert.ThrowsAsync<ArgumentException>(() => repo.CreatePriceAsync(form, ""));
    }
}
