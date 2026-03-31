using SklaDinya_desktop_BL_component.Exceptions;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_DA_component.Dtos;

namespace SklaDinya_desktop_DA_component.Mapping;

internal static partial class Mapper
{
    public static MeModel ToMe(MeDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Username))
            throw new ServerException("Поле 'me.username' пустое в ответе API.");
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ServerException("Поле 'me.name' пустое в ответе API.");

        return new MeModel
        {
            Id       = dto.Id,
            Username = dto.Username,
            Name     = dto.Name,
            Email    = dto.Email,
            Role     = dto.Role,
        };
    }

    public static UserModel ToUser(UserDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Username))
            throw new ServerException("Поле 'user.username' пустое в ответе API.");
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ServerException("Поле 'user.name' пустое в ответе API.");

        return new UserModel
        {
            Id        = dto.Id,
            Username  = dto.Username,
            Name      = dto.Name,
            Email     = dto.Email,
            Role      = dto.Role,
            Banned    = dto.Banned,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt,
        };
    }

    public static List<UserModel> ToUserList(List<UserDto> list) =>
        list.Select(ToUser).ToList();
}
