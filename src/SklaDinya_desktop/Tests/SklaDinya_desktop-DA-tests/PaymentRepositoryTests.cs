using SklaDinya_desktop_BL_component.Exceptions;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_DA_component.Repositories;
using SklaDinya_desktop_DA_tests.Helpers;

namespace SklaDinya_desktop_DA_tests;

public class PaymentRepositoryTests
{
    private const string Token = "valid.jwt.token";

    // ── PayNoopAsync ───────────────────────────────────────────────────────

    [Fact]
    public async Task PayNoopAsync_ServerReturnsOk_ReturnsPaidBookings()
    {
        var id   = Guid.NewGuid();
        var repo = new PaymentRepository(MockHttpFactory.CreateOk(new[] { FakeDto.BookingForUser(id) }));
        var form = new PaymentForm { Receipt = "receipt.jwt" };

        var result = await repo.PayNoopAsync(form, Token);

        Assert.Single(result);
        Assert.Equal(id, result[0].Id);
    }

    [Fact]
    public async Task PayNoopAsync_ServerReturnsUnauthorized_ThrowsUnauthorizedException()
    {
        var repo = new PaymentRepository(MockHttpFactory.CreateUnauthorized());
        var form = new PaymentForm { Receipt = "receipt.jwt" };

        await Assert.ThrowsAsync<UnauthorizedException>(() => repo.PayNoopAsync(form, Token));
    }

    [Fact]
    public async Task PayNoopAsync_EmptyToken_ThrowsArgumentException()
    {
        var repo = new PaymentRepository(MockHttpFactory.CreateOk());
        var form = new PaymentForm { Receipt = "receipt.jwt" };

        await Assert.ThrowsAsync<ArgumentException>(() => repo.PayNoopAsync(form, ""));
    }

    // ── PayRandomAsync ─────────────────────────────────────────────────────

    [Fact]
    public async Task PayRandomAsync_ServerReturnsOk_ReturnsPaidBookings()
    {
        var id   = Guid.NewGuid();
        var repo = new PaymentRepository(MockHttpFactory.CreateOk(new[] { FakeDto.BookingForUser(id) }));
        var form = new PaymentForm { Receipt = "receipt.jwt" };

        var result = await repo.PayRandomAsync(form, Token);

        Assert.Single(result);
        Assert.Equal(id, result[0].Id);
    }

    [Fact]
    public async Task PayRandomAsync_ServerReturns418_ThrowsPaymentFailedException()
    {
        var repo = new PaymentRepository(MockHttpFactory.CreatePaymentFailed());
        var form = new PaymentForm { Receipt = "receipt.jwt" };

        await Assert.ThrowsAsync<PaymentFailedException>(() => repo.PayRandomAsync(form, Token));
    }

    [Fact]
    public async Task PayRandomAsync_EmptyToken_ThrowsArgumentException()
    {
        var repo = new PaymentRepository(MockHttpFactory.CreateOk());
        var form = new PaymentForm { Receipt = "receipt.jwt" };

        await Assert.ThrowsAsync<ArgumentException>(() => repo.PayRandomAsync(form, ""));
    }
}
