namespace SklaDinya_desktop_DA_component.Dtos;

/// <summary>Запрос гарантированной оплаты — POST /api/v1/payments/noop</summary>
public record PaymentNoopRequest(string Receipt);

/// <summary>Запрос оплаты с шансом 50% — POST /api/v1/payments/random</summary>
public record PaymentRandomRequest(string Receipt);
