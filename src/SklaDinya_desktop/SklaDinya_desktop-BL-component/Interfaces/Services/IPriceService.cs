using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Models;

namespace SklaDinya_desktop_BL_component.Interfaces.Services;

/// <summary>
/// Сервис для работы с тарифами
/// </summary>
public interface IPriceService
{
    /// <summary>Получить тарифы конкретного пункта хранения (публично)</summary>
    Task<List<PriceModel>> GetPricesAsync(Guid storageId);

    /// <summary>Получить тарифы своего пункта хранения (для оператора)</summary>
    Task<List<PriceModel>> GetMyPricesAsync();

    /// <summary>Добавить тариф для своего пункта хранения (для оператора)</summary>
    Task CreatePriceAsync(PriceCreateForm form);
}
