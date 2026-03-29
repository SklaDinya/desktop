namespace SklaDinya_desktop_BL_component.Exceptions;

/// <summary>
/// Пользователь не авторизован (HTTP 401)
/// </summary>
public class UnauthorizedException : ApiException
{
    public UnauthorizedException()
        : base(401, "Пользователь не авторизован.") { }

    public UnauthorizedException(string message)
        : base(401, message) { }
}
