namespace SklaDinya_desktop_BL_component.Models;

/// <summary>
/// Краткие данные о пользователе, отображаемые при получении бронирования оператором
/// </summary>
public class BookingUserModel
{
    /// <summary>Идентификатор пользователя</summary>
    public Guid Id { get; set; }

    /// <summary>Имя пользователя</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Почта пользователя</summary>
    public string? Email { get; set; }
}
