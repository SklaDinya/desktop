using System.Text.Json.Serialization;
using SklaDinya_desktop_DA_component.Converters;

namespace SklaDinya_desktop_DA_component.Dtos;

/// <summary>Ответ API: тариф. price приходит как decimal128 (строка)</summary>
internal record PriceDto(
    Guid     StorageId,
    string   CellClass,
    [property: JsonConverter(typeof(DecimalStringConverter))]
    decimal  Price,
    DateTime CreatedAt);

internal record PriceCreateRequest(
    string  CellClass,
    [property: JsonConverter(typeof(DecimalStringConverter))]
    decimal Price);
