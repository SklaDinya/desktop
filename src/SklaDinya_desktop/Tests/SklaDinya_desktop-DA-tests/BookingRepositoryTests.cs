using SklaDinya_desktop_BL_component.Exceptions;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_DA_component.Repositories;
using SklaDinya_desktop_DA_tests.Helpers;

namespace SklaDinya_desktop_DA_tests;

public class BookingRepositoryTests
{
    private const string Token = "valid.jwt.token";

    // ── GetMyBookingsAsync ─────────────────────────────────────────────────

    [Fact]
    public async Task GetMyBookingsAsync_ServerReturnsOk_ReturnsMappedList()
    {
        var id    = Guid.NewGuid();
        var repo  = new BookingRepository(MockHttpFactory.CreateOk(new[] { FakeDto.BookingForUser(id) }));
        var query = new BookingSearchQuery { PageNumber = 1, PageSize = 10 };

        var result = await repo.GetMyBookingsAsync(query, Token);

        Assert.Single(result);
        Assert.Equal(id, result[0].Id);
    }

    [Fact]
    public async Task GetMyBookingsAsync_ServerReturnsUnauthorized_ThrowsUnauthorizedException()
    {
        var repo  = new BookingRepository(MockHttpFactory.CreateUnauthorized());
        var query = new BookingSearchQuery { PageNumber = 1, PageSize = 10 };

        await Assert.ThrowsAsync<UnauthorizedException>(() => repo.GetMyBookingsAsync(query, Token));
    }

    [Fact]
    public async Task GetMyBookingsAsync_EmptyToken_ThrowsArgumentException()
    {
        var repo  = new BookingRepository(MockHttpFactory.CreateOk());
        var query = new BookingSearchQuery { PageNumber = 1, PageSize = 10 };

        await Assert.ThrowsAsync<ArgumentException>(() => repo.GetMyBookingsAsync(query, ""));
    }

    // ── CreateBookingAsync ─────────────────────────────────────────────────

    [Fact]
    public async Task CreateBookingAsync_ServerReturnsOk_ReturnsMappedReceipt()
    {
        var repo = new BookingRepository(MockHttpFactory.CreateOk(FakeDto.BookingReceipt()));
        var form = new BookingCreateForm
        {
            StorageId   = Guid.NewGuid(),
            CellIds     = [Guid.NewGuid()],
            StartTime   = DateTime.UtcNow,
            BookingTime = TimeSpan.FromHours(2)
        };

        var result = await repo.CreateBookingAsync(form, Token);

        Assert.NotNull(result.Booking);
        Assert.Equal("receipt.jwt.token", result.Receipt);
    }

    [Fact]
    public async Task CreateBookingAsync_ServerReturnsConflict_ThrowsConflictException()
    {
        var repo = new BookingRepository(MockHttpFactory.CreateConflict());
        var form = new BookingCreateForm
        {
            StorageId   = Guid.NewGuid(),
            CellIds     = [Guid.NewGuid()],
            StartTime   = DateTime.UtcNow,
            BookingTime = TimeSpan.FromHours(1)
        };

        await Assert.ThrowsAsync<ConflictException>(() => repo.CreateBookingAsync(form, Token));
    }

    [Fact]
    public async Task CreateBookingAsync_EmptyToken_ThrowsArgumentException()
    {
        var repo = new BookingRepository(MockHttpFactory.CreateOk());
        var form = new BookingCreateForm { StorageId = Guid.NewGuid(), CellIds = [Guid.NewGuid()], StartTime = DateTime.UtcNow, BookingTime = TimeSpan.FromHours(1) };

        await Assert.ThrowsAsync<ArgumentException>(() => repo.CreateBookingAsync(form, ""));
    }

    // ── GetMyBookingByIdAsync ──────────────────────────────────────────────

    [Fact]
    public async Task GetMyBookingByIdAsync_ValidArgs_ReturnsMappedBooking()
    {
        var id   = Guid.NewGuid();
        var repo = new BookingRepository(MockHttpFactory.CreateOk(FakeDto.BookingForUser(id)));

        var result = await repo.GetMyBookingByIdAsync(id, Token);

        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task GetMyBookingByIdAsync_ServerReturnsNotFound_ThrowsNotFoundException()
    {
        var repo = new BookingRepository(MockHttpFactory.CreateNotFound());

        await Assert.ThrowsAsync<NotFoundException>(() => repo.GetMyBookingByIdAsync(Guid.NewGuid(), Token));
    }

    [Fact]
    public async Task GetMyBookingByIdAsync_EmptyToken_ThrowsArgumentException()
    {
        var repo = new BookingRepository(MockHttpFactory.CreateOk());

        await Assert.ThrowsAsync<ArgumentException>(() => repo.GetMyBookingByIdAsync(Guid.NewGuid(), ""));
    }

    // ── CancelMyBookingAsync ───────────────────────────────────────────────

    [Fact]
    public async Task CancelMyBookingAsync_ValidArgs_ReturnsMappedBooking()
    {
        var id   = Guid.NewGuid();
        var repo = new BookingRepository(MockHttpFactory.CreateOk(FakeDto.BookingForUser(id)));

        var result = await repo.CancelMyBookingAsync(id, Token);

        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task CancelMyBookingAsync_ServerReturnsNotFound_ThrowsNotFoundException()
    {
        var repo = new BookingRepository(MockHttpFactory.CreateNotFound());

        await Assert.ThrowsAsync<NotFoundException>(() => repo.CancelMyBookingAsync(Guid.NewGuid(), Token));
    }

    [Fact]
    public async Task CancelMyBookingAsync_EmptyToken_ThrowsArgumentException()
    {
        var repo = new BookingRepository(MockHttpFactory.CreateOk());

        await Assert.ThrowsAsync<ArgumentException>(() => repo.CancelMyBookingAsync(Guid.NewGuid(), ""));
    }

    // ── GetStorageBookingsAsync ────────────────────────────────────────────

    [Fact]
    public async Task GetStorageBookingsAsync_ServerReturnsOk_ReturnsMappedList()
    {
        var id    = Guid.NewGuid();
        var repo  = new BookingRepository(MockHttpFactory.CreateOk(new[] { FakeDto.BookingForOperator(id) }));
        var query = new OperatorBookingSearchQuery
        {
            StartBooking = DateTime.UtcNow,
            EndBooking   = DateTime.UtcNow.AddDays(1),
            PageNumber   = 1,
            PageSize     = 10
        };

        var result = await repo.GetStorageBookingsAsync(query, Token);

        Assert.Single(result);
        Assert.Equal(id, result[0].Id);
    }

    [Fact]
    public async Task GetStorageBookingsAsync_EmptyToken_ThrowsArgumentException()
    {
        var repo  = new BookingRepository(MockHttpFactory.CreateOk());
        var query = new OperatorBookingSearchQuery { StartBooking = DateTime.UtcNow, EndBooking = DateTime.UtcNow.AddDays(1), PageNumber = 1, PageSize = 10 };

        await Assert.ThrowsAsync<ArgumentException>(() => repo.GetStorageBookingsAsync(query, ""));
    }
}
