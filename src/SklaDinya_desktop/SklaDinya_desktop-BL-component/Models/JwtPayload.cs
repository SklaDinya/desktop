using SklaDinya_desktop_BL_component.Enums;

namespace SklaDinya_desktop_BL_component.Models;

/// <summary>
/// Данные, извлечённые из payload JWT-токена.
/// Поля соответствуют тому, что кладёт бэкенд:
///   userId, userRole — для всех пользователей;
///   storageId, role  — дополнительно для операторов.
/// </summary>
public class JwtPayload
{
    /// <summary>Идентификатор пользователя</summary>
    public Guid UserId { get; init; }

    /// <summary>Роль пользователя в системе</summary>
    public UserRole UserRole { get; init; }

    /// <summary>Идентификатор пункта хранения (только для операторов)</summary>
    public Guid? StorageId { get; init; }

    /// <summary>Роль оператора внутри пункта хранения (только для операторов)</summary>
    public OperatorRole? OperatorRole { get; init; }
}
