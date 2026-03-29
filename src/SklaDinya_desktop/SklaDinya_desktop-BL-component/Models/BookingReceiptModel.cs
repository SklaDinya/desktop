namespace SklaDinya_desktop_BL_component.Models;

/// <summary>
/// Чек, возвращаемый после создания бронирования.
/// Содержит данные о созданном бронировании и JWT-чек для его оплаты.
/// </summary>
public class BookingReceiptModel
{
    /// <summary>Данные созданного бронирования</summary>
    public BookingModel Booking { get; set; } = null!;

    /// <summary>JWT-чек для проведения оплаты</summary>
    public string Receipt { get; set; } = string.Empty;
}