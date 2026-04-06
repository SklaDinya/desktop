using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Exceptions;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_BL_DA_IntegrationalTests.Helpers;

namespace SklaDinya_desktop_BL_DA_IntegrationalTests;

/// <summary>
/// Интеграционные тесты BookingService.
/// Цепочка: BookingService → BookingRepository → ApiClient(mock HTTP).
/// </summary>
public class BookingServiceIntegrationTests
{
    private static readonly string Token = FakeDto.Token();

    // ── GetMyBookingsAsync ─────────────────────────────────────────────────

    [Fact]
    public async Task GetMyBookingsAsync_ServerReturnsOk_ReturnsMappedBookings()
    {
        var id      = Guid.NewGuid();
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Booking(
            MockHttpFactory.CreateOk(new[] { FakeDto.BookingForUser(id) }), session);

        var result = await sut.GetMyBookingsAsync(new BookingSearchQuery { PageNumber = 1, PageSize = 10 });

        Assert.Single(result);
        Assert.Equal(id, result[0].Id);
        Assert.NotNull(result[0].Storage);
        Assert.NotEmpty(result[0].Cells);
    }

    [Fact]
    public async Task GetMyBookingsAsync_ServerReturnsUnauthorized_ThrowsUnauthorizedException()
    {
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Booking(MockHttpFactory.CreateUnauthorized(), session);

        await Assert.ThrowsAsync<UnauthorizedException>(
            () => sut.GetMyBookingsAsync(new BookingSearchQuery()));
    }

    // ── CreateBookingAsync ─────────────────────────────────────────────────

    [Fact]
    public async Task CreateBookingAsync_ServerReturnsOk_ReturnsBookingAndSavesLastReceipt()
    {
        var bookingId = Guid.NewGuid();
        var session   = ServiceFactory.Session(Token);
        var sut       = ServiceFactory.Booking(
            MockHttpFactory.CreateOk(FakeDto.BookingReceipt(bookingId)), session);

        var result = await sut.CreateBookingAsync(new BookingCreateForm
        {
            StorageId   = Guid.NewGuid(),
            CellIds     = [Guid.NewGuid()],
            StartTime   = DateTime.UtcNow,
            BookingTime = TimeSpan.FromHours(2)
        });

        Assert.Equal(bookingId, result.Id);
        Assert.NotNull(sut.LastReceipt);
        Assert.Equal("receipt.jwt.token", sut.LastReceipt!.Receipt);
    }

    [Fact]
    public async Task CreateBookingAsync_ServerReturnsConflict_ThrowsConflictException()
    {
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Booking(MockHttpFactory.CreateConflict(), session);

        await Assert.ThrowsAsync<ConflictException>(() => sut.CreateBookingAsync(new BookingCreateForm
        {
            StorageId   = Guid.NewGuid(),
            CellIds     = [Guid.NewGuid()],
            StartTime   = DateTime.UtcNow,
            BookingTime = TimeSpan.FromHours(1)
        }));
    }

    // ── GetMyBookingByIdAsync ──────────────────────────────────────────────

    [Fact]
    public async Task GetMyBookingByIdAsync_ServerReturnsOk_ReturnsMappedBooking()
    {
        var id      = Guid.NewGuid();
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Booking(
            MockHttpFactory.CreateOk(FakeDto.BookingForUser(id)), session);

        var result = await sut.GetMyBookingByIdAsync(id);

        Assert.Equal(id, result.Id);
        Assert.Equal(BookingStatus.Paid, result.Status);
    }

    [Fact]
    public async Task GetMyBookingByIdAsync_ServerReturnsNotFound_ThrowsNotFoundException()
    {
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Booking(MockHttpFactory.CreateNotFound(), session);

        await Assert.ThrowsAsync<NotFoundException>(
            () => sut.GetMyBookingByIdAsync(Guid.NewGuid()));
    }

    // ── CancelMyBookingAsync ───────────────────────────────────────────────

    [Fact]
    public async Task CancelMyBookingAsync_ServerReturnsOk_ReturnsCancelledBooking()
    {
        var id      = Guid.NewGuid();
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Booking(
            MockHttpFactory.CreateOk(FakeDto.BookingForUser(id)), session);

        var result = await sut.CancelMyBookingAsync(id);

        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task CancelMyBookingAsync_ServerReturnsNotFound_ThrowsNotFoundException()
    {
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Booking(MockHttpFactory.CreateNotFound(), session);

        await Assert.ThrowsAsync<NotFoundException>(
            () => sut.CancelMyBookingAsync(Guid.NewGuid()));
    }

    // ── GetStorageBookingsAsync ────────────────────────────────────────────

    [Fact]
    public async Task GetStorageBookingsAsync_ServerReturnsOk_ReturnsMappedOperatorBookings()
    {
        var id      = Guid.NewGuid();
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Booking(
            MockHttpFactory.CreateOk(new[] { FakeDto.BookingForOperator(id) }), session);

        var result = await sut.GetStorageBookingsAsync(new OperatorBookingSearchQuery
        {
            StartBooking = DateTime.UtcNow,
            EndBooking   = DateTime.UtcNow.AddDays(1),
            PageNumber   = 1,
            PageSize     = 10
        });

        Assert.Single(result);
        Assert.Equal(id, result[0].Id);
        Assert.NotNull(result[0].User);
    }

    [Fact]
    public async Task GetStorageBookingsAsync_ServerReturnsForbidden_ThrowsForbiddenException()
    {
        var session = ServiceFactory.Session(Token);
        var sut     = ServiceFactory.Booking(MockHttpFactory.CreateForbidden(), session);

        await Assert.ThrowsAsync<ForbiddenException>(
            () => sut.GetStorageBookingsAsync(new OperatorBookingSearchQuery
            {
                StartBooking = DateTime.UtcNow,
                EndBooking   = DateTime.UtcNow.AddDays(1)
            }));
    }
}
