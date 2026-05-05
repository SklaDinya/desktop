using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Models;

namespace SklaDinya_desktop_BL_component.Interfaces.Repositories;

/// <summary>
/// Репозиторий для работы с оплатой бронирований.
/// Все методы защищённые — требуют JWT-токен.
/// </summary>
/// <remarks>
/// По swagger эндпоинты <c>/payments/noop</c> и <c>/payments/random</c>
/// возвращают массив бронирований. Реальный бэкенд возвращает одно
/// (то самое, которое оплачивали) — это расхождение в спецификации,
/// договорились следовать фактическому поведению. Поэтому сигнатуры
/// репозитория возвращают <see cref="BookingModel"/>, а не список.
/// </remarks>
public interface IPaymentRepository
{
    /// <summary>
    /// Гарантированная оплата бронирования.
    /// Возвращает оплаченное бронирование.
    /// </summary>
    Task<BookingModel> PayNoopAsync(PaymentForm form, string token);

    /// <summary>
    /// Оплата бронирования с шансом 50%.
    /// Возвращает оплаченное бронирование.
    /// Выбрасывает <see cref="Exceptions.PaymentFailedException"/> при неудаче.
    /// </summary>
    Task<BookingModel> PayRandomAsync(PaymentForm form, string token);
}
