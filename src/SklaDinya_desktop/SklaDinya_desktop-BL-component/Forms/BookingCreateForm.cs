namespace SklaDinya_desktop_BL_component.Forms;

/// <summary>
/// Форма создания бронирования
/// </summary>
public class BookingCreateForm
{
    /// <summary>Идентификатор пункта хранения</summary>
    public Guid StorageId { get; set; }

    /// <summary>Список идентификаторов ячеек для бронирования</summary>
    public List<Guid> CellIds { get; set; } = [];

    /// <summary>Время начала бронирования</summary>
    public DateTime StartTime { get; set; }

    /// <summary>Длительность бронирования</summary>
    public TimeSpan BookingTime { get; set; }
}
