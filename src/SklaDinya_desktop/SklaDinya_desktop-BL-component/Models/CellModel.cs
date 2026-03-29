namespace SklaDinya_desktop_BL_component.Models;

/// <summary>
/// Данные о камере хранения (ячейке)
/// </summary>
public class CellModel
{
    /// <summary>Идентификатор ячейки</summary>
    public Guid Id { get; set; }

    /// <summary>Идентификатор пункта хранения</summary>
    public Guid StorageId { get; set; }

    /// <summary>Название ячейки</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Класс ячейки</summary>
    public string CellClass { get; set; } = string.Empty;

    /// <summary>Время создания</summary>
    public DateTime CreatedAt { get; set; }
}
