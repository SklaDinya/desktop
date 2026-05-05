using SklaDinya_desktop_BL_component.Enums;

namespace SklaDinya_desktop_BL_component.Models;

/// <summary>
/// Данные об операторе пункта хранения
/// </summary>
public class OperatorModel
{
    /// <summary>Идентификатор оператора</summary>
    public Guid Id { get; set; }

    /// <summary>Логин оператора</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>Имя оператора</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Почта оператора</summary>
    public string? Email { get; set; }

    /// <summary>Роль оператора</summary>
    public OperatorRole Role { get; set; }

    /// <summary>Заблокирован ли аккаунт</summary>
    public bool Banned { get; set; }

    /// <summary>Время создания аккаунта</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Время последнего обновления аккаунта</summary>
    public DateTime UpdatedAt { get; set; }
}
