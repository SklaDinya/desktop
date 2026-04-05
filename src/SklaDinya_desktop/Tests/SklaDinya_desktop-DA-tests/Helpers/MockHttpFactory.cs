using System.Net;
using System.Text.Json;
using Moq;
using Moq.Protected;
using SklaDinya_desktop_DA_component.Http;

namespace SklaDinya_desktop_DA_tests.Helpers;

/// <summary>
/// Фабрика ApiClient с замоканным HttpMessageHandler.
/// Принимает анонимные объекты вместо internal DTO — тестовый проект не имеет
/// доступа к internal-типам DA-компонента.
/// </summary>
internal static class MockHttpFactory
{
    /// <summary>
    /// Настройки сериализации совпадают с теми, что использует ApiClient:
    /// camelCase + enum как строки.
    /// </summary>
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy        = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new System.Text.Json.Serialization.JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        },
    };

    public static ApiClient Create(HttpStatusCode statusCode, object? responseBody = null)
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        var content = responseBody is null
            ? new StringContent("")
            : new StringContent(
                JsonSerializer.Serialize(responseBody, JsonOptions),
                System.Text.Encoding.UTF8,
                "application/json");

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content    = content,
            });

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("http://localhost")
        };

        return new ApiClient(httpClient);
    }

    public static ApiClient CreateOk(object? body = null) => Create(HttpStatusCode.OK, body);
    public static ApiClient CreateUnauthorized()          => Create(HttpStatusCode.Unauthorized);
    public static ApiClient CreateNotFound()              => Create(HttpStatusCode.NotFound);
    public static ApiClient CreateConflict()              => Create(HttpStatusCode.Conflict);
    public static ApiClient CreateForbidden()             => Create(HttpStatusCode.Forbidden);
    public static ApiClient CreatePaymentFailed()         => Create((HttpStatusCode)418);
}
