using SklaDinya_desktop_BL_component.Models;

namespace SklaDinya_desktop_BL_component.Interfaces.Services;

/// <summary>
/// Сервис для проведения оплаты бронирований.
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Провести гарантированную оплату последнего созданного бронирования.
    /// </summary>
    Task<BookingModel> PayNoopAsync();

    /// <summary>
    /// Провести оплату последнего созданного бронирования с шансом 50%.
    /// </summary>
    Task<BookingModel> PayRandomAsync();
}
