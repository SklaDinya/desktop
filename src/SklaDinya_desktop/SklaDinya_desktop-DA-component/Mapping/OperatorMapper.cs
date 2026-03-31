using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_DA_component.Dtos;

namespace SklaDinya_desktop_DA_component.Mapping;

internal static partial class Mapper
{
    public static OperatorModel ToOperator(OperatorDto dto)
    {
        var username = RequireNonEmpty(dto.Username, "operator.username");
        var name     = RequireNonEmpty(dto.Name,     "operator.name");

        return new OperatorModel
        {
            Id        = dto.Id,
            Username  = username,
            Name      = name,
            Email     = dto.Email,
            Role      = dto.Role,
            Banned    = dto.Banned,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt,
        };
    }

    public static List<OperatorModel> ToOperatorList(List<OperatorDto> list) =>
        list.Select(ToOperator).ToList();
}
