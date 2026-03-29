using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Models;

namespace SklaDinya_desktop_BL_component.Interfaces.Services;

/// <summary>
/// Сервис для проведения оплаты бронирований
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Провести гарантированную оплату бронирования.
    /// Возвращает список оплаченных бронирований.
    /// </summary>
    Task<List<BookingModel>> PayNoopAsync(PaymentForm form);

    /// <summary>
    /// Провести оплату с шансом 50%.
    /// Возвращает список оплаченных бронирований при успехе.
    /// Выбрасывает <see cref="Exceptions.PaymentFailedException"/> при неудаче.
    /// </summary>
    Task<List<BookingModel>> PayRandomAsync(PaymentForm form);
}
