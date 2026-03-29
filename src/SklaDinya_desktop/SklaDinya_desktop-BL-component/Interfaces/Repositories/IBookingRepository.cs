using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;

namespace SklaDinya_desktop_BL_component.Interfaces.Repositories;

/// <summary>
/// Репозиторий для работы с бронированиями
/// </summary>
public interface IBookingRepository
{
    /// <summary>Получить список своих бронирований (для клиента)</summary>
    Task<List<BookingModel>> GetMyBookingsAsync(BookingSearchQuery query);

    /// <summary>Создать бронирование. Возвращает чек для оплаты.</summary>
    Task<BookingReceiptModel> CreateBookingAsync(BookingCreateForm form);

    /// <summary>Получить конкретное своё бронирование по ID (для клиента)</summary>
    Task<BookingModel> GetMyBookingByIdAsync(Guid bookingId);

    /// <summary>Отменить своё бронирование (для клиента)</summary>
    Task<BookingModel> CancelMyBookingAsync(Guid bookingId);

    /// <summary>Найти бронирования своего пункта хранения (для оператора)</summary>
    Task<List<BookingOperatorModel>> GetStorageBookingsAsync(OperatorBookingSearchQuery query);
}
