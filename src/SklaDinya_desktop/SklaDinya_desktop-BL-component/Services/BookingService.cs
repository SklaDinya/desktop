using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Interfaces.Services;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;

namespace SklaDinya_desktop_BL_component.Services;

/// <summary>
/// Сервис для работы с бронированиями.
/// </summary>
public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;

    public BookingService(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    /// <inheritdoc/>
    public Task<List<BookingModel>> GetMyBookingsAsync(BookingSearchQuery query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));
        return _bookingRepository.GetMyBookingsAsync(query);
    }

    /// <inheritdoc/>
    public Task<BookingReceiptModel> CreateBookingAsync(BookingCreateForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        return _bookingRepository.CreateBookingAsync(form);
    }

    /// <inheritdoc/>
    public Task<BookingModel> GetMyBookingByIdAsync(Guid bookingId) =>
        _bookingRepository.GetMyBookingByIdAsync(bookingId);

    /// <inheritdoc/>
    public Task<BookingModel> CancelMyBookingAsync(Guid bookingId) =>
        _bookingRepository.CancelMyBookingAsync(bookingId);

    /// <inheritdoc/>
    public Task<List<BookingOperatorModel>> GetStorageBookingsAsync(OperatorBookingSearchQuery query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));
        return _bookingRepository.GetStorageBookingsAsync(query);
    }
}
