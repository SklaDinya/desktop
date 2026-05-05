using System.Globalization;
using System.Text;

namespace SklaDinya_desktop_DA_component.Http;

/// <summary>
/// Простой потокобезопасный файловый логгер.
/// Пишет в файл logs/app-YYYY-MM-DD.log в рабочей директории приложения.
/// Используется для записи стеков исключений и контекста запросов к API.
/// </summary>
public static class FileLogger
{
    private static readonly object _sync = new();
    private static readonly string _logDir =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");

    /// <summary>Полный путь к актуальному лог-файлу (на текущую дату).</summary>
    public static string CurrentLogPath =>
        Path.Combine(_logDir,
            $"app-{DateTime.Now:yyyy-MM-dd}.log");

    /// <summary>Записать произвольное информационное сообщение.</summary>
    public static void Info(string message)
        => Write("INFO", message, exception: null, context: null);

    /// <summary>
    /// Записать ошибку с произвольным контекстом.
    /// </summary>
    /// <param name="message">Краткое описание операции.</param>
    /// <param name="exception">Исключение (для стека).</param>
    /// <param name="context">
    /// Доп. данные: метод, URL, тело запроса, статус ответа, тело ответа и т.п.
    /// </param>
    public static void Error(
        string message,
        Exception? exception = null,
        IReadOnlyDictionary<string, string?>? context = null)
        => Write("ERROR", message, exception, context);

    private static void Write(
        string level,
        string message,
        Exception? exception,
        IReadOnlyDictionary<string, string?>? context)
    {
        try
        {
            Directory.CreateDirectory(_logDir);

            var sb = new StringBuilder();
            sb.Append('[')
              .Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff",
                  CultureInfo.InvariantCulture))
              .Append("] [").Append(level).Append("] ")
              .AppendLine(message);

            if (context is { Count: > 0 })
            {
                sb.AppendLine("  Контекст:");
                foreach (var kv in context)
                {
                    var val = kv.Value ?? "<null>";
                    // Многострочные значения (например, тело JSON) — с отступом
                    if (val.Contains('\n'))
                    {
                        sb.Append("    ").Append(kv.Key).AppendLine(":");
                        foreach (var line in val.Split('\n'))
                            sb.Append("      ").AppendLine(line.TrimEnd('\r'));
                    }
                    else
                    {
                        sb.Append("    ").Append(kv.Key).Append(": ")
                          .AppendLine(val);
                    }
                }
            }

            if (exception is not null)
            {
                sb.AppendLine("  Исключение:");
                sb.Append("    Type: ").AppendLine(exception.GetType().FullName);
                sb.Append("    Message: ").AppendLine(exception.Message);
                sb.AppendLine("    StackTrace:");
                foreach (var line in (exception.ToString() ?? string.Empty)
                             .Split('\n'))
                    sb.Append("      ").AppendLine(line.TrimEnd('\r'));
            }

            sb.AppendLine(new string('-', 80));

            lock (_sync)
            {
                File.AppendAllText(CurrentLogPath, sb.ToString(), Encoding.UTF8);
            }
        }
        catch
        {
            // Логгер ни в коем случае не должен ронять приложение
        }
    }
}
