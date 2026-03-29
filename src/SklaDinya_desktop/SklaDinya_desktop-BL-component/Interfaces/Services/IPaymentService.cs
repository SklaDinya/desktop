using SklaDinya_desktop_BL_component.Models;

namespace SklaDinya_desktop_BL_component.Interfaces.Services;

/// <summary>
/// Сервис для проведения оплаты бронирований.
/// Чек для оплаты берётся автоматически из <see cref="IBookingService.LastReceipt"/>.
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Провести гарантированную оплату последнего созданного бронирования.
    /// Возвращает список оплаченных бронирований.
    /// </summary>
    Task<List<BookingModel>> PayNoopAsync();

    /// <summary>
    /// Провести оплату последнего созданного бронирования с шансом 50%.
    /// Возвращает список оплаченных бронирований при успехе.
    /// Выбрасывает <see cref="Exceptions.PaymentFailedException"/> при неудаче.
    /// </summary>
    Task<List<BookingModel>> PayRandomAsync();
}