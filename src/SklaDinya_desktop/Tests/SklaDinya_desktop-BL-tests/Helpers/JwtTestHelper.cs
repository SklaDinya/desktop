namespace SklaDinya_desktop_BL_tests.Helpers;

/// <summary>
/// Вспомогательный класс для создания минимальных JWT в тестах.
/// Сигнатура не валидируется SessionService, поэтому используется заглушка.
/// </summary>
internal static class JwtTestHelper
{
    /// <summary>
    /// Создаёт JWT вида header.payload.signature с произвольным JSON-payload.
    /// </summary>
    public static string Build(string payloadJson)
    {
        var header = Base64UrlEncode("{\"alg\":\"HS256\",\"typ\":\"JWT\"}"u8.ToArray());
        var payload = Base64UrlEncode(System.Text.Encoding.UTF8.GetBytes(payloadJson));
        return $"{header}.{payload}.fakesignature";
    }

    /// <summary>JWT с ролью Client (минимально необходимый payload).</summary>
    public static string ClientToken()
        => Build("""{"userId":"00000000-0000-0000-0000-000000000001","userRole":"Client"}""");

    /// <summary>JWT с ролью Admin.</summary>
    public static string AdminToken()
        => Build("""{"userId":"00000000-0000-0000-0000-000000000002","userRole":"Admin"}""");

    /// <summary>JWT оператора с дополнительными полями storageId и role.</summary>
    public static string OperatorToken()
        => Build("""{"userId":"00000000-0000-0000-0000-000000000003","userRole":"StorageOperator","storageId":"00000000-0000-0000-0000-000000000010","role":"MainOperator"}""");

    private static string Base64UrlEncode(byte[] bytes)
        => Convert.ToBase64String(bytes)
                  .TrimEnd('=')
                  .Replace('+', '-')
                  .Replace('/', '_');
}
