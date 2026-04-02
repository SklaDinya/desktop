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
    public async Task<List<BookingModel>> PayNoopAsync(PaymentForm form, string token)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        var body = Mapper.ToPaymentNoopRequest(form);
        var dtos = await client.PostAsync<List<BookingUserDto>>(
            "/api/v1/payments/noop", body, token);
        return Mapper.ToBookingList(dtos);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// При HTTP 418 сервер сигнализирует о неудаче моковой оплаты —
    /// <see cref="ApiClient"/> выбросит <see cref="SklaDinya_desktop_BL_component.Exceptions.PaymentFailedException"/>.
    /// </remarks>
    public async Task<List<BookingModel>> PayRandomAsync(PaymentForm form, string token)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        var body = Mapper.ToPaymentRandomRequest(form);
        var dtos = await client.PostAsync<List<BookingUserDto>>(
            "/api/v1/payments/random", body, token);
        return Mapper.ToBookingList(dtos);
    }
}
