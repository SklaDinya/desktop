namespace SklaDinya_desktop_BL_component.Models;

/// <summary>
/// Чек, возвращаемый после создания бронирования, используется для его оплаты
/// </summary>
public class BookingReceiptModel
{
    /// <summary>JWT-чек для проведения оплаты</summary>
    public string Receipt { get; set; } = string.Empty;
}
