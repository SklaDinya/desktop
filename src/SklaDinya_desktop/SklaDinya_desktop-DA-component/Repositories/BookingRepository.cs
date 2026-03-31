using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_DA_component.Dtos;
using SklaDinya_desktop_DA_component.Http;
using SklaDinya_desktop_DA_component.Mapping;

namespace SklaDinya_desktop_DA_component.Repositories;

/// <summary>
/// Репозиторий для работы с бронированиями.
/// </summary>
public class BookingRepository(ApiClient client) : IBookingRepository
{
    /// <inheritdoc/>
    public async Task<List<BookingModel>> GetMyBookingsAsync(
        BookingSearchQuery query, string token)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        var url = new QueryBuilder("/api/v1/users/me/bookings")
            .Add("pageNumber", query.PageNumber)
            .Add("pageSize",   query.PageSize)
            .Build();

        var dtos = await client.GetAsync<List<BookingUserDto>>(url, token);
        return Mapper.ToBookingList(dtos);
    }

    /// <inheritdoc/>
    public async Task<BookingReceiptModel> CreateBookingAsync(
        BookingCreateForm form, string token)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        var body = new BookingCreateRequest(
            form.StorageId,
            form.CellIds,
            form.StartTime,
            form.BookingTime);

        var dto = await client.PostAsync<BookingReceiptDto>(
            "/api/v1/users/me/bookings", body, token);
        return Mapper.ToBookingReceipt(dto);
    }

    /// <inheritdoc/>
    public async Task<BookingModel> GetMyBookingByIdAsync(Guid bookingId, string token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        var dto = await client.GetAsync<BookingUserDto>(
            $"/api/v1/users/me/bookings/{bookingId}", token);
        return Mapper.ToBooking(dto);
    }

    /// <inheritdoc/>
    public async Task<BookingModel> CancelMyBookingAsync(Guid bookingId, string token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        var dto = await client.DeleteAsync<BookingUserDto>(
            $"/api/v1/users/me/bookings/{bookingId}", token);
        return Mapper.ToBooking(dto);
    }

    /// <inheritdoc/>
    public async Task<List<BookingOperatorModel>> GetStorageBookingsAsync(
        OperatorBookingSearchQuery query, string token)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        var statusesDto = query.Statuses?
            .Select(Mapper.ToBookingStatusDto)
            .ToList();

        var url = new QueryBuilder("/api/v1/storages/my/bookings")
            .Add("startBooking",     query.StartBooking.ToString("o"))
            .Add("endBooking",       query.EndBooking.ToString("o"))
            .AddEnumList("statuses", statusesDto)
            .Add("pageNumber",       query.PageNumber)
            .Add("pageSize",         query.PageSize)
            .Build();

        var dtos = await client.GetAsync<List<BookingOperatorDto>>(url, token);
        return Mapper.ToBookingOperatorList(dtos);
    }
}
