namespace SklaDinya_desktop_BL_component.Forms;

/// <summary>
/// Форма создания пункта хранения (вместе с главным оператором)
/// </summary>
public class StorageCreateForm
{
    /// <summary>Логин главного оператора</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>Пароль главного оператора</summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>Имя главного оператора</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Почта главного оператора</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Название пункта хранения</summary>
    public string StorageName { get; set; } = string.Empty;

    /// <summary>Адрес пункта хранения</summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>Описание пункта хранения (необязательно)</summary>
    public string? Description { get; set; }
}
