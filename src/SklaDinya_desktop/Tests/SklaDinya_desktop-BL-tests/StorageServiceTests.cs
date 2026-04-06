using Moq;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Interfaces.Services;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_BL_component.Services;
using SklaDinya_desktop_BL_tests.Helpers;

namespace SklaDinya_desktop_BL_tests;

public class StorageServiceTests
{
    private readonly Mock<IStorageRepository> _repo    = new();
    private readonly Mock<ISessionService>    _session = new();
    private readonly IStorageService          _sut;

    private const string Token = "test.jwt.token";

    public StorageServiceTests()
    {
        _session.Setup(s => s.Token).Returns(Token);
        _sut = new StorageService(_repo.Object, _session.Object);
    }

    // ── GetStoragesAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task GetStoragesAsync_ValidQuery_ReturnsStorages()
    {
        var query    = new StorageSearchQuery { PageNumber = 1, PageSize = 10 };
        var expected = new List<SklaDinya_desktop_BL_component.Models.StorageModel> { ModelBuilder.Storage() };

        _repo.Setup(r => r.GetStoragesAsync(query)).ReturnsAsync(expected);

        var result = await _sut.GetStoragesAsync(query);

        Assert.Equal(expected, result);
    }

    // ── CreateStorageAsync ─────────────────────────────────────────────────

    [Fact]
    public async Task CreateStorageAsync_ValidForm_CallsRepo()
    {
        var form = new StorageCreateForm { Username = "op", Password = "p", Name = "N", Email = "e@e.com", StorageName = "S", Address = "A", Description = "D" };

        _repo.Setup(r => r.CreateStorageAsync(form)).Returns(Task.CompletedTask);

        await _sut.CreateStorageAsync(form);

        _repo.Verify(r => r.CreateStorageAsync(form), Times.Once);
    }

    // ── GetStorageByIdAsync ────────────────────────────────────────────────

    [Fact]
    public async Task GetStorageByIdAsync_ValidId_ReturnsStorage()
    {
        var expected = ModelBuilder.Storage();

        _repo.Setup(r => r.GetStorageByIdAsync(expected.Id, Token)).ReturnsAsync(expected);

        var result = await _sut.GetStorageByIdAsync(expected.Id);

        Assert.Equal(expected, result);
    }

    // ── UpdateStorageByIdAsync ─────────────────────────────────────────────

    [Fact]
    public async Task UpdateStorageByIdAsync_ValidArgs_ReturnsUpdatedStorage()
    {
        var id       = Guid.NewGuid();
        var form     = new StorageUpdateForm { Name = "New" };
        var expected = ModelBuilder.Storage(id);

        _repo.Setup(r => r.UpdateStorageByIdAsync(id, form, Token)).ReturnsAsync(expected);

        var result = await _sut.UpdateStorageByIdAsync(id, form);

        Assert.Equal(expected, result);
        _repo.Verify(r => r.UpdateStorageByIdAsync(id, form, Token), Times.Once);
    }

    // ── ApproveStorageAsync ────────────────────────────────────────────────

    [Fact]
    public async Task ApproveStorageAsync_ValidId_ReturnsApprovedStorage()
    {
        var expected = ModelBuilder.Storage();

        _repo.Setup(r => r.ApproveStorageAsync(expected.Id, Token)).ReturnsAsync(expected);

        var result = await _sut.ApproveStorageAsync(expected.Id);

        Assert.Equal(expected, result);
    }

    // ── RejectStorageAsync ─────────────────────────────────────────────────

    [Fact]
    public async Task RejectStorageAsync_ValidId_CallsRepo()
    {
        var id = Guid.NewGuid();

        _repo.Setup(r => r.RejectStorageAsync(id, Token)).Returns(Task.CompletedTask);

        await _sut.RejectStorageAsync(id);

        _repo.Verify(r => r.RejectStorageAsync(id, Token), Times.Once);
    }

    // ── GetMyStorageAsync ──────────────────────────────────────────────────

    [Fact]
    public async Task GetMyStorageAsync_ValidToken_ReturnsStorage()
    {
        var expected = ModelBuilder.Storage();

        _repo.Setup(r => r.GetMyStorageAsync(Token)).ReturnsAsync(expected);

        var result = await _sut.GetMyStorageAsync();

        Assert.Equal(expected, result);
    }

    // ── UpdateMyStorageAsync ───────────────────────────────────────────────

    [Fact]
    public async Task UpdateMyStorageAsync_ValidForm_ReturnsUpdatedStorage()
    {
        var form     = new StorageUpdateForm { Name = "Updated" };
        var expected = ModelBuilder.Storage();

        _repo.Setup(r => r.UpdateMyStorageAsync(form, Token)).ReturnsAsync(expected);

        var result = await _sut.UpdateMyStorageAsync(form);

        Assert.Equal(expected, result);
        _repo.Verify(r => r.UpdateMyStorageAsync(form, Token), Times.Once);
    }
}
