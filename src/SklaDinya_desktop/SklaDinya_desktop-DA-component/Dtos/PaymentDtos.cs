namespace SklaDinya_desktop_DA_component.Dtos;

/// <summary>Запрос гарантированной оплаты — POST /api/v1/payments/noop</summary>
internal record PaymentNoopRequest(string Receipt);

/// <summary>Запрос оплаты с шансом 50% — POST /api/v1/payments/random</summary>
internal record PaymentRandomRequest(string Receipt);
