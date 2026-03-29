namespace SklaDinya_desktop_BL_component.Exceptions;

/// <summary>
/// Конфликт данных — ресурс уже существует или занят (HTTP 409)
/// </summary>
public class ConflictException : ApiException
{
    public ConflictException()
        : base(409, "Конфликт данных.") { }

    public ConflictException(string message)
        : base(409, message) { }
}
