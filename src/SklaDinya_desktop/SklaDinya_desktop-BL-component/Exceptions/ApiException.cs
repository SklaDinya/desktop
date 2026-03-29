namespace SklaDinya_desktop_BL_component.Exceptions;

/// <summary>
/// Исключение, возникающее при получении ошибки от API бэкенда
/// </summary>
public class ApiException : Exception
{
    /// <summary>HTTP-статус код ответа</summary>
    public int StatusCode { get; }

    public ApiException(int statusCode, string message)
        : base(message)
    {
        StatusCode = statusCode;
    }

    public ApiException(int statusCode, string message, Exception innerException)
        : base(message, innerException)
    {
        StatusCode = statusCode;
    }
}
