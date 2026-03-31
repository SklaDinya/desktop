using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using SklaDinya_desktop_BL_component.Exceptions;
using SklaDinya_desktop_DA_component.Converters;

namespace SklaDinya_desktop_DA_component.Http;

/// <summary>
/// Базовый HTTP-клиент для обращения к REST API бэкенда.
/// Выбрасывает типизированные исключения из BL-компонента при ошибочных статусах.
/// </summary>
public class ApiClient(HttpClient http)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy        = JsonNamingPolicy.CamelCase,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
            new TimeSpanIso8601Converter(),
            new DecimalStringConverter(),
        },
    };

    // ─── GET ────────────────────────────────────────────────────────────────

    public async Task<T> GetAsync<T>(string url, string? token = null)
    {
        using var request  = BuildRequest(HttpMethod.Get, url, token);
        using var response = await http.SendAsync(request);
        await EnsureSuccessAsync(response);
        return await DeserializeAsync<T>(response);
    }

    // ─── POST ───────────────────────────────────────────────────────────────

    public async Task<T> PostAsync<T>(string url, object body, string? token = null)
    {
        using var request  = BuildRequest(HttpMethod.Post, url, token, body);
        using var response = await http.SendAsync(request);
        await EnsureSuccessAsync(response);
        return await DeserializeAsync<T>(response);
    }

    public async Task PostAsync(string url, object body, string? token = null)
    {
        using var request  = BuildRequest(HttpMethod.Post, url, token, body);
        using var response = await http.SendAsync(request);
        await EnsureSuccessAsync(response);
    }

    // ─── PATCH ──────────────────────────────────────────────────────────────

    public async Task<T> PatchAsync<T>(string url, object body, string? token = null)
    {
        using var request  = BuildRequest(HttpMethod.Patch, url, token, body);
        using var response = await http.SendAsync(request);
        await EnsureSuccessAsync(response);
        return await DeserializeAsync<T>(response);
    }

    /// <summary>PATCH без тела — для эндпоинтов вроде /approve</summary>
    public async Task<T> PatchAsync<T>(string url, string? token = null)
    {
        using var request  = BuildRequest(HttpMethod.Patch, url, token);
        using var response = await http.SendAsync(request);
        await EnsureSuccessAsync(response);
        return await DeserializeAsync<T>(response);
    }

    // ─── DELETE ─────────────────────────────────────────────────────────────

    public async Task<T> DeleteAsync<T>(string url, string? token = null)
    {
        using var request  = BuildRequest(HttpMethod.Delete, url, token);
        using var response = await http.SendAsync(request);
        await EnsureSuccessAsync(response);
        return await DeserializeAsync<T>(response);
    }

    public async Task DeleteAsync(string url, string? token = null)
    {
        using var request  = BuildRequest(HttpMethod.Delete, url, token);
        using var response = await http.SendAsync(request);
        await EnsureSuccessAsync(response);
    }

    // ─── Helpers ────────────────────────────────────────────────────────────

    private static HttpRequestMessage BuildRequest(
        HttpMethod method, string url, string? token, object? body = null)
    {
        var request = new HttpRequestMessage(method, url);

        if (token is not null)
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        if (body is not null)
        {
            var json = JsonSerializer.Serialize(body, JsonOptions);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        return request;
    }

    private static async Task<T> DeserializeAsync<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(content))
            throw new ServerException(
                $"Сервер вернул пустое тело ответа при статусе {(int)response.StatusCode}.");

        var result = JsonSerializer.Deserialize<T>(content, JsonOptions);

        if (result is null)
            throw new ServerException(
                "Не удалось десериализовать ответ сервера. " +
                $"Тело: {content[..Math.Min(200, content.Length)]}");

        return result;
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode) return;

        var body = string.Empty;
        try { body = await response.Content.ReadAsStringAsync(); }
        catch { /* игнорируем ошибки чтения тела */ }

        var msg = string.IsNullOrWhiteSpace(body) ? null : body;

        throw response.StatusCode switch
        {
            HttpStatusCode.BadRequest       => new ApiException(400,
                msg ?? "Некорректные данные запроса."),
            HttpStatusCode.Unauthorized     => new UnauthorizedException(
                msg ?? "Пользователь не авторизован."),
            HttpStatusCode.Forbidden        => new ForbiddenException(
                msg ?? "Доступ запрещён."),
            HttpStatusCode.NotFound         => new NotFoundException(
                msg ?? "Ресурс не найден."),
            HttpStatusCode.MethodNotAllowed => new ReadOnlyModeException(
                msg ?? "Сервер запущен в режиме только для чтения."),
            HttpStatusCode.Conflict         => new ConflictException(
                msg ?? "Конфликт данных."),
            (HttpStatusCode)418             => new PaymentFailedException(
                msg ?? "Оплата не прошла. Попробуйте ещё раз."),
            HttpStatusCode.InternalServerError => new ServerException(
                msg ?? "Внутренняя ошибка сервера."),
            _ => new ApiException((int)response.StatusCode,
                msg ?? $"Неожиданный статус ответа: {(int)response.StatusCode}."),
        };
    }
}
