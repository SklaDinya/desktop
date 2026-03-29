using SklaDinya_desktop_BL_component.Exceptions;

namespace SklaDinya_desktop_DA_component.Mapping;

/// <summary>
/// Базовые guard-методы для валидации полей ответа API.
/// Используются всеми частичными классами маппера.
/// </summary>
internal static partial class Mapper
{
    private static void EnsureNotNull<T>(T? value, string name) where T : class
    {
        if (value is null)
            throw new ServerException($"Сервер вернул null вместо {name}.");
    }

    private static string RequireString(string? value, string field)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ServerException($"Поле '{field}' пустое или null в ответе API.");
        return value;
    }

    private static bool RequireBool(bool? value, string field)
    {
        if (!value.HasValue)
            throw new ServerException($"Поле '{field}' равно null в ответе API.");
        return value.Value;
    }

    private static Guid ParseGuid(string? value, string field)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ServerException($"Поле '{field}' пустое или null в ответе API.");

        if (!Guid.TryParse(value, out var guid))
            throw new ServerException(
                $"Поле '{field}' содержит некорректный GUID: '{value}'.");

        return guid;
    }

    private static DateTime ParseDateTime(string? value, string field)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ServerException($"Поле '{field}' пустое или null в ответе API.");

        if (!DateTime.TryParse(value, null,
                System.Globalization.DateTimeStyles.RoundtripKind, out var dt))
            throw new ServerException(
                $"Поле '{field}' содержит некорректную дату: '{value}'.");

        return dt;
    }

    private static TimeSpan ParseDuration(string? value, string field)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ServerException($"Поле '{field}' пустое или null в ответе API.");

        try { return System.Xml.XmlConvert.ToTimeSpan(value); }
        catch
        {
            throw new ServerException(
                $"Поле '{field}' содержит некорректную ISO 8601 длительность: '{value}'.");
        }
    }

    private static decimal ParseDecimal(string? value, string field)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ServerException($"Поле '{field}' пустое или null в ответе API.");

        if (!decimal.TryParse(value,
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out var dec))
            throw new ServerException(
                $"Поле '{field}' содержит некорректное число: '{value}'.");

        return dec;
    }

    private static TEnum ParseEnum<TEnum>(string? value, string field)
        where TEnum : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ServerException($"Поле '{field}' пустое или null в ответе API.");

        if (!Enum.TryParse<TEnum>(value, out var result))
            throw new ApiException(0,
                $"Поле '{field}' содержит неизвестное значение '{value}'. " +
                "Возможно, API обновился — обновите приложение.");

        return result;
    }
}
