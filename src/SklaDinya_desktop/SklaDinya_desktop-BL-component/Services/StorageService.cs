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
