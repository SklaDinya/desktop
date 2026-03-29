using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Interfaces.Services;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;

namespace SklaDinya_desktop_BL_component.Services;

/// <summary>
/// Сервис для управления операторами пункта хранения.
/// Доступен только главному оператору (MainOperator).
/// </summary>
public class OperatorService : IOperatorService
{
    private readonly IOperatorRepository _operatorRepository;

    public OperatorService(IOperatorRepository operatorRepository)
    {
        _operatorRepository = operatorRepository;
    }

    /// <inheritdoc/>
    public Task<List<OperatorModel>> GetOperatorsAsync(OperatorSearchQuery query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));
        return _operatorRepository.GetOperatorsAsync(query);
    }

    /// <inheritdoc/>
    public Task<OperatorModel> CreateOperatorAsync(OperatorCreateForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        return _operatorRepository.CreateOperatorAsync(form);
    }

    /// <inheritdoc/>
    public Task<OperatorModel> GetOperatorByIdAsync(Guid operatorId) =>
        _operatorRepository.GetOperatorByIdAsync(operatorId);

    /// <inheritdoc/>
    public Task<OperatorModel> UpdateOperatorAsync(Guid operatorId, OperatorUpdateForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        return _operatorRepository.UpdateOperatorAsync(operatorId, form);
    }
}
