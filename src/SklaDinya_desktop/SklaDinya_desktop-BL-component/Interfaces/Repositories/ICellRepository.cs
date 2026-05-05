using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;

namespace SklaDinya_desktop_BL_component.Interfaces.Repositories;

/// <summary>
/// Репозиторий для работы с камерами хранения (ячейками).
/// Публичный поиск и классы ячеек токен не требуют.
/// Операторские методы (my-эндпоинты) требуют JWT-токен.
/// </summary>
public interface ICellRepository
{
    /// <summary>Найти доступные ячейки в конкретном пункте хранения — публично</summary>
    Task<List<CellModel>> GetCellsAsync(Guid storageId, CellSearchQuery query);

    /// <summary>Получить классы ячеек конкретного пункта хранения — публично</summary>
    Task<List<string>> GetCellClassesAsync(Guid storageId);

    /// <summary>Найти ячейки своего пункта хранения (для оператора)</summary>
    Task<List<CellModel>> GetMyCellsAsync(MyCellSearchQuery query, string token);

    /// <summary>Получить классы ячеек своего пункта хранения (для оператора)</summary>
    Task<List<string>> GetMyCellClassesAsync(string token);

    /// <summary>Добавить ячейку в свой пункт хранения (для оператора)</summary>
    Task<CellModel> CreateCellAsync(CellCreateForm form, string token);
}
