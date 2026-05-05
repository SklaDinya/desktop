using SklaDinya_desktop_BL_component.Exceptions;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_DA_IntegrationalTests.Helpers;

namespace SklaDinya_desktop_BL_DA_IntegrationalTests;

/// <summary>
/// Интеграционные тесты PriceService.
/// Цепочка: PriceService → PriceRepository → ApiClient(mock HTTP).
/// </summary>
public class PriceServiceIntegrationTests
{
    private static readonly string Token = FakeDto.Token();

    // ── GetPricesAsync ─────────────────────────────────────────────────────

    [Fact]
    public async Task GetPricesAsync_ServerReturnsOk_ReturnsMappedPrices()
    {
        var storageId = Guid.NewGuid();
        var session = ServiceFactory.Session(Token);
        var sut = ServiceFactory.Price(
            MockHttpFactory.CreateOk(new[] { FakeDto.Price(storageId) }), session);

        var result = await sut.GetPricesAsync(storageId);

        Assert.Single(result);
        Assert.Equal("Small", result[0].CellClass);
        Assert.Equal(99.99m, result[0].Price);
    }

    [Fact]
    public async Task GetPricesAsync_ServerReturnsNotFound_ThrowsNotFoundException()
    {
        var session = ServiceFactory.Session(Token);
        var sut = ServiceFactory.Price(MockHttpFactory.CreateNotFound(), session);

        await Assert.ThrowsAsync<NotFoundException>(
            () => sut.GetPricesAsync(Guid.NewGuid()));
    }

    // ── GetMyPricesAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task GetMyPricesAsync_ServerReturnsOk_ReturnsMappedPrices()
    {
        var session = ServiceFactory.Session(Token);
        var sut = ServiceFactory.Price(
            MockHttpFactory.CreateOk(FakeDto.PriceList(2)), session);

        var result = await sut.GetMyPricesAsync();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetMyPricesAsync_ServerReturnsUnauthorized_ThrowsUnauthorizedException()
    {
        var session = ServiceFactory.Session(Token);
        var sut = ServiceFactory.Price(MockHttpFactory.CreateUnauthorized(), session);

        await Assert.ThrowsAsync<UnauthorizedException>(() => sut.GetMyPricesAsync());
    }

    // ── CreatePriceAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task CreatePriceAsync_ServerReturnsOk_CompletesSuccessfully()
    {
        var session = ServiceFactory.Session(Token);
        var sut = ServiceFactory.Price(MockHttpFactory.CreateOk(), session);

        var ex = await Record.ExceptionAsync(
            () => sut.CreatePriceAsync(new PriceCreateForm { CellClass = "Large", Price = 200m }));

        Assert.Null(ex);
    }

    [Fact]
    public async Task CreatePriceAsync_ServerReturnsForbidden_ThrowsForbiddenException()
    {
        var session = ServiceFactory.Session(Token);
        var sut = ServiceFactory.Price(MockHttpFactory.CreateForbidden(), session);

        await Assert.ThrowsAsync<ForbiddenException>(
            () => sut.CreatePriceAsync(new PriceCreateForm { CellClass = "Large", Price = 200m }));
    }
}
