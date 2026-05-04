using Moq;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Interfaces.Services;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Services;
using SklaDinya_desktop_BL_tests.Helpers;

namespace SklaDinya_desktop_BL_tests;

public class PaymentServiceTests
{
    private readonly Mock<IPaymentRepository> _repo = new();
    private readonly Mock<IBookingService> _bookingService = new();
    private readonly Mock<ISessionService> _session = new();
    private readonly IPaymentService _sut;

    private const string Token = "test.jwt.token";

    public PaymentServiceTests()
    {
        _session.Setup(s => s.Token).Returns(Token);
        _sut = new PaymentService(_repo.Object, _bookingService.Object, _session.Object);
    }

    // ── PayNoopAsync ───────────────────────────────────────────────────────

    [Fact]
    public async Task PayNoopAsync_WithLastReceipt_CallsRepoWithReceiptAndReturnsBooking()
    {
        var receipt = ModelBuilder.BookingReceipt();
        // Ожидаем одно оплаченное бронирование, а не список — сигнатура
        // IPaymentRepository.PayNoopAsync теперь возвращает BookingModel.
        var expected = ModelBuilder.Booking();

        _bookingService.Setup(b => b.LastReceipt).Returns(receipt);
        _repo.Setup(r => r.PayNoopAsync(
                It.Is<PaymentForm>(f => f.Receipt == receipt.Receipt), Token))
             .ReturnsAsync(expected);

        var result = await _sut.PayNoopAsync();

        Assert.Same(expected, result);
        _repo.Verify(r => r.PayNoopAsync(It.Is<PaymentForm>(f => f.Receipt == receipt.Receipt), Token), Times.Once);
    }

    [Fact]
    public async Task PayNoopAsync_NoLastReceipt_ThrowsInvalidOperationExceptionWithoutCallingRepo()
    {
        _bookingService.Setup(b => b.LastReceipt).Returns(null as BookingReceiptModel);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.PayNoopAsync());

        _repo.Verify(r => r.PayNoopAsync(It.IsAny<PaymentForm>(), It.IsAny<string>()), Times.Never);
    }

    // ── PayRandomAsync ─────────────────────────────────────────────────────

    [Fact]
    public async Task PayRandomAsync_WithLastReceipt_CallsRepoWithReceiptAndReturnsBooking()
    {
        var receipt = ModelBuilder.BookingReceipt();
        var expected = ModelBuilder.Booking();

        _bookingService.Setup(b => b.LastReceipt).Returns(receipt);
        _repo.Setup(r => r.PayRandomAsync(
                It.Is<PaymentForm>(f => f.Receipt == receipt.Receipt), Token))
             .ReturnsAsync(expected);

        var result = await _sut.PayRandomAsync();

        Assert.Same(expected, result);
        _repo.Verify(r => r.PayRandomAsync(It.Is<PaymentForm>(f => f.Receipt == receipt.Receipt), Token), Times.Once);
    }

    [Fact]
    public async Task PayRandomAsync_NoLastReceipt_ThrowsInvalidOperationExceptionWithoutCallingRepo()
    {
        _bookingService.Setup(b => b.LastReceipt).Returns(null as BookingReceiptModel);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.PayRandomAsync());

        _repo.Verify(r => r.PayRandomAsync(It.IsAny<PaymentForm>(), It.IsAny<string>()), Times.Never);
    }
}
