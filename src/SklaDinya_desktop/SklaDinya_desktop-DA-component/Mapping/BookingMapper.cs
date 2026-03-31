using SklaDinya_desktop_BL_component.Exceptions;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_DA_component.Dtos;

namespace SklaDinya_desktop_DA_component.Mapping;

internal static partial class Mapper
{
    public static BookingModel ToBooking(BookingUserDto dto)
    {
        return new BookingModel
        {
            Id          = dto.Id,
            UserId      = dto.UserId,
            StorageId   = dto.StorageId,
            Storage     = ToStorage(dto.Storage),
            Cells       = ToCellList(dto.Cells),
            StartTime   = dto.StartTime,
            BookingTime = dto.BookingTime,
            CreatedAt   = dto.CreatedAt,
            Status      = dto.Status,
        };
    }

    public static List<BookingModel> ToBookingList(List<BookingUserDto> list) =>
        list.Select(ToBooking).ToList();

    public static BookingOperatorModel ToBookingOperator(BookingOperatorDto dto)
    {
        return new BookingOperatorModel
        {
            Id          = dto.Id,
            UserId      = dto.UserId,
            User        = ToBookingUser(dto.User),
            StorageId   = dto.StorageId,
            Cells       = ToCellList(dto.Cells),
            StartTime   = dto.StartTime,
            BookingTime = dto.BookingTime,
            CreatedAt   = dto.CreatedAt,
            Status      = dto.Status,
        };
    }

    public static List<BookingOperatorModel> ToBookingOperatorList(List<BookingOperatorDto> list) =>
        list.Select(ToBookingOperator).ToList();

    public static BookingReceiptModel ToBookingReceipt(BookingReceiptDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Receipt))
            throw new ServerException("Поле 'bookingReceipt.receipt' пустое в ответе API.");

        return new BookingReceiptModel
        {
            Booking = ToBooking(dto.Booking),
            Receipt = dto.Receipt,
        };
    }

    private static BookingUserModel ToBookingUser(BookingUserInfoDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ServerException("Поле 'bookingUser.name' пустое в ответе API.");

        return new BookingUserModel
        {
            Id    = dto.Id,
            Name  = dto.Name,
            Email = dto.Email,
        };
    }
}
