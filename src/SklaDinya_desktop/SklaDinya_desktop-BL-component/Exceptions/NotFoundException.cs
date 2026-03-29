namespace SklaDinya_desktop_BL_component.Exceptions;

/// <summary>
/// Запрашиваемый ресурс не найден (HTTP 404)
/// </summary>
public class NotFoundException : ApiException
{
    public NotFoundException()
        : base(404, "Ресурс не найден.") { }

    public NotFoundException(string message)
        : base(404, message) { }
}
