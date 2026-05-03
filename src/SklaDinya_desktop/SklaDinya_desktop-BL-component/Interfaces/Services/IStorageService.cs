using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;

namespace SklaDinya_desktop_BL_component.Interfaces.Services;

/// <summary>
/// Сервис для работы с пунктами хранения
/// </summary>
public interface IStorageService
{
    /// <summary>Найти пункты хранения (публично)</summary>
    Task<List<StorageModel>> GetStoragesAsync(StorageSearchQuery query);

    /// <summary>
    /// Поиск пунктов по строке из единого поискового поля.
    /// </summary>
    Task<List<StorageModel>> SearchStoragesAsync(
        string text, int pageNumber = 0, int pageSize = 20);

    /// <summary>
    /// Найти заявки на создание пунктов хранения — для администратора.
    /// </summary>
    Task<List<StorageModel>> GetStorageRequestsAsync(StorageSearchQuery query);

    /// <summary>Подать заявку на создание пункта хранения</summary>
    Task CreateStorageAsync(StorageCreateForm form);

    /// <summary>Получить пункт хранения по ID</summary>
    Task<StorageModel> GetStorageByIdAsync(Guid storageId);

    /// <summary>Обновить данные пункта хранения по ID (для администратора)</summary>
    Task<StorageModel> UpdateStorageByIdAsync(Guid storageId, StorageUpdateForm form);

    /// <summary>Подтвердить заявку на создание пункта хранения (для администратора)</summary>
    Task<StorageModel> ApproveStorageAsync(Guid storageId);

    /// <summary>Отклонить заявку на создание пункта хранения (для администратора)</summary>
    Task RejectStorageAsync(Guid storageId);

    /// <summary>Получить данные своего пункта хранения (для оператора)</summary>
    Task<StorageModel> GetMyStorageAsync();

    /// <summary>Обновить данные своего пункта хранения (для оператора)</summary>
    Task<StorageModel> UpdateMyStorageAsync(StorageUpdateForm form);
}
