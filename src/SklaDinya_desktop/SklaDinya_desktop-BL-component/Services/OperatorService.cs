using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Interfaces.Services;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;

namespace SklaDinya_desktop_BL_component.Services;

/// <summary>
/// Сервис для управления операторами пункта хранения.
/// </summary>
public class OperatorService(IOperatorRepository operatorRepository, ISessionService session) : IOperatorService
{
    /// <inheritdoc/>
    public Task<List<OperatorModel>> GetOperatorsAsync(OperatorSearchQuery query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));
        return operatorRepository.GetOperatorsAsync(query, session.Token!);
    }

    /// <inheritdoc/>
    public Task<OperatorModel> CreateOperatorAsync(OperatorCreateForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        return operatorRepository.CreateOperatorAsync(form, session.Token!);
    }

    /// <inheritdoc/>
    public Task<OperatorModel> GetOperatorByIdAsync(Guid operatorId) =>
        operatorRepository.GetOperatorByIdAsync(operatorId, session.Token!);

    /// <inheritdoc/>
    public Task<OperatorModel> UpdateOperatorAsync(Guid operatorId, OperatorUpdateForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        return operatorRepository.UpdateOperatorAsync(operatorId, form, session.Token!);
    }
}
