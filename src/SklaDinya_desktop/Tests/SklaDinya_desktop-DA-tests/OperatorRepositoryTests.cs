using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Exceptions;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_DA_component.Repositories;
using SklaDinya_desktop_DA_tests.Helpers;

namespace SklaDinya_desktop_DA_tests;

public class OperatorRepositoryTests
{
    private const string Token = "valid.jwt.token";

    // ── GetOperatorsAsync ──────────────────────────────────────────────────

    [Fact]
    public async Task GetOperatorsAsync_ServerReturnsOk_ReturnsMappedList()
    {
        var id    = Guid.NewGuid();
        var repo  = new OperatorRepository(MockHttpFactory.CreateOk(new[] { FakeDto.Operator(id) }));
        var query = new OperatorSearchQuery { PageNumber = 1, PageSize = 10 };

        var result = await repo.GetOperatorsAsync(query, Token);

        Assert.Single(result);
        Assert.Equal(id, result[0].Id);
    }

    [Fact]
    public async Task GetOperatorsAsync_NullQuery_ThrowsArgumentNullException()
    {
        var repo = new OperatorRepository(MockHttpFactory.CreateOk());

        await Assert.ThrowsAsync<ArgumentNullException>(() => repo.GetOperatorsAsync(null!, Token));
    }

    [Fact]
    public async Task GetOperatorsAsync_EmptyToken_ThrowsArgumentException()
    {
        var repo  = new OperatorRepository(MockHttpFactory.CreateOk());
        var query = new OperatorSearchQuery { PageNumber = 1, PageSize = 10 };

        await Assert.ThrowsAsync<ArgumentException>(() => repo.GetOperatorsAsync(query, ""));
    }

    // ── CreateOperatorAsync ────────────────────────────────────────────────

    [Fact]
    public async Task CreateOperatorAsync_ValidArgs_ReturnsMappedOperator()
    {
        var id   = Guid.NewGuid();
        var repo = new OperatorRepository(MockHttpFactory.CreateOk(FakeDto.Operator(id)));
        var form = new OperatorCreateForm
        {
            Username = "op", Password = "p", Name = "N",
            Email    = "e@e.com", Role = OperatorRole.OrdinaryOperator
        };

        var result = await repo.CreateOperatorAsync(form, Token);

        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task CreateOperatorAsync_ServerReturnsConflict_ThrowsConflictException()
    {
        var repo = new OperatorRepository(MockHttpFactory.CreateConflict());
        var form = new OperatorCreateForm
        {
            Username = "existing", Password = "p", Name = "N",
            Email    = "e@e.com", Role = OperatorRole.OrdinaryOperator
        };

        await Assert.ThrowsAsync<ConflictException>(() => repo.CreateOperatorAsync(form, Token));
    }

    [Fact]
    public async Task CreateOperatorAsync_NullForm_ThrowsArgumentNullException()
    {
        var repo = new OperatorRepository(MockHttpFactory.CreateOk());

        await Assert.ThrowsAsync<ArgumentNullException>(() => repo.CreateOperatorAsync(null!, Token));
    }

    // ── GetOperatorByIdAsync ───────────────────────────────────────────────

    [Fact]
    public async Task GetOperatorByIdAsync_ValidArgs_ReturnsMappedOperator()
    {
        var id   = Guid.NewGuid();
        var repo = new OperatorRepository(MockHttpFactory.CreateOk(FakeDto.Operator(id)));

        var result = await repo.GetOperatorByIdAsync(id, Token);

        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task GetOperatorByIdAsync_ServerReturnsNotFound_ThrowsNotFoundException()
    {
        var repo = new OperatorRepository(MockHttpFactory.CreateNotFound());

        await Assert.ThrowsAsync<NotFoundException>(() => repo.GetOperatorByIdAsync(Guid.NewGuid(), Token));
    }

    [Fact]
    public async Task GetOperatorByIdAsync_EmptyToken_ThrowsArgumentException()
    {
        var repo = new OperatorRepository(MockHttpFactory.CreateOk());

        await Assert.ThrowsAsync<ArgumentException>(() => repo.GetOperatorByIdAsync(Guid.NewGuid(), ""));
    }

    // ── UpdateOperatorAsync ────────────────────────────────────────────────

    [Fact]
    public async Task UpdateOperatorAsync_ValidArgs_ReturnsMappedOperator()
    {
        var id   = Guid.NewGuid();
        var repo = new OperatorRepository(MockHttpFactory.CreateOk(FakeDto.Operator(id)));
        var form = new OperatorUpdateForm { Name = "Updated" };

        var result = await repo.UpdateOperatorAsync(id, form, Token);

        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task UpdateOperatorAsync_ServerReturnsNotFound_ThrowsNotFoundException()
    {
        var repo = new OperatorRepository(MockHttpFactory.CreateNotFound());
        var form = new OperatorUpdateForm { Name = "Updated" };

        await Assert.ThrowsAsync<NotFoundException>(() => repo.UpdateOperatorAsync(Guid.NewGuid(), form, Token));
    }

    [Fact]
    public async Task UpdateOperatorAsync_NullForm_ThrowsArgumentNullException()
    {
        var repo = new OperatorRepository(MockHttpFactory.CreateOk());

        await Assert.ThrowsAsync<ArgumentNullException>(() => repo.UpdateOperatorAsync(Guid.NewGuid(), null!, Token));
    }
}
