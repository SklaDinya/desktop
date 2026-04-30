using SklaDinya_desktop_BL_component.Exceptions;

namespace SklaDinya_desktop_UI_component.Helpers;

/// <summary>
/// Обёртка для безопасного выполнения асинхронных операций с показом ошибок пользователю.
/// При любом исключении пишет полную информацию через <see cref="ServiceLocator.Logger"/>,
/// который реализован в DA-компоненте, но используется здесь только через интерфейс из BL.
/// </summary>
/// <remarks>
/// Сигнатуры публичных методов (число и порядок параметров) намеренно совпадают с
/// исходным <see cref="ErrorHelper"/>. Соблазн добавить
/// <see cref="System.Runtime.CompilerServices.CallerMemberNameAttribute"/> и
/// <see cref="System.Runtime.CompilerServices.CallerFilePathAttribute"/>
/// сюда сильный, но добавление таких параметров со значениями по умолчанию
/// сместит разрешение перегрузок: компилятор начнёт выбирать generic-вариант
/// для вызовов вида <c>TryAsync(() =&gt; SomethingAsync(), "Готово.")</c>,
/// и появятся CS0029 в местах, где ожидается <see cref="bool"/>. Поэтому
/// caller-информацию мы получаем из <see cref="System.Diagnostics.StackTrace"/>
/// внутри метода — это совместимо с существующими вызовами и не ломает сборку.
/// </remarks>
public static class ErrorHelper
{
    /// <summary>
    /// Выполнить асинхронную операцию, перехватив типизированные исключения API.
    /// При ошибке показывает MessageBox и возвращает false.
    /// </summary>
    public static async Task<bool> TryAsync(Func<Task> action, string? successMessage = null)
    {
        try
        {
            await action();
            if (successMessage is not null)
                MessageBox.Show(successMessage, "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }
        catch (ApiException ex)
        {
            LogUiError(ex);
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        catch (Exception ex)
        {
            LogUiError(ex);
            MessageBox.Show($"Непредвиденная ошибка: {ex.Message}", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
    }

    /// <summary>
    /// Выполнить асинхронную операцию с результатом.
    /// </summary>
    public static async Task<T?> TryAsync<T>(Func<Task<T>> action) where T : class
    {
        try
        {
            return await action();
        }
        catch (ApiException ex)
        {
            LogUiError(ex);
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return null;
        }
        catch (Exception ex)
        {
            LogUiError(ex);
            MessageBox.Show($"Непредвиденная ошибка: {ex.Message}", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }
    }

    /// <summary>
    /// Записать ошибку в лог. Caller-информацию достаём из стека вызовов,
    /// чтобы не менять сигнатуру публичных методов.
    /// </summary>
    private static void LogUiError(Exception ex)
    {
        var (caller, file) = ResolveCaller();
        // ServiceLocator.Logger — IAppLogger из BL. UI к DA напрямую не обращается.
        ServiceLocator.Logger.Error(
            $"Ошибка в UI-операции ({caller})",
            ex,
            new Dictionary<string, string?>
            {
                ["CallerMember"] = caller,
                ["CallerFile"] = file,
            });
    }

    /// <summary>
    /// Достаёт первый кадр стека, который не принадлежит самому
    /// <see cref="ErrorHelper"/> — это и есть UI-метод, вызвавший
    /// <c>TryAsync</c>. Полностью без изменения сигнатур публичных методов,
    /// поэтому не ломает разрешение перегрузок у вызывающего кода.
    /// </summary>
    private static (string Member, string File) ResolveCaller()
    {
        try
        {
            var stack = new System.Diagnostics.StackTrace(fNeedFileInfo: true);
            for (int i = 0; i < stack.FrameCount; i++)
            {
                var frame = stack.GetFrame(i);
                var method = frame?.GetMethod();
                if (method is null) continue;
                var declaringType = method.DeclaringType;
                if (declaringType == typeof(ErrorHelper)) continue;

                // Пропускаем компилятор-сгенерированные state machine async-методов:
                // у них имена вида "<MoveNext>g__SomeName|0_0".
                var typeName = declaringType?.FullName ?? "<unknown>";
                if (typeName.Contains('+') &&
                    method.Name is "MoveNext" or "InvokeStub")
                    continue;

                var member = method.Name;
                var file = frame?.GetFileName() ?? string.Empty;
                return ($"{declaringType?.Name}.{member}", file);
            }
        }
        catch
        {
            // Логгер не должен ронять приложение — отдадим заглушку.
        }
        return ("<unknown>", string.Empty);
    }
}