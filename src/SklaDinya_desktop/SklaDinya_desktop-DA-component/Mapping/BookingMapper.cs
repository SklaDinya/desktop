using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Exceptions;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_DA_component.Dtos;

namespace SklaDinya_desktop_DA_component.Mapping;

internal static partial class Mapper
{
    public static BookingModel ToBooking(BookingUserDto? dto)
    {
        EnsureNotNull(dto, "BookingUserDto");
        return new BookingModel
        {
            Id          = ParseGuid(dto!.Id,       "booking.id"),
            UserId      = ParseGuid(dto.UserId,    "booking.userId"),
            StorageId   = ParseGuid(dto.StorageId, "booking.storageId"),
            Storage     = ToStorage(dto.Storage),
            Cells       = ToCellList(dto.Cells),
            StartTime   = ParseDateTime(dto.StartTime,   "booking.startTime"),
            BookingTime = ParseDuration(dto.BookingTime, "booking.bookingTime"),
            CreatedAt   = ParseDateTime(dto.CreatedAt,   "booking.createdAt"),
            Status      = ParseEnum<BookingStatus>(dto.Status, "booking.status"),
        };
    }

    public static List<BookingModel> ToBookingList(List<BookingUserDto>? list)
    {
        EnsureNotNull(list, "Booking[]");
        return list!.Select(ToBooking).ToList();
    }

    public static BookingOperatorModel ToBookingOperator(BookingOperatorDto? dto)
    {
        EnsureNotNull(dto, "BookingOperatorDto");
        return new BookingOperatorModel
        {
            Id          = ParseGuid(dto!.Id,       "bookingOperator.id"),
            UserId      = ParseGuid(dto.UserId,    "bookingOperator.userId"),
            User        = ToBookingUser(dto.User),
            StorageId   = ParseGuid(dto.StorageId, "bookingOperator.storageId"),
            Cells       = ToCellList(dto.Cells),
            StartTime   = ParseDateTime(dto.StartTime,   "bookingOperator.startTime"),
            BookingTime = ParseDuration(dto.BookingTime, "bookingOperator.bookingTime"),
            CreatedAt   = ParseDateTime(dto.CreatedAt,   "bookingOperator.createdAt"),
            Status      = ParseEnum<BookingStatus>(dto.Status, "bookingOperator.status"),
        };
    }

    public static List<BookingOperatorModel> ToBookingOperatorList(List<BookingOperatorDto>? list)
    {
        EnsureNotNull(list, "BookingOperator[]");
        return list!.Select(ToBookingOperator).ToList();
    }

    public static BookingReceiptModel ToBookingReceipt(BookingReceiptDto? dto)
    {
        EnsureNotNull(dto, "BookingReceiptDto");

        if (dto!.Booking is null)
            throw new ServerException(
                "Поле 'booking' в ответе на создание бронирования равно null.");

        return new BookingReceiptModel
        {
            Booking = ToBooking(dto.Booking),
            Receipt = RequireString(dto.Receipt, "bookingReceipt.receipt"),
        };
    }

    private static BookingUserModel ToBookingUser(BookingUserInfoDto? dto)
    {
        EnsureNotNull(dto, "BookingUserInfoDto");
        return new BookingUserModel
        {
            Id    = ParseGuid(dto!.Id,      "bookingUser.id"),
            Name  = RequireString(dto.Name, "bookingUser.name"),
            Email = dto.Email,
        };
    }
}
