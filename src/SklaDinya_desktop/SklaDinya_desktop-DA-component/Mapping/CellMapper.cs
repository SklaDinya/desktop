using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_DA_component.Dtos;

namespace SklaDinya_desktop_DA_component.Mapping;

internal static partial class Mapper
{
    // ── DA → BL ────────────────────────────────────────────────────────────

    public static CellModel ToCell(CellDto dto)
    {
        var name = RequireNonEmpty(dto.Name, "cell.name");
        var cellClass = RequireNonEmpty(dto.CellClass, "cell.cellClass");

        return new CellModel
        {
            Id = dto.Id,
            StorageId = dto.StorageId,
            Name = name,
            CellClass = cellClass,
            CreatedAt = dto.CreatedAt,
        };
    }

    public static List<CellModel> ToCellList(List<CellDto> list) =>
        list.Select(ToCell).ToList();

    // ── BL → DA ────────────────────────────────────────────────────────────

    public static CellCreateRequest ToCellCreateRequest(CellCreateForm form) =>
        new(form.Name, form.CellClass);
}
