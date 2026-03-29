namespace SklaDinya_desktop_BL_component.Forms;

/// <summary>
/// Форма для создания камеры хранения (ячейки)
/// </summary>
public class CellCreateForm
{
    /// <summary>Название ячейки</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Класс ячейки</summary>
    public string CellClass { get; set; } = string.Empty;
}
