namespace SklaDinya_desktop_BL_component.Exceptions;

/// <summary>
/// Сервер работает в режиме только для чтения (HTTP 405)
/// </summary>
public class ReadOnlyModeException : ApiException
{
    public ReadOnlyModeException()
        : base(405, "Сервер запущен в режиме только для чтения.") { }

    public ReadOnlyModeException(string message)
        : base(405, message) { }
}
