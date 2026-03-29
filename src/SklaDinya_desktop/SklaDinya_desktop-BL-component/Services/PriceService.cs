using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Interfaces.Services;
using SklaDinya_desktop_BL_component.Models;

namespace SklaDinya_desktop_BL_component.Services;

/// <summary>
/// Сервис для работы с тарифами пунктов хранения.
/// </summary>
public class PriceService(IPriceRepository priceRepository, ISessionService session) : IPriceService
{
    /// <inheritdoc/>
    public Task<List<PriceModel>> GetPricesAsync(Guid storageId) =>
        priceRepository.GetPricesAsync(storageId);

    /// <inheritdoc/>
    public Task<List<PriceModel>> GetMyPricesAsync() =>
        priceRepository.GetMyPricesAsync(session.Token!);

    /// <inheritdoc/>
    public Task CreatePriceAsync(PriceCreateForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        return priceRepository.CreatePriceAsync(form, session.Token!);
    }
}
