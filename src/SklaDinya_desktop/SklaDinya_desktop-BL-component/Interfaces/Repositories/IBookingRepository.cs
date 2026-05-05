using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;

namespace SklaDinya_desktop_BL_component.Interfaces.Repositories;

/// <summary>
/// Репозиторий для работы с бронированиями.
/// Все методы защищённые — требуют JWT-токен.
/// </summary>
public interface IBookingRepository
{
    /// <summary>Получить список своих бронирований (для клиента)</summary>
    Task<List<BookingModel>> GetMyBookingsAsync(BookingSearchQuery query, string token);

    /// <summary>Создать бронирование. Возвращает чек (booking + receipt JWT).</summary>
    Task<BookingReceiptModel> CreateBookingAsync(BookingCreateForm form, string token);

    /// <summary>Получить конкретное своё бронирование по ID (для клиента)</summary>
    Task<BookingModel> GetMyBookingByIdAsync(Guid bookingId, string token);

    /// <summary>Отменить своё бронирование (для клиента)</summary>
    Task<BookingModel> CancelMyBookingAsync(Guid bookingId, string token);

    /// <summary>Найти бронирования своего пункта хранения (для оператора)</summary>
    Task<List<BookingOperatorModel>> GetStorageBookingsAsync(OperatorBookingSearchQuery query, string token);
}
