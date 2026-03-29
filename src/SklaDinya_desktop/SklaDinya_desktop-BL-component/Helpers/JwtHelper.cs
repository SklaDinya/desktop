using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Models;

namespace SklaDinya_desktop_BL_component.Helpers;

/// <summary>
/// Утилита для извлечения данных из JWT-токена без верификации подписи.
/// Верификация подписи — ответственность бэкенда; десктоп только читает payload.
/// </summary>
public static class JwtHelper
{
    /// <summary>
    /// Распарсить payload JWT и вернуть типизированную модель.
    /// </summary>
    /// <exception cref="ArgumentException">Токен имеет неверный формат.</exception>
    /// <exception cref="InvalidOperationException">Payload не содержит обязательных полей.</exception>
    public static JwtPayload ParsePayload(string token)
    {
        var parts = token.Split('.');
        if (parts.Length != 3)
            throw new ArgumentException("Неверный формат JWT-токена.", nameof(token));

        var payload = DecodeBase64Url(parts[1]);
        using var doc = JsonDocument.Parse(payload);
        var root = doc.RootElement;

        var userId = Guid.Parse(root.GetProperty("userId").GetString()
            ?? throw new InvalidOperationException("JWT не содержит userId."));

        var userRoleStr = root.GetProperty("userRole").GetString()
            ?? throw new InvalidOperationException("JWT не содержит userRole.");

        var userRole = Enum.Parse<UserRole>(userRoleStr);

        Guid? storageId = null;
        if (root.TryGetProperty("storageId", out var storageIdEl) &&
            storageIdEl.ValueKind == JsonValueKind.String)
        {
            storageId = Guid.Parse(storageIdEl.GetString()!);
        }

        OperatorRole? operatorRole = null;
        if (root.TryGetProperty("role", out var roleEl) &&
            roleEl.ValueKind == JsonValueKind.String)
        {
            operatorRole = Enum.Parse<OperatorRole>(roleEl.GetString()!);
        }

        return new JwtPayload
        {
            UserId = userId,
            UserRole = userRole,
            StorageId = storageId,
            OperatorRole = operatorRole,
        };
    }

    private static string DecodeBase64Url(string base64Url)
    {
        // Base64Url -> Base64: заменяем символы и добавляем padding
        var base64 = base64Url
            .Replace('-', '+')
            .Replace('_', '/');

        var padding = (base64.Length % 4) switch
        {
            2 => "==",
            3 => "=",
            _ => ""
        };

        var bytes = Convert.FromBase64String(base64 + padding);
        return Encoding.UTF8.GetString(bytes);
    }
}
