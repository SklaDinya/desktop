using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_DA_component.Dtos;

namespace SklaDinya_desktop_DA_component.Mapping;

internal static partial class Mapper
{
    public static CellModel ToCell(CellDto? dto)
    {
        EnsureNotNull(dto, "CellDto");
        return new CellModel
        {
            Id        = ParseGuid(dto!.Id,           "cell.id"),
            StorageId = ParseGuid(dto.StorageId,     "cell.storageId"),
            Name      = RequireString(dto.Name,      "cell.name"),
            CellClass = RequireString(dto.CellClass, "cell.cellClass"),
            CreatedAt = ParseDateTime(dto.CreatedAt, "cell.createdAt"),
        };
    }

    public static List<CellModel> ToCellList(List<CellDto>? list)
    {
        EnsureNotNull(list, "Cell[]");
        return list!.Select(ToCell).ToList();
    }

    public static List<string> ToCellClasses(List<string>? list)
    {
        EnsureNotNull(list, "CellClasses[]");
        return list!;
    }
}
