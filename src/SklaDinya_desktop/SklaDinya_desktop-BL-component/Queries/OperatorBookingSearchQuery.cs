using SklaDinya_desktop_BL_component.Enums;

namespace SklaDinya_desktop_BL_component.Queries;

/// <summary>
/// Параметры поиска бронирований операторов пункта хранения
/// </summary>
public class OperatorBookingSearchQuery : PageQuery
{
    /// <summary>Начало диапазона поиска бронирований</summary>
    public DateTime StartBooking { get; set; }

    /// <summary>Конец диапазона поиска бронирований</summary>
    public DateTime EndBooking { get; set; }

    /// <summary>Фильтр по статусам бронирований (необязательно)</summary>
    public List<BookingStatus>? Statuses { get; set; }
}
