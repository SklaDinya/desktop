using SklaDinya_desktop_BL_component.Exceptions;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;

namespace SklaDinya_desktop_DA_component.Converters;

/// <summary>
/// Конвертирует ISO 8601 duration (P3D, PT2H30M и т.п.) ↔ <see cref="TimeSpan"/>.
/// API использует формат duration для полей bookingTime и timeBooking.
/// </summary>
internal sealed class TimeSpanIso8601Converter : JsonConverter<TimeSpan>
{
    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();

        if (string.IsNullOrWhiteSpace(value))
            throw new ServerException("Поле duration пустое или null в ответе API.");

        try
        {
            return XmlConvert.ToTimeSpan(value);
        }
        catch
        {
            throw new ServerException(
                $"Не удалось разобрать ISO 8601 duration: '{value}'.");
        }
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(XmlConvert.ToString(value));
    }
}
