using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_DA_component.Dtos;
using SklaDinya_desktop_DA_component.Http;
using SklaDinya_desktop_DA_component.Mapping;

namespace SklaDinya_desktop_DA_component.Repositories;

/// <summary>
/// Репозиторий для работы с пунктами хранения.
/// </summary>
public class StorageRepository(ApiClient client) : IStorageRepository
{
    /// <inheritdoc/>
    public async Task<List<StorageModel>> GetStoragesAsync(StorageSearchQuery query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));

        var url = new QueryBuilder("/api/v1/storages")
            .Add("name", query.Name)
            .Add("address", query.Address)
            .Add("pageNumber", query.PageNumber)
            .Add("pageSize", query.PageSize)
            .Build();

        var dtos = await client.GetAsync<List<StorageDto>>(url);
        return Mapper.ToStorageList(dtos);
    }

    /// <inheritdoc/>
    public async Task CreateStorageAsync(StorageCreateForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));

        var body = Mapper.ToStorageCreateRequest(form);
        await client.PostAsync("/api/v1/storages", body);
    }

    /// <inheritdoc/>
    public async Task<StorageModel> GetStorageByIdAsync(Guid storageId, string token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        var dto = await client.GetAsync<StorageDto>($"/api/v1/storages/{storageId}", token);
        return Mapper.ToStorage(dto);
    }

    /// <inheritdoc/>
    public async Task<StorageModel> UpdateStorageByIdAsync(
        Guid storageId, StorageUpdateForm form, string token)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        var body = Mapper.ToStorageUpdateRequest(form);
        var dto = await client.PatchAsync<StorageDto>(
            $"/api/v1/storages/{storageId}", body, token);
        return Mapper.ToStorage(dto);
    }

    /// <inheritdoc/>
    public async Task<StorageModel> ApproveStorageAsync(Guid storageId, string token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        var dto = await client.PatchAsync<StorageDto>(
            $"/api/v1/storages/{storageId}/approve", token);
        return Mapper.ToStorage(dto);
    }

    /// <inheritdoc/>
    public async Task RejectStorageAsync(Guid storageId, string token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        await client.DeleteAsync($"/api/v1/storages/{storageId}/reject", token);
    }

    /// <inheritdoc/>
    public async Task<StorageModel> GetMyStorageAsync(string token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        var dto = await client.GetAsync<StorageDto>("/api/v1/storages/my", token);
        return Mapper.ToStorage(dto);
    }

    /// <inheritdoc/>
    public async Task<StorageModel> UpdateMyStorageAsync(StorageUpdateForm form, string token)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        var body = Mapper.ToStorageUpdateRequest(form);
        var dto = await client.PatchAsync<StorageDto>("/api/v1/storages/my", body, token);
        return Mapper.ToStorage(dto);
    }
}
