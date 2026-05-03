using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Interfaces.Services;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;

namespace SklaDinya_desktop_BL_component.Services;

/// <summary>
/// Сервис для работы с пунктами хранения.
/// </summary>
public class StorageService(IStorageRepository storageRepository, ISessionService session) : IStorageService
{
    /// <inheritdoc/>
    public Task<List<StorageModel>> GetStoragesAsync(StorageSearchQuery query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));
        return storageRepository.GetStoragesAsync(query);
    }

    /// <inheritdoc/>
    public async Task<List<StorageModel>> SearchStoragesAsync(
        string text, int pageNumber = 0, int pageSize = 20)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return await storageRepository.GetStoragesAsync(
                new StorageSearchQuery { PageNumber = pageNumber, PageSize = pageSize });
        }

        var trimmed = text.Trim();

        var byNameTask = storageRepository.GetStoragesAsync(new StorageSearchQuery
        {
            Name = trimmed,
            PageNumber = pageNumber,
            PageSize = pageSize,
        });
        var byAddressTask = storageRepository.GetStoragesAsync(new StorageSearchQuery
        {
            Address = trimmed,
            PageNumber = pageNumber,
            PageSize = pageSize,
        });

        var results = await Task.WhenAll(byNameTask, byAddressTask);

        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var merged = new List<StorageModel>(capacity: results[0].Count + results[1].Count);
        foreach (var storage in results.SelectMany(r => r))
        {
            if (seen.Add(storage.Name))
                merged.Add(storage);
        }
        return merged;
    }

    /// <inheritdoc/>
    public Task<List<StorageModel>> GetStorageRequestsAsync(StorageSearchQuery query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));
        return storageRepository.GetStorageRequestsAsync(query, session.Token!);
    }

    /// <inheritdoc/>
    public Task CreateStorageAsync(StorageCreateForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        return storageRepository.CreateStorageAsync(form);
    }

    /// <inheritdoc/>
    public Task<StorageModel> GetStorageByIdAsync(Guid storageId) =>
        storageRepository.GetStorageByIdAsync(storageId, session.Token!);

    /// <inheritdoc/>
    public Task<StorageModel> UpdateStorageByIdAsync(Guid storageId, StorageUpdateForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        return storageRepository.UpdateStorageByIdAsync(storageId, form, session.Token!);
    }

    /// <inheritdoc/>
    public Task<StorageModel> ApproveStorageAsync(Guid storageId) =>
        storageRepository.ApproveStorageAsync(storageId, session.Token!);

    /// <inheritdoc/>
    public Task RejectStorageAsync(Guid storageId) =>
        storageRepository.RejectStorageAsync(storageId, session.Token!);

    /// <inheritdoc/>
    public Task<StorageModel> GetMyStorageAsync() =>
        storageRepository.GetMyStorageAsync(session.Token!);

    /// <inheritdoc/>
    public Task<StorageModel> UpdateMyStorageAsync(StorageUpdateForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        return storageRepository.UpdateMyStorageAsync(form, session.Token!);
    }
}
