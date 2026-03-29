namespace SklaDinya_desktop_BL_component.Exceptions;

/// <summary>
/// Внутренняя ошибка сервера (HTTP 500)
/// </summary>
public class ServerException : ApiException
{
    public ServerException()
        : base(500, "Внутренняя ошибка сервера.") { }

    public ServerException(string message)
        : base(500, message) { }
}
