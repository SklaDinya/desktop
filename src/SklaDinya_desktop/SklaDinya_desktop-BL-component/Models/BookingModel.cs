using SklaDinya_desktop_BL_component.Enums;

namespace SklaDinya_desktop_BL_component.Models;

/// <summary>
/// Данные о бронировании для пользователя (клиента)
/// </summary>
public class BookingModel
{
    /// <summary>Идентификатор бронирования</summary>
    public Guid Id { get; set; }

    /// <summary>Идентификатор пользователя</summary>
    public Guid UserId { get; set; }

    /// <summary>Идентификатор пункта хранения</summary>
    public Guid StorageId { get; set; }

    /// <summary>Данные пункта хранения</summary>
    public StorageModel Storage { get; set; } = null!;

    /// <summary>Список забронированных ячеек</summary>
    public List<CellModel> Cells { get; set; } = [];

    /// <summary>Время начала бронирования</summary>
    public DateTime StartTime { get; set; }

    /// <summary>Длительность бронирования</summary>
    public TimeSpan BookingTime { get; set; }

    /// <summary>Время создания бронирования</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Статус бронирования</summary>
    public BookingStatus Status { get; set; }
}
