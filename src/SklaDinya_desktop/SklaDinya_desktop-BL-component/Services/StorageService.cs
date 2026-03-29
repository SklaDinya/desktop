using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Interfaces.Services;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;

namespace SklaDinya_desktop_BL_component.Services;

/// <summary>
/// Сервис для работы с пунктами хранения.
/// </summary>
public class StorageService : IStorageService
{
    private readonly IStorageRepository _storageRepository;

    public StorageService(IStorageRepository storageRepository)
    {
        _storageRepository = storageRepository;
    }

    /// <inheritdoc/>
    public Task<List<StorageModel>> GetStoragesAsync(StorageSearchQuery query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));
        return _storageRepository.GetStoragesAsync(query);
    }

    /// <inheritdoc/>
    public Task CreateStorageAsync(StorageCreateForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        return _storageRepository.CreateStorageAsync(form);
    }

    /// <inheritdoc/>
    public Task<StorageModel> GetStorageByIdAsync(Guid storageId) =>
        _storageRepository.GetStorageByIdAsync(storageId);

    /// <inheritdoc/>
    public Task<StorageModel> UpdateStorageByIdAsync(Guid storageId, StorageUpdateForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        return _storageRepository.UpdateStorageByIdAsync(storageId, form);
    }

    /// <inheritdoc/>
    public Task<StorageModel> ApproveStorageAsync(Guid storageId) =>
        _storageRepository.ApproveStorageAsync(storageId);

    /// <inheritdoc/>
    public Task RejectStorageAsync(Guid storageId) =>
        _storageRepository.RejectStorageAsync(storageId);

    /// <inheritdoc/>
    public Task<StorageModel> GetMyStorageAsync() =>
        _storageRepository.GetMyStorageAsync();

    /// <inheritdoc/>
    public Task<StorageModel> UpdateMyStorageAsync(StorageUpdateForm form)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        return _storageRepository.UpdateMyStorageAsync(form);
    }
}
