using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_DA_component.Dtos;
using SklaDinya_desktop_DA_component.Http;
using SklaDinya_desktop_DA_component.Mapping;

namespace SklaDinya_desktop_DA_component.Repositories;

/// <summary>
/// Репозиторий для управления операторами пункта хранения.
/// </summary>
public class OperatorRepository(ApiClient client) : IOperatorRepository
{
    /// <inheritdoc/>
    public async Task<List<OperatorModel>> GetOperatorsAsync(
        OperatorSearchQuery query, string token)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        var roleDto = query.Role.HasValue
            ? Mapper.ToOperatorRoleDto(query.Role.Value)
            : (Enums.OperatorRoleDto?)null;

        var url = new QueryBuilder("/api/v1/storages/my/operators")
            .Add("username", query.Username)
            .Add("name", query.Name)
            .Add("email", query.Email)
            .AddEnum("role", roleDto)
            .Add("pageNumber", query.PageNumber)
            .Add("pageSize", query.PageSize)
            .Build();

        var dtos = await client.GetAsync<List<OperatorDto>>(url, token);
        return Mapper.ToOperatorList(dtos);
    }

    /// <inheritdoc/>
    public async Task<OperatorModel> CreateOperatorAsync(OperatorCreateForm form, string token)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        var body = Mapper.ToOperatorCreateRequest(form);
        var dto = await client.PostAsync<OperatorDto>(
            "/api/v1/storages/my/operators", body, token);
        return Mapper.ToOperator(dto);
    }

    /// <inheritdoc/>
    public async Task<OperatorModel> GetOperatorByIdAsync(Guid operatorId, string token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        var dto = await client.GetAsync<OperatorDto>(
            $"/api/v1/storages/my/operators/{operatorId}", token);
        return Mapper.ToOperator(dto);
    }

    /// <inheritdoc/>
    public async Task<OperatorModel> UpdateOperatorAsync(
        Guid operatorId, OperatorUpdateForm form, string token)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        var body = Mapper.ToOperatorUpdateRequest(form);
        var dto = await client.PatchAsync<OperatorDto>(
            $"/api/v1/storages/my/operators/{operatorId}", body, token);
        return Mapper.ToOperator(dto);
    }
}
