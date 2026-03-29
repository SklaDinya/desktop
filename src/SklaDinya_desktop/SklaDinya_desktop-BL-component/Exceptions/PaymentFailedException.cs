namespace SklaDinya_desktop_BL_component.Exceptions;

/// <summary>
/// Оплата не прошла (HTTP 418, используется сервером как признак неудачной моковой оплаты)
/// </summary>
public class PaymentFailedException : ApiException
{
    public PaymentFailedException()
        : base(418, "Оплата не прошла. Попробуйте ещё раз.") { }

    public PaymentFailedException(string message)
        : base(418, message) { }
}
