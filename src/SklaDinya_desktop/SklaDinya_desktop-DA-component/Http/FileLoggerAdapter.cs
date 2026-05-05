using SklaDinya_desktop_BL_component.Interfaces.Logging;

namespace SklaDinya_desktop_DA_component.Http;

/// <summary>
/// Существует, чтобы UI-компонент мог работать с логгером через интерфейс из BL,
/// не завися напрямую от пространства имён DA.
/// </summary>
public sealed class FileLoggerAdapter : IAppLogger
{
    /// <inheritdoc/>
    public void Info(string message) => FileLogger.Info(message);

    /// <inheritdoc/>
    public void Error(
        string message,
        Exception? exception = null,
        IReadOnlyDictionary<string, string?>? context = null)
        => FileLogger.Error(message, exception, context);
}
