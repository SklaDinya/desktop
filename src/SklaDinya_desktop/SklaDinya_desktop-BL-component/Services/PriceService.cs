using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Interfaces.Services;
using SklaDinya_desktop_BL_component.Models;

namespace SklaDinya_desktop_BL_component.Services;

/// <summary>
/// Сервис для работы с тарифами пунктов хранения.
/// </summary>
public class PriceService : IPriceService
{
    private readonly IPriceRepository _priceRepository;

    public PriceService(IPriceRepository priceRepository)
    {
        _priceRepository = priceRepository;
    }

    /// <inheritdoc/>
    public Task<List<PriceModel>> GetPricesAsync(Guid storageId) =>
        _priceRepository.GetPricesAsync(storageId);

    /// <inheritdoc/>
    public Task<List<PriceModel>> GetMyPricesAsync() =>
        _priceRepository.GetMyPricesAsync();

    /// <inheritdoc/>
    public Task CreatePriceAsync(PriceCreateForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        return _priceRepository.CreatePriceAsync(form);
    }
}
