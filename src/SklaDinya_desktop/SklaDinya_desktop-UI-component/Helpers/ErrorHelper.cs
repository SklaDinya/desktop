using SklaDinya_desktop_BL_component.Exceptions;

namespace SklaDinya_desktop_UI_component.Helpers;

/// <summary>
/// Обёртка для безопасного выполнения асинхронных операций с показом ошибок пользователю
/// </summary>
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
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        catch (Exception ex)
        {
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
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return null;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Непредвиденная ошибка: {ex.Message}", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }
    }
}
