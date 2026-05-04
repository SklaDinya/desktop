using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_DA_component.Dtos;

namespace SklaDinya_desktop_DA_component.Mapping;

internal static partial class Mapper
{
    // ── DA → BL ────────────────────────────────────────────────────────────

    public static PriceModel ToPrice(PriceDto dto)
    {
        var cellClass = RequireNonEmpty(dto.CellClass, "price.cellClass");

        return new PriceModel
        {
            StorageId = dto.StorageId,
            CellClass = cellClass,
            Price = dto.Price,
            CreatedAt = dto.CreatedAt,
        };
    }

    public static List<PriceModel> ToPriceList(List<PriceDto> list) =>
        list.Select(ToPrice).ToList();

    // ── BL → DA ────────────────────────────────────────────────────────────

    public static PriceCreateRequest ToPriceCreateRequest(PriceCreateForm form) =>
        new(form.CellClass, form.Price);
}
