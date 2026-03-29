using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Interfaces.Services;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;

namespace SklaDinya_desktop_BL_component.Services;

/// <summary>
/// Сервис для работы с камерами хранения (ячейками).
/// </summary>
public class CellService(ICellRepository cellRepository, ISessionService session) : ICellService
{
    /// <inheritdoc/>
    public Task<List<CellModel>> GetCellsAsync(Guid storageId, CellSearchQuery query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));
        return cellRepository.GetCellsAsync(storageId, query);
    }

    /// <inheritdoc/>
    public Task<List<string>> GetCellClassesAsync(Guid storageId) =>
        cellRepository.GetCellClassesAsync(storageId);

    /// <inheritdoc/>
    public Task<List<CellModel>> GetMyCellsAsync(MyCellSearchQuery query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));
        return cellRepository.GetMyCellsAsync(query, session.Token!);
    }

    /// <inheritdoc/>
    public Task<List<string>> GetMyCellClassesAsync() =>
        cellRepository.GetMyCellClassesAsync(session.Token!);

    /// <inheritdoc/>
    public Task<CellModel> CreateCellAsync(CellCreateForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        return cellRepository.CreateCellAsync(form, session.Token!);
    }
}
