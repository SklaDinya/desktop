using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Interfaces.Services;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;

namespace SklaDinya_desktop_BL_component.Services;

/// <summary>
/// Сервис для работы с бронированиями.
/// </summary>
public class BookingService(IBookingRepository bookingRepository, ISessionService session) : IBookingService
{
    /// <summary>
    /// Чек последнего созданного (но ещё не оплаченного) бронирования.
    /// </summary>
    public BookingReceiptModel? LastReceipt { get; private set; }

    /// <inheritdoc/>
    public Task<List<BookingModel>> GetMyBookingsAsync(BookingSearchQuery query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));
        return bookingRepository.GetMyBookingsAsync(query, session.Token!);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Чек сохраняется в <see cref="LastReceipt"/>.
    /// </remarks>
    public async Task<BookingModel> CreateBookingAsync(BookingCreateForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));

        var receipt = await bookingRepository.CreateBookingAsync(form, session.Token!);
        LastReceipt = receipt;
        return receipt.Booking;
    }

    /// <inheritdoc/>
    public Task<BookingModel> GetMyBookingByIdAsync(Guid bookingId) =>
        bookingRepository.GetMyBookingByIdAsync(bookingId, session.Token!);

    /// <inheritdoc/>
    public Task<BookingModel> CancelMyBookingAsync(Guid bookingId) =>
        bookingRepository.CancelMyBookingAsync(bookingId, session.Token!);

    /// <inheritdoc/>
    public Task<List<BookingOperatorModel>> GetStorageBookingsAsync(OperatorBookingSearchQuery query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));
        return bookingRepository.GetStorageBookingsAsync(query, session.Token!);
    }
}
