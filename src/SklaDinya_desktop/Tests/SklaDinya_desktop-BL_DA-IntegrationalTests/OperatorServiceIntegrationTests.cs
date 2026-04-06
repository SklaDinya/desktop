using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Exceptions;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_BL_DA_IntegrationalTests.Helpers;

namespace SklaDinya_desktop_BL_DA_IntegrationalTests;

/// <summary>
/// Интеграционные тесты OperatorService.
/// Цепочка: OperatorService → OperatorRepository → ApiClient(mock HTTP).
/// </summary>
public class OperatorServiceIntegrationTests
{
    private static readonly string Token = FakeDto.Token();

    // ── GetOperatorsAsync ──────────────────────────────────────────────────

    [Fact]
    public async Task GetOperatorsAsync_ServerReturnsOk_ReturnsMappedList()
    {
        var id      = Guid.NewGuid();
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Operator(
            MockHttpFactory.CreateOk(new[] { FakeDto.Operator(id) }), session);

        var result = await sut.GetOperatorsAsync(new OperatorSearchQuery { PageNumber = 1, PageSize = 10 });

        Assert.Single(result);
        Assert.Equal(id,                           result[0].Id);
        Assert.Equal(OperatorRole.OrdinaryOperator, result[0].Role);
    }

    [Fact]
    public async Task GetOperatorsAsync_ServerReturnsForbidden_ThrowsForbiddenException()
    {
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Operator(MockHttpFactory.CreateForbidden(), session);

        await Assert.ThrowsAsync<ForbiddenException>(
            () => sut.GetOperatorsAsync(new OperatorSearchQuery()));
    }

    // ── CreateOperatorAsync ────────────────────────────────────────────────

    [Fact]
    public async Task CreateOperatorAsync_ServerReturnsOk_ReturnsMappedOperator()
    {
        var id      = Guid.NewGuid();
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Operator(MockHttpFactory.CreateOk(FakeDto.Operator(id)), session);

        var result = await sut.CreateOperatorAsync(new OperatorCreateForm
        {
            Username = "op", Password = "p", Name = "N",
            Email    = "e@e.com", Role = OperatorRole.OrdinaryOperator
        });

        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task CreateOperatorAsync_ServerReturnsConflict_ThrowsConflictException()
    {
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Operator(MockHttpFactory.CreateConflict(), session);

        await Assert.ThrowsAsync<ConflictException>(() => sut.CreateOperatorAsync(new OperatorCreateForm
        {
            Username = "existing", Password = "p", Name = "N",
            Email    = "e@e.com", Role = OperatorRole.OrdinaryOperator
        }));
    }

    // ── GetOperatorByIdAsync ───────────────────────────────────────────────

    [Fact]
    public async Task GetOperatorByIdAsync_ServerReturnsOk_ReturnsMappedOperator()
    {
        var id      = Guid.NewGuid();
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Operator(MockHttpFactory.CreateOk(FakeDto.Operator(id)), session);

        var result = await sut.GetOperatorByIdAsync(id);

        Assert.Equal(id,          result.Id);
        Assert.Equal("operator1", result.Username);
    }

    [Fact]
    public async Task GetOperatorByIdAsync_ServerReturnsNotFound_ThrowsNotFoundException()
    {
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Operator(MockHttpFactory.CreateNotFound(), session);

        await Assert.ThrowsAsync<NotFoundException>(
            () => sut.GetOperatorByIdAsync(Guid.NewGuid()));
    }

    // ── UpdateOperatorAsync ────────────────────────────────────────────────

    [Fact]
    public async Task UpdateOperatorAsync_ServerReturnsOk_ReturnsMappedOperator()
    {
        var id      = Guid.NewGuid();
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Operator(MockHttpFactory.CreateOk(FakeDto.Operator(id)), session);

        var result = await sut.UpdateOperatorAsync(id, new OperatorUpdateForm { Name = "Updated" });

        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task UpdateOperatorAsync_ServerReturnsNotFound_ThrowsNotFoundException()
    {
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Operator(MockHttpFactory.CreateNotFound(), session);

        await Assert.ThrowsAsync<NotFoundException>(
            () => sut.UpdateOperatorAsync(Guid.NewGuid(), new OperatorUpdateForm { Name = "Updated" }));
    }
}
