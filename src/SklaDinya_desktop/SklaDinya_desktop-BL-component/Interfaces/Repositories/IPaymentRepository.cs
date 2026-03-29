using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Models;

namespace SklaDinya_desktop_BL_component.Interfaces.Repositories;

/// <summary>
/// Репозиторий для работы с оплатой бронирований.
/// Все методы защищённые — требуют JWT-токен.
/// </summary>
public interface IPaymentRepository
{
    /// <summary>
    /// Гарантированная оплата бронирования.
    /// Возвращает список оплаченных бронирований.
    /// </summary>
    Task<List<BookingModel>> PayNoopAsync(PaymentForm form, string token);

    /// <summary>
    /// Оплата бронирования с шансом 50%.
    /// Возвращает список оплаченных бронирований.
    /// Выбрасывает <see cref="Exceptions.PaymentFailedException"/> при неудаче.
    /// </summary>
    Task<List<BookingModel>> PayRandomAsync(PaymentForm form, string token);
}
