using SklaDinya_desktop_UI_component.Helpers;
using System.Drawing.Drawing2D;

namespace SklaDinya_desktop_UI_component.Controls;

/// <summary>
/// Кнопка со скруглёнными краями, реализованная как UserControl.
/// </summary>
public class RoundedButton : UserControl
{
    private bool _isHovered;
    private bool _isPressed;
    private string _text = string.Empty;
    private Color _backFillColor;

    public RoundedButton()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.SupportsTransparentBackColor,
            true);

        BackColor = Color.Transparent;
        _backFillColor = AppTheme.Primary;
        ForeColor = AppTheme.TextLight;
        Font = AppTheme.FontMedium;
        Cursor = Cursors.Hand;
        Height = AppTheme.ButtonHeight;
    }

    /// <summary>Цвет заливки кнопки (не путать с BackColor, который прозрачный)</summary>
    public Color ButtonColor
    {
        get => _backFillColor;
        set { _backFillColor = value; Invalidate(); }
    }

    public new Color BackColor
    {
        get => base.BackColor;
        set
        {
            if (value != Color.Transparent)
                _backFillColor = value;
            base.BackColor = Color.Transparent;
            Invalidate();
        }
    }

    public override string Text
    {
        get => _text;
        set { _text = value ?? string.Empty; Invalidate(); }
    }

    public new bool Enabled
    {
        get => base.Enabled;
        set { base.Enabled = value; Invalidate(); }
    }

    protected override void OnMouseEnter(EventArgs e) { _isHovered = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { _isHovered = false; _isPressed = false; Invalidate(); base.OnMouseLeave(e); }
    protected override void OnMouseDown(MouseEventArgs e) { _isPressed = true; Invalidate(); base.OnMouseDown(e); }
    protected override void OnMouseUp(MouseEventArgs e) { _isPressed = false; Invalidate(); base.OnMouseUp(e); }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        var bg = ResolveOpaqueBackground();
        using (var bgBrush = new SolidBrush(bg))
            g.FillRectangle(bgBrush, ClientRectangle);

        var rect = new Rectangle(0, 0, Width - 1, Height - 1);
        using var path = RoundedRenderer.RoundedRect(rect, AppTheme.CornerRadius);

        var color = _backFillColor;
        if (!Enabled) color = ControlPaint.LightLight(color);
        else if (_isPressed) color = ControlPaint.Dark(color, 0.15f);
        else if (_isHovered) color = ControlPaint.Dark(color, 0.06f);

        using (var brush = new SolidBrush(color))
            g.FillPath(brush, path);

        // Текст по центру
        var textSize = g.MeasureString(_text, Font);
        var x = (Width - textSize.Width) / 2;
        var y = (Height - textSize.Height) / 2;
        using (var brush = new SolidBrush(ForeColor))
            g.DrawString(_text, Font, brush, x, y);
    }

    private Color ResolveOpaqueBackground()
    {
        var p = Parent;
        while (p is not null)
        {
            if (p.BackColor.A != 0)
            {
                if (Parent is UserControl uc &&
                    uc.BackColor.A == 0 &&
                    !ReferenceEquals(uc, p))
                {
                    return AppTheme.PanelBackground;
                }
                return p.BackColor;
            }
            p = p.Parent;
        }
        return AppTheme.Background;
    }
}
