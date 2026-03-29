namespace SklaDinya_desktop_BL_component.Forms;

/// <summary>
/// Форма для проведения оплаты бронирования.
/// Используется как для гарантированной оплаты, так и для оплаты с шансом 50%.
/// </summary>
public class PaymentForm
{
    /// <summary>JWT-чек, полученный при создании бронирования</summary>
    public string Receipt { get; set; } = string.Empty;
}
