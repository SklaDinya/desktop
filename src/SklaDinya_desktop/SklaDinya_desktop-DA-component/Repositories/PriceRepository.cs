using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_DA_component.Dtos;
using SklaDinya_desktop_DA_component.Http;
using SklaDinya_desktop_DA_component.Mapping;

namespace SklaDinya_desktop_DA_component.Repositories;

/// <summary>
/// Репозиторий для работы с тарифами.
/// </summary>
public class PriceRepository(ApiClient client) : IPriceRepository
{
    /// <inheritdoc/>
    public async Task<List<PriceModel>> GetPricesAsync(Guid storageId)
    {
        var dtos = await client.GetAsync<List<PriceDto>>(
            $"/api/v1/storages/{storageId}/prices");
        return Mapper.ToPriceList(dtos);
    }

    /// <inheritdoc/>
    public async Task<List<PriceModel>> GetMyPricesAsync(string token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        var dtos = await client.GetAsync<List<PriceDto>>(
            "/api/v1/storages/my/prices", token);
        return Mapper.ToPriceList(dtos);
    }

    /// <inheritdoc/>
    public async Task CreatePriceAsync(PriceCreateForm form, string token)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        var body = new PriceCreateRequest(form.CellClass, form.Price);

        await client.PostAsync("/api/v1/storages/my/prices", body, token);
    }
}
