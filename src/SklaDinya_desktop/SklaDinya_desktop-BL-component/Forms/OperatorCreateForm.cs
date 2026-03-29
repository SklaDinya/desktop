using SklaDinya_desktop_BL_component.Enums;

namespace SklaDinya_desktop_BL_component.Forms;

/// <summary>
/// Форма для создания оператора пункта хранения
/// </summary>
public class OperatorCreateForm
{
    /// <summary>Логин оператора</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>Пароль оператора</summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>Имя оператора</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Почта оператора (необязательно)</summary>
    public string? Email { get; set; }

    /// <summary>Роль оператора</summary>
    public OperatorRole Role { get; set; }
}
