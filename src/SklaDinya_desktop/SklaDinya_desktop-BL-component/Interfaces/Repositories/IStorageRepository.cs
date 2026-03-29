using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;

namespace SklaDinya_desktop_BL_component.Interfaces.Repositories;

/// <summary>
/// Репозиторий для работы с пунктами хранения.
/// Публичные методы (поиск, просмотр) токен не требуют.
/// Защищённые методы (обновление, подтверждение, отклонение) требуют JWT-токен.
/// </summary>
public interface IStorageRepository
{
    /// <summary>Найти пункты хранения — публичный поиск, токен не нужен</summary>
    Task<List<StorageModel>> GetStoragesAsync(StorageSearchQuery query);

    /// <summary>Создать пункт хранения (заявка, без авторизации)</summary>
    Task CreateStorageAsync(StorageCreateForm form);

    /// <summary>Получить пункт хранения по ID</summary>
    Task<StorageModel> GetStorageByIdAsync(Guid storageId, string token);

    /// <summary>Обновить данные пункта хранения по ID (для администратора)</summary>
    Task<StorageModel> UpdateStorageByIdAsync(Guid storageId, StorageUpdateForm form, string token);

    /// <summary>Подтвердить заявку на создание пункта хранения (для администратора)</summary>
    Task<StorageModel> ApproveStorageAsync(Guid storageId, string token);

    /// <summary>Отклонить заявку на создание пункта хранения (для администратора)</summary>
    Task RejectStorageAsync(Guid storageId, string token);

    /// <summary>Получить данные своего пункта хранения (для оператора)</summary>
    Task<StorageModel> GetMyStorageAsync(string token);

    /// <summary>Обновить данные своего пункта хранения (для оператора)</summary>
    Task<StorageModel> UpdateMyStorageAsync(StorageUpdateForm form, string token);
}
