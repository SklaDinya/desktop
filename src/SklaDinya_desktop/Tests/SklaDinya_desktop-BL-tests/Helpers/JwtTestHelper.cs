namespace SklaDinya_desktop_BL_tests.Helpers;

/// <summary>
/// Строитель минимальных JWT для тестов.
///
/// Кодирование: обычный Base64 с паддингом — именно так работает
/// JwtHelper.DecodeBase64Url (Convert.FromBase64String, без URL-замен символов).
///
/// Имена полей и регистр значений совпадают с тем, что читает JwtHelper:
///   userId       — Guid
///   userRole     — PascalCase (Enum.TryParse без ignoreCase): "Client", "StorageOperator", "Admin"
///   storageId    — Guid, только для операторов
///   OperatorRole — PascalCase: "MainOperator", "OrdinaryOperator", только для операторов
/// </summary>
internal static class JwtTestHelper
{
    public static string Build(string payloadJson)
    {
        var header  = Base64Encode("{\"alg\":\"HS256\",\"typ\":\"JWT\"}"u8.ToArray());
        var payload = Base64Encode(System.Text.Encoding.UTF8.GetBytes(payloadJson));
        return $"{header}.{payload}.fakesignature";
    }

    /// <summary>JWT с ролью Client.</summary>
    public static string ClientToken()
        => Build("""{"userId":"00000000-0000-0000-0000-000000000001","userRole":"Client"}""");

    /// <summary>JWT с ролью Admin.</summary>
    public static string AdminToken()
        => Build("""{"userId":"00000000-0000-0000-0000-000000000002","userRole":"Admin"}""");

    /// <summary>JWT оператора — поле "OperatorRole" совпадает с TryGetProperty("OperatorRole") в JwtHelper.</summary>
    public static string OperatorToken()
        => Build("""{"userId":"00000000-0000-0000-0000-000000000003","userRole":"StorageOperator","storageId":"00000000-0000-0000-0000-000000000010","operatorRole":"MainOperator"}""");

    private static string Base64Encode(byte[] bytes)
        => Convert.ToBase64String(bytes);
}
