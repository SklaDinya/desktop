using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_DA_component.Dtos;

namespace SklaDinya_desktop_DA_component.Mapping;

internal static partial class Mapper
{
    public static StorageModel ToStorage(StorageDto dto)
    {
        var name    = RequireNonEmpty(dto.Name,    "storage.name");
        var address = RequireNonEmpty(dto.Address, "storage.address");

        return new StorageModel
        {
            Id          = dto.Id,
            Name        = name,
            Address     = address,
            Description = dto.Description,
            Status      = dto.Status,
            CreatedAt   = dto.CreatedAt,
            UpdatedAt   = dto.UpdatedAt,
        };
    }

    public static List<StorageModel> ToStorageList(List<StorageDto> list) =>
        list.Select(ToStorage).ToList();
}
