using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Models;

namespace SklaDinya_desktop_BL_component.Interfaces.Repositories;

/// <summary>
/// Репозиторий для работы с тарифами.
/// Публичный просмотр тарифов токен не требует.
/// Операторские методы требуют JWT-токен.
/// </summary>
public interface IPriceRepository
{
    /// <summary>Получить тарифы конкретного пункта хранения — публично</summary>
    Task<List<PriceModel>> GetPricesAsync(Guid storageId);

    /// <summary>Получить тарифы своего пункта хранения (для оператора)</summary>
    Task<List<PriceModel>> GetMyPricesAsync(string token);

    /// <summary>Добавить тариф для своего пункта хранения (для оператора)</summary>
    Task CreatePriceAsync(PriceCreateForm form, string token);
}
