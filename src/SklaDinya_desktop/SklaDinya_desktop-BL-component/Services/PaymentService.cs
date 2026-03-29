using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Interfaces.Services;
using SklaDinya_desktop_BL_component.Models;

namespace SklaDinya_desktop_BL_component.Services;

/// <summary>
/// Сервис для проведения оплаты бронирований.
/// </summary>
public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;

    public PaymentService(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    /// <inheritdoc/>
    public Task<List<BookingModel>> PayNoopAsync(PaymentForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        return _paymentRepository.PayNoopAsync(form);
    }

    /// <inheritdoc/>
    /// <exception cref="Exceptions.PaymentFailedException">
    /// Выбрасывается, когда сервер вернул 418 — оплата не прошла.
    /// UI должен поймать это исключение и предложить попробовать снова.
    /// </exception>
    public Task<List<BookingModel>> PayRandomAsync(PaymentForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        return _paymentRepository.PayRandomAsync(form);
    }
}
