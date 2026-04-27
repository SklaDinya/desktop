using SklaDinya_desktop_UI_component.Helpers;
using System.Drawing.Drawing2D;

namespace SklaDinya_desktop_UI_component.Controls;

/// <summary>
/// Кнопка со скруглёнными краями
/// </summary>
public class RoundedButton : Button
{
    private Color _hoverBackColor;
    private bool _isHovered;

    public RoundedButton()
    {
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        BackColor = AppTheme.Primary;
        ForeColor = AppTheme.TextLight;
        Font = AppTheme.FontMedium;
        Cursor = Cursors.Hand;
        Height = AppTheme.ButtonHeight;
        _hoverBackColor = ControlPaint.Dark(BackColor, 0.1f);
    }

    protected override void OnBackColorChanged(EventArgs e)
    {
        base.OnBackColorChanged(e);
        _hoverBackColor = ControlPaint.Dark(BackColor, 0.1f);
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        _isHovered = true;
        Invalidate();
        base.OnMouseEnter(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        _isHovered = false;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        e.Graphics.Clear(Parent?.BackColor ?? AppTheme.Background);

        var rect = new Rectangle(0, 0, Width - 1, Height - 1);
        using var path = RoundedRenderer.RoundedRect(rect, AppTheme.CornerRadius);
        using var brush = new SolidBrush(_isHovered ? _hoverBackColor : BackColor);

        e.Graphics.FillPath(brush, path);

        var textSize = e.Graphics.MeasureString(Text, Font);
        float x = (Width - textSize.Width) / 2;
        float y = (Height - textSize.Height) / 2;
        using var textBrush = new SolidBrush(ForeColor);
        e.Graphics.DrawString(Text, Font, textBrush, x, y);
    }
}
