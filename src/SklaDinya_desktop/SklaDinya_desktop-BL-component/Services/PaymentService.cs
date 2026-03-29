using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Interfaces.Services;
using SklaDinya_desktop_BL_component.Models;

namespace SklaDinya_desktop_BL_component.Services;

/// <summary>
/// Сервис для проведения оплаты бронирований.
/// </summary>
public class PaymentService(IPaymentRepository paymentRepository, ISessionService session) : IPaymentService
{
    /// <inheritdoc/>
    public Task<List<BookingModel>> PayNoopAsync(PaymentForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        return paymentRepository.PayNoopAsync(form, session.Token!);
    }

    /// <inheritdoc/>
    /// <exception cref="Exceptions.PaymentFailedException">
    /// Выбрасывается, когда сервер вернул 418 — оплата не прошла.
    /// UI должен поймать это исключение и предложить попробовать снова.
    /// </exception>
    public Task<List<BookingModel>> PayRandomAsync(PaymentForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        return paymentRepository.PayRandomAsync(form, session.Token!);
    }
}
