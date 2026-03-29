using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_DA_component.Dtos;

namespace SklaDinya_desktop_DA_component.Mapping;

internal static partial class Mapper
{
    public static PriceModel ToPrice(PriceDto? dto)
    {
        EnsureNotNull(dto, "PriceDto");
        return new PriceModel
        {
            StorageId = ParseGuid(dto!.StorageId,    "price.storageId"),
            CellClass = RequireString(dto.CellClass, "price.cellClass"),
            Price     = ParseDecimal(dto.Price,      "price.price"),
            CreatedAt = ParseDateTime(dto.CreatedAt, "price.createdAt"),
        };
    }

    public static List<PriceModel> ToPriceList(List<PriceDto>? list)
    {
        EnsureNotNull(list, "Price[]");
        return list!.Select(ToPrice).ToList();
    }
}
