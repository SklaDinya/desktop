using Moq;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Interfaces.Services;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_BL_component.Services;
using SklaDinya_desktop_BL_tests.Helpers;

namespace SklaDinya_desktop_BL_tests;

public class BookingServiceTests
{
    private readonly Mock<IBookingRepository> _repo    = new();
    private readonly Mock<ISessionService>    _session = new();
    private readonly IBookingService          _sut;

    private const string Token = "test.jwt.token";

    public BookingServiceTests()
    {
        _session.Setup(s => s.Token).Returns(Token);
        _sut = new BookingService(_repo.Object, _session.Object);
    }

    // ── GetMyBookingsAsync ─────────────────────────────────────────────────

    [Fact]
    public async Task GetMyBookingsAsync_ValidQuery_ReturnsBookings()
    {
        var query    = new BookingSearchQuery { PageNumber = 1, PageSize = 10 };
        var expected = new List<SklaDinya_desktop_BL_component.Models.BookingModel> { ModelBuilder.Booking() };

        _repo.Setup(r => r.GetMyBookingsAsync(query, Token)).ReturnsAsync(expected);

        var result = await _sut.GetMyBookingsAsync(query);

        Assert.Equal(expected, result);
        _repo.Verify(r => r.GetMyBookingsAsync(query, Token), Times.Once);
    }

    // ── CreateBookingAsync ─────────────────────────────────────────────────

    [Fact]
    public async Task CreateBookingAsync_ValidForm_ReturnsBookingAndSavesReceipt()
    {
        var form    = new BookingCreateForm { StorageId = Guid.NewGuid(), CellIds = [Guid.NewGuid()], StartTime = DateTime.UtcNow, BookingTime = TimeSpan.FromHours(2) };
        var receipt = ModelBuilder.BookingReceipt();

        _repo.Setup(r => r.CreateBookingAsync(form, Token)).ReturnsAsync(receipt);

        var result = await _sut.CreateBookingAsync(form);

        Assert.Equal(receipt.Booking, result);
        Assert.Equal(receipt,         _sut.LastReceipt);
        _repo.Verify(r => r.CreateBookingAsync(form, Token), Times.Once);
    }

    // ── GetMyBookingByIdAsync ──────────────────────────────────────────────

    [Fact]
    public async Task GetMyBookingByIdAsync_ValidId_ReturnsBooking()
    {
        var expected = ModelBuilder.Booking();

        _repo.Setup(r => r.GetMyBookingByIdAsync(expected.Id, Token)).ReturnsAsync(expected);

        var result = await _sut.GetMyBookingByIdAsync(expected.Id);

        Assert.Equal(expected, result);
    }

    // ── CancelMyBookingAsync ───────────────────────────────────────────────

    [Fact]
    public async Task CancelMyBookingAsync_ValidId_ReturnsCancelledBooking()
    {
        var expected = ModelBuilder.Booking();

        _repo.Setup(r => r.CancelMyBookingAsync(expected.Id, Token)).ReturnsAsync(expected);

        var result = await _sut.CancelMyBookingAsync(expected.Id);

        Assert.Equal(expected, result);
    }

    // ── GetStorageBookingsAsync ────────────────────────────────────────────

    [Fact]
    public async Task GetStorageBookingsAsync_ValidQuery_ReturnsOperatorBookings()
    {
        var query    = new OperatorBookingSearchQuery { StartBooking = DateTime.UtcNow, EndBooking = DateTime.UtcNow.AddDays(1), PageNumber = 1, PageSize = 10 };
        var expected = new List<SklaDinya_desktop_BL_component.Models.BookingOperatorModel> { ModelBuilder.BookingOperator() };

        _repo.Setup(r => r.GetStorageBookingsAsync(query, Token)).ReturnsAsync(expected);

        var result = await _sut.GetStorageBookingsAsync(query);

        Assert.Equal(expected, result);
    }
}
