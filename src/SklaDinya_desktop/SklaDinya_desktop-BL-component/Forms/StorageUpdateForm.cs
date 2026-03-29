namespace SklaDinya_desktop_BL_component.Forms;

/// <summary>
/// Форма обновления данных пункта хранения
/// </summary>
public class StorageUpdateForm
{
    /// <summary>Новое название пункта хранения (необязательно)</summary>
    public string? Name { get; set; }

    /// <summary>Новый адрес пункта хранения (необязательно)</summary>
    public string? Address { get; set; }

    /// <summary>Новое описание пункта хранения (необязательно)</summary>
    public string? Description { get; set; }
}
