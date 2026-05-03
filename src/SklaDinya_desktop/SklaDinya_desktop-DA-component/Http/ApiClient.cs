using SklaDinya_desktop_BL_component.Exceptions;
using SklaDinya_desktop_DA_component.Converters;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SklaDinya_desktop_DA_component.Http;

/// <summary>
/// Базовый HTTP-клиент для обращения к REST API бэкенда.
/// Выбрасывает типизированные исключения из BL-компонента при ошибочных статусах.
/// При любой ошибке записывает контекст запроса/ответа в файловый лог.
/// </summary>
public class ApiClient(HttpClient http)
{

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters =
        {
            // ВАЖНО: специфические конвертеры идут ПЕРЕД общим JsonStringEnumConverter,
            // иначе общий перехватит тип первым (System.Text.Json берёт первый
            // подходящий конвертер из коллекции).
            //
            // BookingStatusJsonConverter принимает обе формы — "Cancelled" (как
            // шлёт реальный бэкенд) и "Canceled" (наше имя в C#).
            new BookingStatusJsonConverter(),
            new TimeSpanIso8601Converter(),
            // Общий enum-конвертер для прочих перечислений (StorageStatusDto,
            // UserRoleDto, OperatorRoleDto). Case-insensitive по умолчанию,
            // поэтому "Active"/"active" обрабатываются одинаково.
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
        },
    };

    // ─── GET ────────────────────────────────────────────────────────────────

    public async Task<T> GetAsync<T>(string url, string? token = null)
    {
        using var request = BuildRequest(HttpMethod.Get, url, token);
        using var response = await LogRequestAndSendAsync(request, requestBodyJson: null);
        await EnsureSuccessAsync(response, request, requestBodyJson: null);
        return await DeserializeAsync<T>(response, request, requestBodyJson: null);
    }

    // ─── POST ───────────────────────────────────────────────────────────────

    public async Task<T> PostAsync<T>(string url, object body, string? token = null)
    {
        var bodyJson = JsonSerializer.Serialize(body, JsonOptions);
        using var request = BuildRequest(HttpMethod.Post, url, token, bodyJson);
        using var response = await LogRequestAndSendAsync(request, bodyJson);
        await EnsureSuccessAsync(response, request, bodyJson);
        return await DeserializeAsync<T>(response, request, bodyJson);
    }

    public async Task PostAsync(string url, object body, string? token = null)
    {
        var bodyJson = JsonSerializer.Serialize(body, JsonOptions);
        using var request = BuildRequest(HttpMethod.Post, url, token, bodyJson);
        using var response = await LogRequestAndSendAsync(request, bodyJson);
        await EnsureSuccessAsync(response, request, bodyJson);
    }

    // ─── PATCH ──────────────────────────────────────────────────────────────

    public async Task<T> PatchAsync<T>(string url, object body, string? token = null)
    {
        var bodyJson = JsonSerializer.Serialize(body, JsonOptions);
        using var request = BuildRequest(HttpMethod.Patch, url, token, bodyJson);
        using var response = await LogRequestAndSendAsync(request, bodyJson);
        await EnsureSuccessAsync(response, request, bodyJson);
        return await DeserializeAsync<T>(response, request, bodyJson);
    }

    /// <summary>PATCH без тела — для эндпоинтов вроде /approve</summary>
    public async Task<T> PatchAsync<T>(string url, string? token = null)
    {
        using var request = BuildRequest(HttpMethod.Patch, url, token);
        using var response = await LogRequestAndSendAsync(request, requestBodyJson: null);
        await EnsureSuccessAsync(response, request, requestBodyJson: null);
        return await DeserializeAsync<T>(response, request, requestBodyJson: null);
    }

    // ─── DELETE ─────────────────────────────────────────────────────────────

    public async Task<T> DeleteAsync<T>(string url, string? token = null)
    {
        using var request = BuildRequest(HttpMethod.Delete, url, token);
        using var response = await LogRequestAndSendAsync(request, requestBodyJson: null);
        await EnsureSuccessAsync(response, request, requestBodyJson: null);
        return await DeserializeAsync<T>(response, request, requestBodyJson: null);
    }

    public async Task DeleteAsync(string url, string? token = null)
    {
        using var request = BuildRequest(HttpMethod.Delete, url, token);
        using var response = await LogRequestAndSendAsync(request, requestBodyJson: null);
        await EnsureSuccessAsync(response, request, requestBodyJson: null);
    }

    // ─── Helpers ────────────────────────────────────────────────────────────

    /// <summary>
    /// Отправить запрос с записью в лог: до запроса — INFO с методом/URL/токеном,
    /// после ответа — INFO со статусом. Сами тела запроса/ответа в INFO не пишем,
    /// чтобы лог не разрастался; они попадают в ERROR-лог из <see cref="EnsureSuccessAsync"/>
    /// при ненулевом статусе. Полный URL в логе можно скопировать в адресную строку
    /// браузера (если запрос — GET без авторизации) или в curl-команду для проверки.
    /// </summary>
    private async Task<HttpResponseMessage> LogRequestAndSendAsync(
        HttpRequestMessage request, string? requestBodyJson)
    {
        var fullUrl = ResolveFullUrl(request);
        var auth = request.Headers.Authorization is { } a
            ? $"{a.Scheme} {MaskToken(a.Parameter)}"
            : "<none>";

        FileLogger.Info(
            $"HTTP запрос → {request.Method} {fullUrl} | Authorization: {auth}" +
            (requestBodyJson is null ? "" : $" | Body: {requestBodyJson}"));

        var response = await http.SendAsync(request);

        FileLogger.Info(
            $"HTTP ответ ← {(int)response.StatusCode} {response.StatusCode} " +
            $"для {request.Method} {fullUrl}");

        return response;
    }

    /// <summary>
    /// Возвращает полный URL запроса. <see cref="HttpRequestMessage.RequestUri"/>
    /// может быть относительным, если у <see cref="HttpClient"/> задан BaseAddress —
    /// для лога склеиваем их вручную, чтобы было что копировать в браузер.
    /// </summary>
    private string ResolveFullUrl(HttpRequestMessage request)
    {
        var uri = request.RequestUri;
        if (uri is null) return "<no-uri>";
        if (uri.IsAbsoluteUri) return uri.ToString();
        if (http.BaseAddress is { } baseAddr)
            return new Uri(baseAddr, uri).ToString();
        return uri.ToString();
    }

    /// <summary>
    /// Маскирует JWT для лога: оставляет 8 первых и 4 последних символа,
    /// между ними «…». Помогает по логу понять «один ли это токен» без раскрытия.
    /// </summary>
    private static string MaskToken(string? token)
    {
        if (string.IsNullOrWhiteSpace(token)) return "<empty>";
        if (token.Length <= 16) return token[..Math.Min(4, token.Length)] + "…";
        return token[..8] + "…" + token[^4..] + $" (len={token.Length})";
    }

    private static HttpRequestMessage BuildRequest(
        HttpMethod method, string url, string? token, string? bodyJson = null)
    {
        var request = new HttpRequestMessage(method, url);

        if (token is not null)
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        if (bodyJson is not null)
        {
            request.Content = new StringContent(bodyJson, Encoding.UTF8, "application/json");
        }

        return request;
    }

    /// <summary>
    /// Десериализация ответа с особой обработкой <see cref="string"/>:
    /// бэкенд может вернуть «голую» строку (например, JWT) либо как plain text,
    /// либо как JSON-строку в кавычках. Поддерживаем оба варианта.
    /// </summary>
    private static async Task<T> DeserializeAsync<T>(
        HttpResponseMessage response,
        HttpRequestMessage request,
        string? requestBodyJson)
    {
        var content = await response.Content.ReadAsStringAsync();

        // ── Особый случай: ожидаем строку ──────────────────────────────
        // Сервер по контракту может вернуть JWT как plain text (без кавычек),
        // тогда JsonSerializer.Deserialize<string> упадёт. Обрабатываем оба
        // варианта корректно. ВАЖНО: возвращаем trimmed-значение —
        // \r\n в конце ломает заголовок Authorization при следующем запросе.
        if (typeof(T) == typeof(string))
        {
            var trimmed = content?.Trim() ?? string.Empty;

            // Если ответ выглядит как JSON-строка в кавычках — десериализуем
            // штатно. Иначе возвращаем как есть.
            if (trimmed.StartsWith('"') && trimmed.EndsWith('"'))
            {
                try
                {
                    var parsed = JsonSerializer.Deserialize<string>(trimmed, JsonOptions);
                    return (T)(object)((parsed ?? string.Empty).Trim());
                }
                catch
                {
                    // Падать не будем — отдадим как есть, без кавычек
                    var unquoted = trimmed[1..^1].Trim();
                    return (T)(object)unquoted;
                }
            }

            return (T)(object)trimmed;
        }

        // ── Обычная JSON-десериализация ────────────────────────────────
        try
        {
            return JsonSerializer.Deserialize<T>(content, JsonOptions)!;
        }
        catch (Exception ex)
        {
            FileLogger.Error(
                "Ошибка десериализации ответа сервера",
                ex,
                BuildContext(request, requestBodyJson, response, content,
                    extra: ("ExpectedType", typeof(T).FullName)));

            throw new ServerException(
                $"Ошибка при десериализации ответа сервера: {ex.Message}");
        }
    }

    private static async Task EnsureSuccessAsync(
        HttpResponseMessage response,
        HttpRequestMessage request,
        string? requestBodyJson)
    {
        if (response.IsSuccessStatusCode) return;

        var body = string.Empty;
        try { body = await response.Content.ReadAsStringAsync(); }
        catch { /* игнорируем ошибки чтения тела */ }

        var msg = string.IsNullOrWhiteSpace(body) ? null : body;

        ApiException ex = response.StatusCode switch
        {
            HttpStatusCode.BadRequest => new ApiException(400,
                msg ?? "Некорректные данные запроса."),
            HttpStatusCode.Unauthorized => new UnauthorizedException(
                msg ?? "Пользователь не авторизован."),
            HttpStatusCode.Forbidden => new ForbiddenException(
                msg ?? "Доступ запрещён."),
            HttpStatusCode.NotFound => new NotFoundException(
                msg ?? "Ресурс не найден."),
            HttpStatusCode.MethodNotAllowed => new ReadOnlyModeException(
                msg ?? "Сервер запущен в режиме только для чтения."),
            HttpStatusCode.Conflict => new ConflictException(
                msg ?? "Конфликт данных."),
            (HttpStatusCode)418 => new PaymentFailedException(
                msg ?? "Оплата не прошла. Попробуйте ещё раз."),
            HttpStatusCode.InternalServerError => new ServerException(
                msg ?? "Внутренняя ошибка сервера."),
            _ => new ApiException((int)response.StatusCode,
                msg ?? $"Неожиданный статус ответа: {(int)response.StatusCode}."),
        };

        FileLogger.Error(
            $"HTTP {(int)response.StatusCode} {response.StatusCode} от сервера",
            ex,
            BuildContext(request, requestBodyJson, response, body));

        throw ex;
    }

    /// <summary>
    /// Собирает структуру контекста для лога: операция, URL, тело запроса,
    /// статус ответа и тело ответа.
    /// </summary>
    private static Dictionary<string, string?> BuildContext(
        HttpRequestMessage request,
        string? requestBodyJson,
        HttpResponseMessage? response = null,
        string? responseBody = null,
        (string Key, string? Value)? extra = null)
    {
        var dict = new Dictionary<string, string?>
        {
            ["Operation"] = $"{request.Method} {request.RequestUri}",
            ["Method"]    = request.Method.Method,
            ["Url"]       = request.RequestUri?.ToString(),
            ["RequestBody"] = requestBodyJson ?? "<empty>",
        };

        if (response is not null)
        {
            dict["ResponseStatus"] =
                $"{(int)response.StatusCode} {response.StatusCode}";
            dict["ResponseBody"] =
                string.IsNullOrEmpty(responseBody) ? "<empty>" : responseBody;
        }

        if (extra is { } e)
            dict[e.Key] = e.Value;

        return dict;
    }
}
