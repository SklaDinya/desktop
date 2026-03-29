namespace SklaDinya_desktop_BL_component.Exceptions;

/// <summary>
/// Доступ запрещён — недостаточно прав или аккаунт заблокирован (HTTP 403)
/// </summary>
public class ForbiddenException : ApiException
{
    public ForbiddenException()
        : base(403, "Доступ запрещён.") { }

    public ForbiddenException(string message)
        : base(403, message) { }
}
