namespace SklaDinya_desktop_BL_component.Interfaces.Logging;

/// <summary>
/// Контракт прикладного логгера. Объявлен в BL, чтобы UI и DA
/// могли пользоваться им, не завися друг от друга.
/// </summary>
public interface IAppLogger
{
    /// <summary>Информационное сообщение.</summary>
    void Info(string message);

    /// <summary>
    /// Ошибка с произвольным контекстом (метод, URL, тело запроса и т.п.)
    /// и опциональным исключением для стека.
    /// </summary>
    void Error(
        string message,
        Exception? exception = null,
        IReadOnlyDictionary<string, string?>? context = null);
}
