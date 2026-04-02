using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_DA_component.Dtos;

namespace SklaDinya_desktop_DA_component.Mapping;

internal static partial class Mapper
{
    // ── BL → DA ────────────────────────────────────────────────────────────
    // Auth не возвращает модели — сервер отдаёт JWT-строку напрямую.
    // Направление DA → BL здесь отсутствует намеренно.

    public static LoginRequest ToLoginRequest(LoginForm form) =>
        new(form.Username, form.Password);

    public static RegistrationRequest ToRegistrationRequest(RegistrationForm form) =>
        new(form.Username, form.Password, form.Name, form.Email);
}
