using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Models;

namespace SklaDinya_desktop_BL_component.Interfaces.Repositories;

/// <summary>
/// Репозиторий для работы с оплатой бронирований
/// </summary>
public interface IPaymentRepository
{
    /// <summary>
    /// Гарантированная оплата бронирования.
    /// Возвращает список оплаченных бронирований.
    /// </summary>
    Task<List<BookingModel>> PayNoopAsync(PaymentForm form);

    /// <summary>
    /// Оплата бронирования с шансом 50%.
    /// Возвращает список оплаченных бронирований.
    /// Выбрасывает <see cref="Exceptions.PaymentFailedException"/> при неудаче.
    /// </summary>
    Task<List<BookingModel>> PayRandomAsync(PaymentForm form);
}
