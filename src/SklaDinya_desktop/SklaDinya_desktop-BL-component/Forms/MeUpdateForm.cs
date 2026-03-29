namespace SklaDinya_desktop_BL_component.Forms;

/// <summary>
/// Форма для обновления данных своего аккаунта
/// </summary>
public class MeUpdateForm
{
    /// <summary>Новый логин (необязательно)</summary>
    public string? Username { get; set; }

    /// <summary>Старый пароль (требуется при смене пароля)</summary>
    public string? OldPassword { get; set; }

    /// <summary>Новый пароль (необязательно)</summary>
    public string? NewPassword { get; set; }

    /// <summary>Новое имя (необязательно)</summary>
    public string? Name { get; set; }

    /// <summary>Новая почта (необязательно)</summary>
    public string? Email { get; set; }
}
