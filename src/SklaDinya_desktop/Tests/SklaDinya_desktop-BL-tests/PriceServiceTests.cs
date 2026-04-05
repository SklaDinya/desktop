using Moq;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Interfaces.Services;
using SklaDinya_desktop_BL_component.Services;
using SklaDinya_desktop_BL_tests.Helpers;

namespace SklaDinya_desktop_BL_tests;

public class PriceServiceTests
{
    private readonly Mock<IPriceRepository> _repo    = new();
    private readonly Mock<ISessionService>  _session = new();
    private readonly PriceService           _sut;

    private const string Token = "test.jwt.token";

    public PriceServiceTests()
    {
        _session.Setup(s => s.Token).Returns(Token);
        _sut = new PriceService(_repo.Object, _session.Object);
    }

    // ── GetPricesAsync ─────────────────────────────────────────────────────

    [Fact]
    public async Task GetPricesAsync_ValidStorageId_ReturnsPrices()
    {
        var storageId = Guid.NewGuid();
        var expected  = new List<SklaDinya_desktop_BL_component.Models.PriceModel> { ModelBuilder.Price(storageId) };

        _repo.Setup(r => r.GetPricesAsync(storageId)).ReturnsAsync(expected);

        var result = await _sut.GetPricesAsync(storageId);

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetPricesAsync_RepoThrows_PropagatesException()
    {
        _repo.Setup(r => r.GetPricesAsync(It.IsAny<Guid>()))
             .ThrowsAsync(new InvalidOperationException("Not found"));

        await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.GetPricesAsync(Guid.NewGuid()));
    }

    // ── GetMyPricesAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task GetMyPricesAsync_ValidToken_ReturnsPrices()
    {
        var expected = new List<SklaDinya_desktop_BL_component.Models.PriceModel> { ModelBuilder.Price() };

        _repo.Setup(r => r.GetMyPricesAsync(Token)).ReturnsAsync(expected);

        var result = await _sut.GetMyPricesAsync();

        Assert.Equal(expected, result);
        _repo.Verify(r => r.GetMyPricesAsync(Token), Times.Once);
    }

    [Fact]
    public async Task GetMyPricesAsync_RepoThrows_PropagatesException()
    {
        _repo.Setup(r => r.GetMyPricesAsync(Token))
             .ThrowsAsync(new UnauthorizedAccessException());

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _sut.GetMyPricesAsync());
    }

    // ── CreatePriceAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task CreatePriceAsync_ValidForm_CallsRepo()
    {
        var form = new PriceCreateForm { CellClass = "Medium", Price = 150m };

        _repo.Setup(r => r.CreatePriceAsync(form, Token)).Returns(Task.CompletedTask);

        await _sut.CreatePriceAsync(form);

        _repo.Verify(r => r.CreatePriceAsync(form, Token), Times.Once);
    }

    [Fact]
    public async Task CreatePriceAsync_NullForm_ThrowsArgumentNullException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.CreatePriceAsync(null!));

        _repo.Verify(r => r.CreatePriceAsync(It.IsAny<PriceCreateForm>(), It.IsAny<string>()), Times.Never);
    }
}
