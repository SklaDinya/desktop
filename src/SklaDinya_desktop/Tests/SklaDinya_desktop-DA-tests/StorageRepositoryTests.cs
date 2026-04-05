using SklaDinya_desktop_BL_component.Exceptions;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_DA_component.Repositories;
using SklaDinya_desktop_DA_tests.Helpers;

namespace SklaDinya_desktop_DA_tests;

public class StorageRepositoryTests
{
    private const string Token = "valid.jwt.token";

    // ── GetStoragesAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task GetStoragesAsync_ServerReturnsOk_ReturnsMappedList()
    {
        var repo = new StorageRepository(MockHttpFactory.CreateOk(FakeDto.StorageList(1)));
        var query = new StorageSearchQuery { PageNumber = 1, PageSize = 10 };

        var result = await repo.GetStoragesAsync(query);

        Assert.Single(result);
        Assert.Equal("Test Storage", result[0].Name);
    }

    [Fact]
    public async Task GetStoragesAsync_NullQuery_ThrowsArgumentNullException()
    {
        var repo = new StorageRepository(MockHttpFactory.CreateOk());

        await Assert.ThrowsAsync<ArgumentNullException>(() => repo.GetStoragesAsync(null!));
    }

    // ── CreateStorageAsync ─────────────────────────────────────────────────

    [Fact]
    public async Task CreateStorageAsync_ValidForm_CompletesSuccessfully()
    {
        var repo = new StorageRepository(MockHttpFactory.CreateOk());
        var form = new StorageCreateForm { Username = "op", Password = "p", Name = "N", Email = "e@e.com", StorageName = "S", Address = "A", Description = "D" };

        var ex = await Record.ExceptionAsync(() => repo.CreateStorageAsync(form));

        Assert.Null(ex);
    }

    [Fact]
    public async Task CreateStorageAsync_NullForm_ThrowsArgumentNullException()
    {
        var repo = new StorageRepository(MockHttpFactory.CreateOk());

        await Assert.ThrowsAsync<ArgumentNullException>(() => repo.CreateStorageAsync(null!));
    }

    // ── GetStorageByIdAsync ────────────────────────────────────────────────

    [Fact]
    public async Task GetStorageByIdAsync_ServerReturnsOk_ReturnsMappedStorage()
    {
        var id = Guid.NewGuid();
        var repo = new StorageRepository(MockHttpFactory.CreateOk(FakeDto.Storage(id)));

        var result = await repo.GetStorageByIdAsync(id, Token);

        Assert.Equal(id, result.Id);
        Assert.Equal("Test Storage", result.Name);
    }

    [Fact]
    public async Task GetStorageByIdAsync_ServerReturnsNotFound_ThrowsNotFoundException()
    {
        var repo = new StorageRepository(MockHttpFactory.CreateNotFound());

        await Assert.ThrowsAsync<NotFoundException>(() => repo.GetStorageByIdAsync(Guid.NewGuid(), Token));
    }

    [Fact]
    public async Task GetStorageByIdAsync_EmptyToken_ThrowsArgumentException()
    {
        var repo = new StorageRepository(MockHttpFactory.CreateOk());

        await Assert.ThrowsAsync<ArgumentException>(() => repo.GetStorageByIdAsync(Guid.NewGuid(), ""));
    }

    // ── UpdateStorageByIdAsync ─────────────────────────────────────────────

    [Fact]
    public async Task UpdateStorageByIdAsync_ValidArgs_ReturnsMappedStorage()
    {
        var id = Guid.NewGuid();
        var repo = new StorageRepository(MockHttpFactory.CreateOk(FakeDto.Storage(id)));
        var form = new StorageUpdateForm { Name = "Updated", Address = "Addr", Description = "Desc" };

        var result = await repo.UpdateStorageByIdAsync(id, form, Token);

        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task UpdateStorageByIdAsync_NullForm_ThrowsArgumentNullException()
    {
        var repo = new StorageRepository(MockHttpFactory.CreateOk());

        await Assert.ThrowsAsync<ArgumentNullException>(() => repo.UpdateStorageByIdAsync(Guid.NewGuid(), null!, Token));
    }

    [Fact]
    public async Task UpdateStorageByIdAsync_EmptyToken_ThrowsArgumentException()
    {
        var repo = new StorageRepository(MockHttpFactory.CreateOk());
        var form = new StorageUpdateForm { Name = "Updated" };

        await Assert.ThrowsAsync<ArgumentException>(() => repo.UpdateStorageByIdAsync(Guid.NewGuid(), form, ""));
    }

    [Fact]
    public async Task UpdateStorageByIdAsync_ServerReturnsForbidden_ThrowsForbiddenException()
    {
        var repo = new StorageRepository(MockHttpFactory.CreateForbidden());
        var form = new StorageUpdateForm { Name = "Updated" };

        await Assert.ThrowsAsync<ForbiddenException>(() => repo.UpdateStorageByIdAsync(Guid.NewGuid(), form, Token));
    }

    // ── ApproveStorageAsync ────────────────────────────────────────────────

    [Fact]
    public async Task ApproveStorageAsync_ValidArgs_ReturnsMappedStorage()
    {
        var id = Guid.NewGuid();
        var repo = new StorageRepository(MockHttpFactory.CreateOk(FakeDto.Storage(id)));

        var result = await repo.ApproveStorageAsync(id, Token);

        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task ApproveStorageAsync_ServerReturnsForbidden_ThrowsForbiddenException()
    {
        var repo = new StorageRepository(MockHttpFactory.CreateForbidden());

        await Assert.ThrowsAsync<ForbiddenException>(() => repo.ApproveStorageAsync(Guid.NewGuid(), Token));
    }

    [Fact]
    public async Task ApproveStorageAsync_EmptyToken_ThrowsArgumentException()
    {
        var repo = new StorageRepository(MockHttpFactory.CreateOk());

        await Assert.ThrowsAsync<ArgumentException>(() => repo.ApproveStorageAsync(Guid.NewGuid(), "  "));
    }

    // ── RejectStorageAsync ─────────────────────────────────────────────────

    [Fact]
    public async Task RejectStorageAsync_ValidArgs_CompletesSuccessfully()
    {
        var repo = new StorageRepository(MockHttpFactory.CreateOk());

        var ex = await Record.ExceptionAsync(() => repo.RejectStorageAsync(Guid.NewGuid(), Token));

        Assert.Null(ex);
    }

    [Fact]
    public async Task RejectStorageAsync_ServerReturnsForbidden_ThrowsForbiddenException()
    {
        var repo = new StorageRepository(MockHttpFactory.CreateForbidden());

        await Assert.ThrowsAsync<ForbiddenException>(() => repo.RejectStorageAsync(Guid.NewGuid(), Token));
    }

    [Fact]
    public async Task RejectStorageAsync_EmptyToken_ThrowsArgumentException()
    {
        var repo = new StorageRepository(MockHttpFactory.CreateOk());

        await Assert.ThrowsAsync<ArgumentException>(() => repo.RejectStorageAsync(Guid.NewGuid(), ""));
    }

    // ── GetMyStorageAsync ──────────────────────────────────────────────────

    [Fact]
    public async Task GetMyStorageAsync_ValidToken_ReturnsMappedStorage()
    {
        var id = Guid.NewGuid();
        var repo = new StorageRepository(MockHttpFactory.CreateOk(FakeDto.Storage(id)));

        var result = await repo.GetMyStorageAsync(Token);

        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task GetMyStorageAsync_ServerReturnsUnauthorized_ThrowsUnauthorizedException()
    {
        var repo = new StorageRepository(MockHttpFactory.CreateUnauthorized());

        await Assert.ThrowsAsync<UnauthorizedException>(() => repo.GetMyStorageAsync(Token));
    }

    [Fact]
    public async Task GetMyStorageAsync_EmptyToken_ThrowsArgumentException()
    {
        var repo = new StorageRepository(MockHttpFactory.CreateOk());

        await Assert.ThrowsAsync<ArgumentException>(() => repo.GetMyStorageAsync(""));
    }

    // ── UpdateMyStorageAsync ───────────────────────────────────────────────

    [Fact]
    public async Task UpdateMyStorageAsync_ValidArgs_ReturnsMappedStorage()
    {
        var id = Guid.NewGuid();
        var repo = new StorageRepository(MockHttpFactory.CreateOk(FakeDto.Storage(id)));
        var form = new StorageUpdateForm { Name = "New", Address = "A", Description = "D" };

        var result = await repo.UpdateMyStorageAsync(form, Token);

        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task UpdateMyStorageAsync_NullForm_ThrowsArgumentNullException()
    {
        var repo = new StorageRepository(MockHttpFactory.CreateOk());

        await Assert.ThrowsAsync<ArgumentNullException>(() => repo.UpdateMyStorageAsync(null!, Token));
    }

    [Fact]
    public async Task UpdateMyStorageAsync_EmptyToken_ThrowsArgumentException()
    {
        var repo = new StorageRepository(MockHttpFactory.CreateOk());
        var form = new StorageUpdateForm { Name = "New" };

        await Assert.ThrowsAsync<ArgumentException>(() => repo.UpdateMyStorageAsync(form, ""));
    }

    [Fact]
    public async Task UpdateMyStorageAsync_ServerReturnsUnauthorized_ThrowsUnauthorizedException()
    {
        var repo = new StorageRepository(MockHttpFactory.CreateUnauthorized());
        var form = new StorageUpdateForm { Name = "New" };

        await Assert.ThrowsAsync<UnauthorizedException>(() => repo.UpdateMyStorageAsync(form, Token));
    }
}