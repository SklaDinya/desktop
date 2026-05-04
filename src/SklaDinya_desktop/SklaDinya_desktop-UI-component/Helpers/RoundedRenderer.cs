using System.Drawing.Drawing2D;

namespace SklaDinya_desktop_UI_component.Helpers;

/// <summary>
/// Утилиты для рисования скруглённых прямоугольников
/// </summary>
public static class RoundedRenderer
{
    public static GraphicsPath RoundedRect(Rectangle bounds, int radius)
    {
        var path = new GraphicsPath();
        int d = radius * 2;

        if (d > bounds.Width) d = bounds.Width;
        if (d > bounds.Height) d = bounds.Height;

        path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
        path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
        path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
        path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }

    public static void ApplyRoundedRegion(Control control, int radius)
    {
        control.Region = new Region(RoundedRect(
            new Rectangle(0, 0, control.Width, control.Height), radius));
    }
}
