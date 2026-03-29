using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_DA_component.Dtos;

namespace SklaDinya_desktop_DA_component.Mapping;

internal static partial class Mapper
{
    public static StorageModel ToStorage(StorageDto? dto)
    {
        EnsureNotNull(dto, "StorageDto");
        return new StorageModel
        {
            Id          = ParseGuid(dto!.Id,         "storage.id"),
            Name        = RequireString(dto.Name,    "storage.name"),
            Address     = RequireString(dto.Address, "storage.address"),
            Description = dto.Description,
            Status      = ParseEnum<StorageStatus>(dto.Status,    "storage.status"),
            CreatedAt   = ParseDateTime(dto.CreatedAt, "storage.createdAt"),
            UpdatedAt   = ParseDateTime(dto.UpdatedAt, "storage.updatedAt"),
        };
    }

    public static List<StorageModel> ToStorageList(List<StorageDto>? list)
    {
        EnsureNotNull(list, "Storage[]");
        return list!.Select(ToStorage).ToList();
    }
}
