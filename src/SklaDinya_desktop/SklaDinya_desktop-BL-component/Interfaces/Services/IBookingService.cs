using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;

namespace SklaDinya_desktop_BL_component.Interfaces.Services;

/// <summary>
/// Сервис для работы с бронированиями
/// </summary>
public interface IBookingService
{
    /// <summary>
    /// Чек последнего созданного бронирования.
    /// Заполняется после <see cref="CreateBookingAsync"/> и используется
    /// сервисом оплаты для получения receipt JWT.
    /// </summary>
    BookingReceiptModel LastReceipt { get; }

    /// <summary>Получить список своих бронирований (для клиента)</summary>
    Task<List<BookingModel>> GetMyBookingsAsync(BookingSearchQuery query);

    /// <summary>
    /// Создать бронирование.
    /// Чек сохраняется в <see cref="LastReceipt"/> — передавать его в UI не нужно.
    /// </summary>
    Task<BookingModel> CreateBookingAsync(BookingCreateForm form);

    /// <summary>Получить конкретное своё бронирование по ID (для клиента)</summary>
    Task<BookingModel> GetMyBookingByIdAsync(Guid bookingId);

    /// <summary>Отменить своё бронирование (для клиента)</summary>
    Task<BookingModel> CancelMyBookingAsync(Guid bookingId);

    /// <summary>Найти бронирования своего пункта хранения (для оператора)</summary>
    Task<List<BookingOperatorModel>> GetStorageBookingsAsync(OperatorBookingSearchQuery query);
}