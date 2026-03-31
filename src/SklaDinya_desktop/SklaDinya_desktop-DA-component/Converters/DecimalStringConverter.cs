using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using SklaDinya_desktop_BL_component.Exceptions;

namespace SklaDinya_desktop_DA_component.Converters;

/// <summary>
/// Конвертирует decimal128 (приходит как JSON-строка) ↔ <see cref="decimal"/>.
/// API возвращает поле price в формате decimal128 — строкой вида "993".
/// </summary>
internal sealed class DecimalStringConverter : JsonConverter<decimal>
{
    public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
            return reader.GetDecimal();

        var value = reader.GetString();

        if (string.IsNullOrWhiteSpace(value))
            throw new ServerException("Поле decimal пустое или null в ответе API.");

        if (!decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
            throw new ServerException($"Не удалось разобрать decimal: '{value}'.");

        return result;
    }

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(CultureInfo.InvariantCulture));
    }
}
