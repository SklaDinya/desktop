using SklaDinya_desktop_BL_component.Exceptions;
using SklaDinya_desktop_DA_component.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SklaDinya_desktop_DA_component.Converters;

internal sealed class BookingStatusJsonConverter : JsonConverter<BookingStatusDto>
{
    public override BookingStatusDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new ServerException(
                $"Ожидалась строка для BookingStatus, получен токен {reader.TokenType}.");

        var raw = reader.GetString();
        if (string.IsNullOrWhiteSpace(raw))
            throw new ServerException("Поле status пустое или null в ответе API.");

        // Нормализация: и "Cancelled", и "Canceled" → BookingStatusDto.Canceled.
        var normalized = raw.Trim();
        if (normalized.Equals("Cancelled", StringComparison.OrdinalIgnoreCase))
            return BookingStatusDto.Canceled;

        if (Enum.TryParse<BookingStatusDto>(normalized, ignoreCase: true, out var parsed))
            return parsed;

        throw new ServerException(
            $"Не удалось разобрать статус бронирования: '{raw}'.");
    }

    public override void Write(Utf8JsonWriter writer, BookingStatusDto value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
