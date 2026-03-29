using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_DA_component.Dtos;

namespace SklaDinya_desktop_DA_component.Mapping;

internal static partial class Mapper
{
    public static OperatorModel ToOperator(OperatorDto? dto)
    {
        EnsureNotNull(dto, "OperatorDto");
        return new OperatorModel
        {
            Id        = ParseGuid(dto!.Id,            "operator.id"),
            Username  = RequireString(dto.Username,   "operator.username"),
            Name      = RequireString(dto.Name,       "operator.name"),
            Email     = dto.Email,
            Role      = ParseEnum<OperatorRole>(dto.Role, "operator.role"),
            Banned    = RequireBool(dto.Banned,           "operator.banned"),
            CreatedAt = ParseDateTime(dto.CreatedAt,      "operator.createdAt"),
            UpdatedAt = ParseDateTime(dto.UpdatedAt,      "operator.updatedAt"),
        };
    }

    public static List<OperatorModel> ToOperatorList(List<OperatorDto>? list)
    {
        EnsureNotNull(list, "Operator[]");
        return list!.Select(ToOperator).ToList();
    }
}
