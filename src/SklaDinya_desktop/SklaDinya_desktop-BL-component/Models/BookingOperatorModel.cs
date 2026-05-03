using SklaDinya_desktop_BL_component.Enums;

namespace SklaDinya_desktop_BL_component.Models;

/// <summary>
/// Данные о бронировании для оператора пункта хранения
/// </summary>
public class BookingOperatorModel
{
    /// <summary>Идентификатор бронирования</summary>
    public Guid Id { get; set; }

    /// <summary>Идентификатор пользователя</summary>
    public Guid UserId { get; set; }

    /// <summary>Краткие данные о пользователе</summary>
    public BookingUserModel User { get; set; } = null!;

    /// <summary>Идентификатор пункта хранения</summary>
    public Guid StorageId { get; set; }

    /// <summary>Список забронированных ячеек</summary>
    public List<CellModel> Cells { get; set; } = [];

    /// <summary>Время начала бронирования</summary>
    public DateTime StartTime { get; set; }

    /// <summary>Длительность бронирования</summary>
    public TimeSpan BookingTime { get; set; }

    /// <summary>
    /// Итоговая стоимость бронирования. Считается на бэкенде —
    /// клиент не пересчитывает.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>Время создания бронирования</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Статус бронирования</summary>
    public BookingStatus Status { get; set; }
}
