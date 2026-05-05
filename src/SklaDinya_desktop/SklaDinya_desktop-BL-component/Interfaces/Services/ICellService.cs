using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;

namespace SklaDinya_desktop_BL_component.Interfaces.Services;

/// <summary>
/// Сервис для работы с камерами хранения (ячейками)
/// </summary>
public interface ICellService
{
    /// <summary>Найти доступные ячейки конкретного пункта хранения (публично)</summary>
    Task<List<CellModel>> GetCellsAsync(Guid storageId, CellSearchQuery query);

    /// <summary>Получить классы ячеек конкретного пункта хранения (публично)</summary>
    Task<List<string>> GetCellClassesAsync(Guid storageId);

    /// <summary>Найти ячейки своего пункта хранения (для оператора)</summary>
    Task<List<CellModel>> GetMyCellsAsync(MyCellSearchQuery query);

    /// <summary>Получить классы ячеек своего пункта хранения (для оператора)</summary>
    Task<List<string>> GetMyCellClassesAsync();

    /// <summary>Добавить ячейку в свой пункт хранения (для оператора)</summary>
    Task<CellModel> CreateCellAsync(CellCreateForm form);
}
