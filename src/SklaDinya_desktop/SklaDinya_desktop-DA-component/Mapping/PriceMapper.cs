using SklaDinya_desktop_BL_component.Exceptions;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_DA_component.Dtos;
using System.Globalization;

namespace SklaDinya_desktop_DA_component.Mapping;

internal static partial class Mapper
{
    // ── DA → BL ────────────────────────────────────────────────────────────

    public static PriceModel ToPrice(PriceDto dto)
    {
        var cellClass = RequireNonEmpty(dto.CellClass, "price.cellClass");
        var price = ParseDecimal(dto.Price, "price.price");

        return new PriceModel
        {
            StorageId = dto.StorageId,
            CellClass = cellClass,
            Price = price,
            CreatedAt = dto.CreatedAt,
        };
    }

    public static List<PriceModel> ToPriceList(List<PriceDto> list) =>
        list.Select(ToPrice).ToList();

    // ── BL → DA ────────────────────────────────────────────────────────────

    public static PriceCreateRequest ToPriceCreateRequest(PriceCreateForm form) =>
        new(form.CellClass, form.Price.ToString(CultureInfo.InvariantCulture));

    // ── Вспомогательные методы ─────────────────────────────────────────────

    private static decimal ParseDecimal(string value, string field)
    {
        if (!decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
            throw new ServerException($"Поле '{field}' содержит некорректное число: '{value}'.");

        return result;
    }
}
