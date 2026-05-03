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
/// <remarks>
/// Бэкенд возвращает одно оплаченное бронирование (несмотря на то, что в swagger
/// заявлен массив). Поэтому десериализуем напрямую в <see cref="BookingUserDto"/>.
/// </remarks>
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
    /// <remarks>
    /// При HTTP 418 сервер сигнализирует о неудаче моковой оплаты —
    /// <see cref="ApiClient"/> выбросит <see cref="SklaDinya_desktop_BL_component.Exceptions.PaymentFailedException"/>.
    /// </remarks>
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
