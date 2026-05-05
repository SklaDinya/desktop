using SklaDinya_desktop_BL_component.Exceptions;

namespace SklaDinya_desktop_DA_component.Mapping;

/// <summary>
/// Вспомогательные методы, общие для всех частей маппера.
/// </summary>
internal static partial class Mapper
{
    /// <summary>
    /// Проверяет что строка не пустая и возвращает её.
    /// </summary>
    private static string RequireNonEmpty(string value, string field)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ServerException($"Поле '{field}' пустое в ответе API.");

        return value;
    }
}
