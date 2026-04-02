using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_DA_component.Dtos;
using SklaDinya_desktop_DA_component.Enums;

namespace SklaDinya_desktop_DA_component.Mapping;

internal static partial class Mapper
{
    // ── DA → BL ────────────────────────────────────────────────────────────

    public static UserModel ToUser(UserDto dto) => new()
    {
        Id        = dto.Id,
        Username  = dto.Username,
        Name      = dto.Name,
        Email     = dto.Email,
        Role      = ToUserRole(dto.Role),
        Banned    = dto.Banned,
        CreatedAt = dto.CreatedAt,
        UpdatedAt = dto.UpdatedAt,
    };

    public static List<UserModel> ToUserList(List<UserDto> dtos) =>
        dtos.Select(ToUser).ToList();

    public static MeModel ToMe(MeDto dto) => new()
    {
        Id       = dto.Id,
        Username = dto.Username,
        Name     = dto.Name,
        Email    = dto.Email,
        Role     = ToUserRole(dto.Role),
    };

    // ── BL → DA ────────────────────────────────────────────────────────────

    public static UserCreateRequest ToUserCreateRequest(UserCreateForm form) =>
        new(form.Username,
            form.Password,
            form.Name,
            form.Email,
            ToUserRoleDto(form.Role));

    public static UserUpdateRequest ToUserUpdateRequest(UserUpdateForm form) =>
        new(form.Username, form.Password, form.Name, form.Email, form.Banned);

    public static MeUpdateRequest ToMeUpdateRequest(MeUpdateForm form) =>
        new(form.Username, form.OldPassword, form.NewPassword, form.Name, form.Email);

    public static UserRoleDto ToUserRoleDto(UserRole role) => role switch
    {
        UserRole.Client          => UserRoleDto.Client,
        UserRole.StorageOperator => UserRoleDto.StorageOperator,
        UserRole.Admin           => UserRoleDto.Admin,
        _ => throw new ArgumentOutOfRangeException(nameof(role), $"Неизвестный UserRole: {role}")
    };

    // ── Внутренние конвертеры ───────────────────────────────────────────────

    private static UserRole ToUserRole(UserRoleDto role) => role switch
    {
        UserRoleDto.Client          => UserRole.Client,
        UserRoleDto.StorageOperator => UserRole.StorageOperator,
        UserRoleDto.Admin           => UserRole.Admin,
        _ => throw new ArgumentOutOfRangeException(nameof(role), $"Неизвестный UserRoleDto: {role}")
    };
}
