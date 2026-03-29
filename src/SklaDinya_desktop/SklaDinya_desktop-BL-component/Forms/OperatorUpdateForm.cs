using SklaDinya_desktop_BL_component.Enums;

namespace SklaDinya_desktop_BL_component.Forms;

/// <summary>
/// Форма для обновления данных оператора пункта хранения
/// </summary>
public class OperatorUpdateForm
{
    /// <summary>Логин оператора (необязательно)</summary>
    public string? Username { get; set; }

    /// <summary>Пароль оператора (необязательно)</summary>
    public string? Password { get; set; }

    /// <summary>Имя оператора (необязательно)</summary>
    public string? Name { get; set; }

    /// <summary>Почта оператора (необязательно)</summary>
    public string? Email { get; set; }

    /// <summary>Роль оператора (необязательно)</summary>
    public OperatorRole? Role { get; set; }

    /// <summary>Статус блокировки (необязательно)</summary>
    public bool? Banned { get; set; }
}
