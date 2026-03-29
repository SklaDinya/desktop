using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Interfaces.Services;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;

namespace SklaDinya_desktop_BL_component.Services;

/// <summary>
/// Сервис для работы с камерами хранения (ячейками).
/// </summary>
public class CellService : ICellService
{
    private readonly ICellRepository _cellRepository;

    public CellService(ICellRepository cellRepository)
    {
        _cellRepository = cellRepository;
    }

    /// <inheritdoc/>
    public Task<List<CellModel>> GetCellsAsync(Guid storageId, CellSearchQuery query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));
        return _cellRepository.GetCellsAsync(storageId, query);
    }

    /// <inheritdoc/>
    public Task<List<string>> GetCellClassesAsync(Guid storageId) =>
        _cellRepository.GetCellClassesAsync(storageId);

    /// <inheritdoc/>
    public Task<List<CellModel>> GetMyCellsAsync(MyCellSearchQuery query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));
        return _cellRepository.GetMyCellsAsync(query);
    }

    /// <inheritdoc/>
    public Task<List<string>> GetMyCellClassesAsync() =>
        _cellRepository.GetMyCellClassesAsync();

    /// <inheritdoc/>
    public Task<CellModel> CreateCellAsync(CellCreateForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        return _cellRepository.CreateCellAsync(form);
    }
}
