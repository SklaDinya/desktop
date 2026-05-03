using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Interfaces.Services;
using SklaDinya_desktop_BL_component.Models;

namespace SklaDinya_desktop_BL_component.Services;

/// <summary>
/// Сервис для проведения оплаты бронирований.
/// </summary>
public class PaymentService(
    IPaymentRepository paymentRepository,
    IBookingService bookingService,
    ISessionService session) : IPaymentService
{
    private PaymentForm GetReceiptForm()
    {
        if (bookingService.LastReceipt is null)
            throw new InvalidOperationException(
                "Нет активного чека для оплаты. Сначала создайте бронирование.");

        return new PaymentForm { Receipt = bookingService.LastReceipt.Receipt };
    }

    /// <inheritdoc/>
    public Task<BookingModel> PayNoopAsync()
    {
        var form = GetReceiptForm();
        return paymentRepository.PayNoopAsync(form, session.Token!);
    }

    /// <inheritdoc/>
    /// <exception cref="Exceptions.PaymentFailedException">
    /// Выбрасывается, когда сервер вернул 418 — оплата не прошла.
    /// UI должен поймать это исключение и предложить попробовать снова.
    /// </exception>
    public Task<BookingModel> PayRandomAsync()
    {
        var form = GetReceiptForm();
        return paymentRepository.PayRandomAsync(form, session.Token!);
    }
}
