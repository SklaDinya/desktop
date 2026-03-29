using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_DA_component.Dtos;

namespace SklaDinya_desktop_DA_component.Mapping;

internal static partial class Mapper
{
    public static MeModel ToMe(MeDto? dto)
    {
        EnsureNotNull(dto, "MeDto");
        return new MeModel
        {
            Id       = ParseGuid(dto!.Id,          "me.id"),
            Username = RequireString(dto.Username,  "me.username"),
            Name     = RequireString(dto.Name,      "me.name"),
            Email    = dto.Email,
            Role     = ParseEnum<UserRole>(dto.Role,"me.role"),
        };
    }

    public static UserModel ToUser(UserDto? dto)
    {
        EnsureNotNull(dto, "UserDto");
        return new UserModel
        {
            Id        = ParseGuid(dto!.Id,           "user.id"),
            Username  = RequireString(dto.Username,   "user.username"),
            Name      = RequireString(dto.Name,       "user.name"),
            Email     = dto.Email,
            Role      = ParseEnum<UserRole>(dto.Role, "user.role"),
            Banned    = RequireBool(dto.Banned,       "user.banned"),
            CreatedAt = ParseDateTime(dto.CreatedAt,  "user.createdAt"),
            UpdatedAt = ParseDateTime(dto.UpdatedAt,  "user.updatedAt"),
        };
    }

    public static List<UserModel> ToUserList(List<UserDto>? list)
    {
        EnsureNotNull(list, "User[]");
        return list!.Select(ToUser).ToList();
    }
}
