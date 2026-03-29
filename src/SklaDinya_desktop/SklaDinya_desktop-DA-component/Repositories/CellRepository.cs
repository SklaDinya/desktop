using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_DA_component.Dtos;
using SklaDinya_desktop_DA_component.Http;
using SklaDinya_desktop_DA_component.Mapping;

namespace SklaDinya_desktop_DA_component.Repositories;

/// <summary>
/// Репозиторий для работы с камерами хранения (ячейками).
/// </summary>
public class CellRepository(ApiClient client) : ICellRepository
{
    /// <inheritdoc/>
    public async Task<List<CellModel>> GetCellsAsync(Guid storageId, CellSearchQuery query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));

        var url = new QueryBuilder($"/api/v1/storages/{storageId}/cells")
            .Add("startBooking", query.StartBooking.ToString("o"))
            .Add("timeBooking",  System.Xml.XmlConvert.ToString(query.TimeBooking))
            .AddList("cellClasses", query.CellClasses)
            .Add("pageNumber",   query.PageNumber)
            .Add("pageSize",     query.PageSize)
            .Build();

        var dtos = await client.GetAsync<List<CellDto>>(url);
        return Mapper.ToCellList(dtos);
    }

    /// <inheritdoc/>
    public async Task<List<string>> GetCellClassesAsync(Guid storageId)
    {
        var list = await client.GetAsync<List<string>>(
            $"/api/v1/storages/{storageId}/cells/classes");
        return Mapper.ToCellClasses(list);
    }

    /// <inheritdoc/>
    public async Task<List<CellModel>> GetMyCellsAsync(MyCellSearchQuery query, string token)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        var url = new QueryBuilder("/api/v1/storages/my/cells")
            .AddList("cellClasses", query.CellClasses)
            .Add("pageNumber",      query.PageNumber)
            .Add("pageSize",        query.PageSize)
            .Build();

        var dtos = await client.GetAsync<List<CellDto>>(url, token);
        return Mapper.ToCellList(dtos);
    }

    /// <inheritdoc/>
    public async Task<List<string>> GetMyCellClassesAsync(string token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        var list = await client.GetAsync<List<string>>(
            "/api/v1/storages/my/cells/classes", token);
        return Mapper.ToCellClasses(list);
    }

    /// <inheritdoc/>
    public async Task<CellModel> CreateCellAsync(CellCreateForm form, string token)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        var body = new CellCreateRequest(form.Name, form.CellClass);
        var dto  = await client.PostAsync<CellDto>("/api/v1/storages/my/cells", body, token);
        return Mapper.ToCell(dto);
    }
}
