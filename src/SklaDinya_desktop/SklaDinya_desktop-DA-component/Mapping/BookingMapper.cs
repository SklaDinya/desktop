using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_DA_component.Dtos;
using SklaDinya_desktop_DA_component.Enums;

namespace SklaDinya_desktop_DA_component.Mapping;

internal static partial class Mapper
{
    // ── DA → BL ────────────────────────────────────────────────────────────

    public static BookingModel ToBooking(BookingUserDto dto) => new()
    {
        Id          = dto.Id,
        UserId      = dto.UserId,
        StorageId   = dto.StorageId,
        Storage     = ToStorage(dto.Storage),
        Cells       = dto.Cells.Select(ToCell).ToList(),
        StartTime   = dto.StartTime,
        BookingTime = dto.BookingTime,
        CreatedAt   = dto.CreatedAt,
        Status      = ToBookingStatus(dto.Status),
    };

    public static List<BookingModel> ToBookingList(List<BookingUserDto> dtos) =>
        dtos.Select(ToBooking).ToList();

    public static BookingOperatorModel ToBookingOperator(BookingOperatorDto dto) => new()
    {
        Id          = dto.Id,
        UserId      = dto.UserId,
        User        = new BookingUserModel { Id = dto.User.Id, Name = dto.User.Name, Email = dto.User.Email },
        StorageId   = dto.StorageId,
        Cells       = dto.Cells.Select(ToCell).ToList(),
        StartTime   = dto.StartTime,
        BookingTime = dto.BookingTime,
        CreatedAt   = dto.CreatedAt,
        Status      = ToBookingStatus(dto.Status),
    };

    public static List<BookingOperatorModel> ToBookingOperatorList(List<BookingOperatorDto> dtos) =>
        dtos.Select(ToBookingOperator).ToList();

    public static BookingReceiptModel ToBookingReceipt(BookingReceiptDto dto) => new()
    {
        Receipt = dto.Receipt,
    };

    // ── BL → DA ────────────────────────────────────────────────────────────

    public static BookingStatusDto ToBookingStatusDto(BookingStatus status) => status switch
    {
        BookingStatus.Created   => BookingStatusDto.Created,
        BookingStatus.Paid      => BookingStatusDto.Paid,
        BookingStatus.InProcess => BookingStatusDto.InProcess,
        BookingStatus.Finished  => BookingStatusDto.Finished,
        BookingStatus.Canceled  => BookingStatusDto.Canceled,
        _ => throw new ArgumentOutOfRangeException(nameof(status), $"Неизвестный BookingStatus: {status}")
    };

    // ── Внутренние конвертеры ───────────────────────────────────────────────

    private static BookingStatus ToBookingStatus(BookingStatusDto status) => status switch
    {
        BookingStatusDto.Created   => BookingStatus.Created,
        BookingStatusDto.Paid      => BookingStatus.Paid,
        BookingStatusDto.InProcess => BookingStatus.InProcess,
        BookingStatusDto.Finished  => BookingStatus.Finished,
        BookingStatusDto.Canceled  => BookingStatus.Canceled,
        _ => throw new ArgumentOutOfRangeException(nameof(status), $"Неизвестный BookingStatusDto: {status}")
    };
}
