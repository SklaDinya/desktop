using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Exceptions;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_BL_DA_IntegrationalTests.Helpers;

namespace SklaDinya_desktop_BL_DA_IntegrationalTests;

/// <summary>
/// Интеграционные тесты StorageService.
/// Цепочка: StorageService → StorageRepository → ApiClient(mock HTTP).
/// </summary>
public class StorageServiceIntegrationTests
{
    private static readonly string Token = FakeDto.Token();

    // ── GetStoragesAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task GetStoragesAsync_ServerReturnsOk_ReturnsMappedStorages()
    {
        var id      = Guid.NewGuid();
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Storage(
            MockHttpFactory.CreateOk(new[] { FakeDto.Storage(id) }), session);

        var result = await sut.GetStoragesAsync(new StorageSearchQuery { PageNumber = 1, PageSize = 10 });

        Assert.Single(result);
        Assert.Equal(id,             result[0].Id);
        Assert.Equal("Test Storage", result[0].Name);
        Assert.Equal(StorageStatus.Active, result[0].Status);
    }

    [Fact]
    public async Task GetStoragesAsync_ServerReturnsServerError_ThrowsServerException()
    {
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Storage(
            MockHttpFactory.Create(System.Net.HttpStatusCode.InternalServerError), session);

        await Assert.ThrowsAsync<ServerException>(
            () => sut.GetStoragesAsync(new StorageSearchQuery()));
    }

    [Fact]
    public async Task GetStoragesAsync_NullQuery_ThrowsArgumentNullException()
    {
        var sut = ServiceFactory.Storage(MockHttpFactory.CreateOk(), ServiceFactory.Session(Token));

        await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetStoragesAsync(null!));
    }

    // ── CreateStorageAsync ─────────────────────────────────────────────────

    [Fact]
    public async Task CreateStorageAsync_ServerReturnsOk_CompletesSuccessfully()
    {
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Storage(MockHttpFactory.CreateOk(), session);

        var ex = await Record.ExceptionAsync(() => sut.CreateStorageAsync(new StorageCreateForm
        {
            Username    = "op", Password = "p", Name = "N",
            Email       = "e@e.com", StorageName = "S",
            Address     = "A", Description = "D"
        }));

        Assert.Null(ex);
    }

    [Fact]
    public async Task CreateStorageAsync_ServerReturnsConflict_ThrowsConflictException()
    {
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Storage(MockHttpFactory.CreateConflict(), session);

        await Assert.ThrowsAsync<ConflictException>(() => sut.CreateStorageAsync(new StorageCreateForm
        {
            Username = "op", Password = "p", Name = "N",
            Email    = "e@e.com", StorageName = "S", Address = "A"
        }));
    }

    [Fact]
    public async Task CreateStorageAsync_NullForm_ThrowsArgumentNullException()
    {
        var sut = ServiceFactory.Storage(MockHttpFactory.CreateOk(), ServiceFactory.Session(Token));

        await Assert.ThrowsAsync<ArgumentNullException>(() => sut.CreateStorageAsync(null!));
    }

    // ── GetStorageByIdAsync ────────────────────────────────────────────────

    [Fact]
    public async Task GetStorageByIdAsync_ServerReturnsOk_ReturnsMappedStorage()
    {
        var id      = Guid.NewGuid();
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Storage(MockHttpFactory.CreateOk(FakeDto.Storage(id)), session);

        var result = await sut.GetStorageByIdAsync(id);

        Assert.Equal(id,             result.Id);
        Assert.Equal("Test Storage", result.Name);
    }

    [Fact]
    public async Task GetStorageByIdAsync_ServerReturnsNotFound_ThrowsNotFoundException()
    {
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Storage(MockHttpFactory.CreateNotFound(), session);

        await Assert.ThrowsAsync<NotFoundException>(
            () => sut.GetStorageByIdAsync(Guid.NewGuid()));
    }

    // ── UpdateStorageByIdAsync ─────────────────────────────────────────────

    [Fact]
    public async Task UpdateStorageByIdAsync_ServerReturnsOk_ReturnsMappedStorage()
    {
        var id      = Guid.NewGuid();
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Storage(MockHttpFactory.CreateOk(FakeDto.Storage(id)), session);

        var result = await sut.UpdateStorageByIdAsync(id, new StorageUpdateForm { Name = "Updated" });

        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task UpdateStorageByIdAsync_ServerReturnsForbidden_ThrowsForbiddenException()
    {
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Storage(MockHttpFactory.CreateForbidden(), session);

        await Assert.ThrowsAsync<ForbiddenException>(
            () => sut.UpdateStorageByIdAsync(Guid.NewGuid(), new StorageUpdateForm { Name = "U" }));
    }

    [Fact]
    public async Task UpdateStorageByIdAsync_NullForm_ThrowsArgumentNullException()
    {
        var sut = ServiceFactory.Storage(MockHttpFactory.CreateOk(), ServiceFactory.Session(Token));

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => sut.UpdateStorageByIdAsync(Guid.NewGuid(), null!));
    }

    // ── ApproveStorageAsync ────────────────────────────────────────────────

    [Fact]
    public async Task ApproveStorageAsync_ServerReturnsOk_ReturnsMappedStorage()
    {
        var id      = Guid.NewGuid();
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Storage(MockHttpFactory.CreateOk(FakeDto.Storage(id)), session);

        var result = await sut.ApproveStorageAsync(id);

        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task ApproveStorageAsync_ServerReturnsForbidden_ThrowsForbiddenException()
    {
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Storage(MockHttpFactory.CreateForbidden(), session);

        await Assert.ThrowsAsync<ForbiddenException>(
            () => sut.ApproveStorageAsync(Guid.NewGuid()));
    }

    // ── RejectStorageAsync ─────────────────────────────────────────────────

    [Fact]
    public async Task RejectStorageAsync_ServerReturnsOk_CompletesSuccessfully()
    {
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Storage(MockHttpFactory.CreateOk(), session);

        var ex = await Record.ExceptionAsync(() => sut.RejectStorageAsync(Guid.NewGuid()));

        Assert.Null(ex);
    }

    [Fact]
    public async Task RejectStorageAsync_ServerReturnsForbidden_ThrowsForbiddenException()
    {
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Storage(MockHttpFactory.CreateForbidden(), session);

        await Assert.ThrowsAsync<ForbiddenException>(
            () => sut.RejectStorageAsync(Guid.NewGuid()));
    }

    // ── GetMyStorageAsync ──────────────────────────────────────────────────

    [Fact]
    public async Task GetMyStorageAsync_ServerReturnsOk_ReturnsMappedStorage()
    {
        var id      = Guid.NewGuid();
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Storage(MockHttpFactory.CreateOk(FakeDto.Storage(id)), session);

        var result = await sut.GetMyStorageAsync();

        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task GetMyStorageAsync_ServerReturnsUnauthorized_ThrowsUnauthorizedException()
    {
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Storage(MockHttpFactory.CreateUnauthorized(), session);

        await Assert.ThrowsAsync<UnauthorizedException>(() => sut.GetMyStorageAsync());
    }

    // ── UpdateMyStorageAsync ───────────────────────────────────────────────

    [Fact]
    public async Task UpdateMyStorageAsync_ServerReturnsOk_ReturnsMappedStorage()
    {
        var id      = Guid.NewGuid();
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Storage(MockHttpFactory.CreateOk(FakeDto.Storage(id)), session);

        var result = await sut.UpdateMyStorageAsync(new StorageUpdateForm { Name = "Updated" });

        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task UpdateMyStorageAsync_ServerReturnsUnauthorized_ThrowsUnauthorizedException()
    {
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Storage(MockHttpFactory.CreateUnauthorized(), session);

        await Assert.ThrowsAsync<UnauthorizedException>(
            () => sut.UpdateMyStorageAsync(new StorageUpdateForm { Name = "Updated" }));
    }

    [Fact]
    public async Task UpdateMyStorageAsync_NullForm_ThrowsArgumentNullException()
    {
        var sut = ServiceFactory.Storage(MockHttpFactory.CreateOk(), ServiceFactory.Session(Token));

        await Assert.ThrowsAsync<ArgumentNullException>(() => sut.UpdateMyStorageAsync(null!));
    }
}
