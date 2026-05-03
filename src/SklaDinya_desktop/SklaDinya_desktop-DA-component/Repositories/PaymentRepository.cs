using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_DA_component.Dtos;
using SklaDinya_desktop_DA_component.Http;
using SklaDinya_desktop_DA_component.Mapping;

namespace SklaDinya_desktop_DA_component.Repositories;

/// <summary>
/// Репозиторий для проведения оплаты бронирований.
/// </summary>
public class PaymentRepository(ApiClient client) : IPaymentRepository
{
    /// <inheritdoc/>
    public async Task<BookingModel> PayNoopAsync(PaymentForm form, string token)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        var body = Mapper.ToPaymentNoopRequest(form);
        var dto = await client.PostAsync<BookingUserDto>(
            "/api/v1/payments/noop", body, token);
        return Mapper.ToBooking(dto);
    }

    /// <inheritdoc/>
    public async Task<BookingModel> PayRandomAsync(PaymentForm form, string token)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        var body = Mapper.ToPaymentRandomRequest(form);
        var dto = await client.PostAsync<BookingUserDto>(
            "/api/v1/payments/random", body, token);
        return Mapper.ToBooking(dto);
    }
}
