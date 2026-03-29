using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;

namespace SklaDinya_desktop_BL_component.Interfaces.Repositories;

/// <summary>
/// Репозиторий для работы с операторами пункта хранения
/// </summary>
public interface IOperatorRepository
{
    /// <summary>Найти операторов своего пункта хранения</summary>
    Task<List<OperatorModel>> GetOperatorsAsync(OperatorSearchQuery query);

    /// <summary>Создать оператора для своего пункта хранения</summary>
    Task<OperatorModel> CreateOperatorAsync(OperatorCreateForm form);

    /// <summary>Получить данные оператора по ID</summary>
    Task<OperatorModel> GetOperatorByIdAsync(Guid operatorId);

    /// <summary>Обновить данные оператора</summary>
    Task<OperatorModel> UpdateOperatorAsync(Guid operatorId, OperatorUpdateForm form);
}
