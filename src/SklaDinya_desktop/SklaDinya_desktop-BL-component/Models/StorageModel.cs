using SklaDinya_desktop_BL_component.Enums;

namespace SklaDinya_desktop_BL_component.Models;

/// <summary>
/// Данные о пункте хранения
/// </summary>
public class StorageModel
{
    /// <summary>Идентификатор пункта хранения</summary>
    public Guid Id { get; set; }

    /// <summary>Название пункта хранения</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Адрес пункта хранения</summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>Описание пункта хранения</summary>
    public string? Description { get; set; }

    /// <summary>Статус пункта хранения</summary>
    public StorageStatus Status { get; set; }

    /// <summary>Время создания</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Время последнего обновления</summary>
    public DateTime UpdatedAt { get; set; }
}
