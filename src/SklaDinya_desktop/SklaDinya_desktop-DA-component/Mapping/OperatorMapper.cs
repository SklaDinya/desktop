using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_DA_component.Dtos;
using SklaDinya_desktop_DA_component.Enums;

namespace SklaDinya_desktop_DA_component.Mapping;

internal static partial class Mapper
{
    // ── DA → BL ────────────────────────────────────────────────────────────

    public static OperatorModel ToOperator(OperatorDto dto) => new()
    {
        Id        = dto.Id,
        Username  = dto.Username,
        Name      = dto.Name,
        Email     = dto.Email,
        Role      = ToOperatorRole(dto.Role),
        Banned    = dto.Banned,
        CreatedAt = dto.CreatedAt,
        UpdatedAt = dto.UpdatedAt,
    };

    public static List<OperatorModel> ToOperatorList(List<OperatorDto> dtos) =>
        dtos.Select(ToOperator).ToList();

    // ── BL → DA ────────────────────────────────────────────────────────────

    public static OperatorRoleDto ToOperatorRoleDto(OperatorRole role) => role switch
    {
        OperatorRole.MainOperator     => OperatorRoleDto.MainOperator,
        OperatorRole.OrdinaryOperator => OperatorRoleDto.OrdinaryOperator,
        _ => throw new ArgumentOutOfRangeException(nameof(role), $"Неизвестный OperatorRole: {role}")
    };

    // ── Внутренние конвертеры ───────────────────────────────────────────────

    private static OperatorRole ToOperatorRole(OperatorRoleDto role) => role switch
    {
        OperatorRoleDto.MainOperator     => OperatorRole.MainOperator,
        OperatorRoleDto.OrdinaryOperator => OperatorRole.OrdinaryOperator,
        _ => throw new ArgumentOutOfRangeException(nameof(role), $"Неизвестный OperatorRoleDto: {role}")
    };
}
