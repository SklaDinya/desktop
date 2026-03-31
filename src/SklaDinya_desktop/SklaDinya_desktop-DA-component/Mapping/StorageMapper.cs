using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_DA_component.Dtos;
using SklaDinya_desktop_DA_component.Enums;

namespace SklaDinya_desktop_DA_component.Mapping;

internal static partial class Mapper
{
    // ── DA → BL ────────────────────────────────────────────────────────────

    public static StorageModel ToStorage(StorageDto dto) => new()
    {
        Id          = dto.Id,
        Name        = dto.Name,
        Address     = dto.Address,
        Description = dto.Description,
        Status      = ToStorageStatus(dto.Status),
        CreatedAt   = dto.CreatedAt,
        UpdatedAt   = dto.UpdatedAt,
    };

    public static List<StorageModel> ToStorageList(List<StorageDto> dtos) =>
        dtos.Select(ToStorage).ToList();

    // ── BL → DA ────────────────────────────────────────────────────────────

    public static StorageStatusDto ToStorageStatusDto(StorageStatus status) => status switch
    {
        StorageStatus.Created => StorageStatusDto.Created,
        StorageStatus.Active  => StorageStatusDto.Active,
        _ => throw new ArgumentOutOfRangeException(nameof(status), $"Неизвестный StorageStatus: {status}")
    };

    // ── Внутренние конвертеры ───────────────────────────────────────────────

    private static StorageStatus ToStorageStatus(StorageStatusDto status) => status switch
    {
        StorageStatusDto.Created => StorageStatus.Created,
        StorageStatusDto.Active  => StorageStatus.Active,
        _ => throw new ArgumentOutOfRangeException(nameof(status), $"Неизвестный StorageStatusDto: {status}")
    };
}
