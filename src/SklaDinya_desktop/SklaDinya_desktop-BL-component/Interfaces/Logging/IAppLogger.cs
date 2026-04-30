namespace SklaDinya_desktop_BL_component.Interfaces.Logging;

/// <summary>
/// Контракт прикладного логгера. Объявлен в BL, чтобы UI и DA
/// могли пользоваться им, не завися друг от друга.
/// Конкретная реализация (запись в файл, консоль и т.п.) живёт в DA
/// или в Main и передаётся в UI через <see cref="ServiceLocator"/>.
/// </summary>
public interface IAppLogger
{
    /// <summary>Информационное сообщение.</summary>
    void Info(string message);

    /// <summary>
    /// Ошибка с произвольным контекстом (метод, URL, тело запроса и т.п.)
    /// и опциональным исключением для стека.
    /// </summary>
    /// <param name="message">Краткое описание операции.</param>
    /// <param name="exception">Исключение (опционально).</param>
    /// <param name="context">
    /// Доп. данные для лога — например, метод, URL, тело запроса/ответа.
    /// </param>
    void Error(
        string message,
        Exception? exception = null,
        IReadOnlyDictionary<string, string?>? context = null);
}
