using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_DA_component.Dtos;

namespace SklaDinya_desktop_DA_component.Mapping;

internal static partial class Mapper
{
    // ── BL → DA ────────────────────────────────────────────────────────────
    // Payment не возвращает собственных моделей — ответ маппится через ToBookingList.
    // Направление DA → BL делегируется BookingMapper.ToBookingList.

    public static PaymentNoopRequest ToPaymentNoopRequest(PaymentForm form) =>
        new(form.Receipt);

    public static PaymentRandomRequest ToPaymentRandomRequest(PaymentForm form) =>
        new(form.Receipt);
}
