using SklaDinya_desktop_BL_component.Exceptions;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_DA_IntegrationalTests.Helpers;

namespace SklaDinya_desktop_BL_DA_IntegrationalTests;

/// <summary>
/// Интеграционные тесты PaymentService.
/// </summary>
public class PaymentServiceIntegrationTests
{
    private static readonly string Token = FakeDto.Token();

    /// <summary>
    /// Вспомогательный метод: создаёт бронирование через реальную цепочку,
    /// чтобы LastReceipt был заполнен корректно перед вызовом оплаты.
    /// </summary>
    private static async Task<(
        SklaDinya_desktop_BL_component.Services.BookingService Booking,
        SklaDinya_desktop_BL_component.Services.PaymentService Payment)>
        BuildChainWithReceiptAsync(System.Net.HttpStatusCode paymentStatus)
    {
        var session = ServiceFactory.Session(Token);

        // Шаг 1: создаём бронирование — нужен отдельный клиент с ответом чека
        var bookingClient = MockHttpFactory.CreateOk(FakeDto.BookingReceipt());
        var bookingService = ServiceFactory.Booking(bookingClient, session);
        await bookingService.CreateBookingAsync(new BookingCreateForm
        {
            StorageId = Guid.NewGuid(),
            CellIds = [Guid.NewGuid()],
            StartTime = DateTime.UtcNow,
            BookingTime = TimeSpan.FromHours(2)
        });

        // Шаг 2: строим PaymentService с нужным статусом оплаты
        var paymentClient = MockHttpFactory.Create(paymentStatus,
            paymentStatus == System.Net.HttpStatusCode.OK
                ? (object?)FakeDto.BookingForUser()
                : null);
        var paymentService = ServiceFactory.Payment(paymentClient, bookingService, session);

        return (bookingService, paymentService);
    }

    // ── PayNoopAsync ───────────────────────────────────────────────────────

    [Fact]
    public async Task PayNoopAsync_AfterCreateBooking_ReturnsPaidBooking()
    {
        var (_, payment) = await BuildChainWithReceiptAsync(System.Net.HttpStatusCode.OK);

        var result = await payment.PayNoopAsync();

        Assert.NotNull(result);
    }

    [Fact]
    public async Task PayNoopAsync_WithoutPriorCreateBooking_ThrowsInvalidOperationException()
    {
        var session = ServiceFactory.Session(Token);
        var bookingService = ServiceFactory.Booking(MockHttpFactory.CreateOk(), session);
        var paymentService = ServiceFactory.Payment(MockHttpFactory.CreateOk(), bookingService, session);

        // LastReceipt == null, так как CreateBookingAsync не вызывался
        await Assert.ThrowsAsync<InvalidOperationException>(() => paymentService.PayNoopAsync());
    }

    [Fact]
    public async Task PayNoopAsync_ServerReturnsUnauthorized_ThrowsUnauthorizedException()
    {
        var (_, payment) = await BuildChainWithReceiptAsync(System.Net.HttpStatusCode.Unauthorized);

        await Assert.ThrowsAsync<UnauthorizedException>(() => payment.PayNoopAsync());
    }

    // ── PayRandomAsync ─────────────────────────────────────────────────────

    [Fact]
    public async Task PayRandomAsync_AfterCreateBooking_ServerReturnsOk_ReturnsPaidBooking()
    {
        var (_, payment) = await BuildChainWithReceiptAsync(System.Net.HttpStatusCode.OK);

        var result = await payment.PayRandomAsync();

        Assert.NotNull(result);
    }

    [Fact]
    public async Task PayRandomAsync_AfterCreateBooking_ServerReturns418_ThrowsPaymentFailedException()
    {
        var (_, payment) = await BuildChainWithReceiptAsync((System.Net.HttpStatusCode)418);

        await Assert.ThrowsAsync<PaymentFailedException>(() => payment.PayRandomAsync());
    }

    [Fact]
    public async Task PayRandomAsync_WithoutPriorCreateBooking_ThrowsInvalidOperationException()
    {
        var session = ServiceFactory.Session(Token);
        var bookingService = ServiceFactory.Booking(MockHttpFactory.CreateOk(), session);
        var paymentService = ServiceFactory.Payment(MockHttpFactory.CreateOk(), bookingService, session);

        await Assert.ThrowsAsync<InvalidOperationException>(() => paymentService.PayRandomAsync());
    }
}
