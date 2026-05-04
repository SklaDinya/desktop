using System.Web;

namespace SklaDinya_desktop_DA_component.Http;

/// <summary>
/// Строитель URL с query-параметрами.
/// </summary>
public class QueryBuilder(string baseUrl)
{
    private readonly List<(string Key, string Value)> _params = [];

    public QueryBuilder Add(string key, string? value)
    {
        if (value is not null)
            _params.Add((key, value));
        return this;
    }

    public QueryBuilder Add(string key, int? value) =>
        value.HasValue ? Add(key, value.Value.ToString()) : this;

    public QueryBuilder Add(string key, bool? value) =>
        value.HasValue ? Add(key, value.Value.ToString().ToLower()) : this;

    public QueryBuilder Add(string key, DateTime? value, string format = "o") =>
        value.HasValue ? Add(key, value.Value.ToString(format)) : this;

    public QueryBuilder AddEnum<T>(string key, T? value) where T : struct, Enum =>
        value.HasValue ? Add(key, value.Value.ToString()) : this;

    public QueryBuilder AddList(string key, IEnumerable<string>? values)
    {
        if (values is null) return this;
        foreach (var v in values)
            _params.Add((key, v));
        return this;
    }

    public QueryBuilder AddEnumList<T>(string key, IEnumerable<T>? values) where T : struct, Enum
    {
        if (values is null) return this;
        foreach (var v in values)
            _params.Add((key, v.ToString()));
        return this;
    }

    public string Build()
    {
        if (_params.Count == 0) return baseUrl;

        var query = string.Join("&",
            _params.Select(p =>
                $"{HttpUtility.UrlEncode(p.Key)}={HttpUtility.UrlEncode(p.Value)}"));

        return $"{baseUrl}?{query}";
    }
}
