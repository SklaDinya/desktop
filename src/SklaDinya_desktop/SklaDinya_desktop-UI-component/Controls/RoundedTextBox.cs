using SklaDinya_desktop_UI_component.Helpers;
using System.Drawing.Drawing2D;

namespace SklaDinya_desktop_UI_component.Controls;

/// <summary>
/// Текстовое поле со скруглёнными краями и видимым placeholder.
/// </summary>
public class RoundedTextBox : UserControl
{
    private readonly TextBox _innerTextBox;
    private string _placeholder = string.Empty;
    private bool _isPasswordField;

    public RoundedTextBox()
    {
        BackColor = AppTheme.FieldBackground;
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);

        _innerTextBox = new TextBox
        {
            BorderStyle = BorderStyle.None,
            Font = AppTheme.FontRegular,
            BackColor = AppTheme.FieldBackground,
            ForeColor = AppTheme.TextDark,
        };
        _innerTextBox.TextChanged += (_, _) => { OnTextChanged(EventArgs.Empty); UpdateVisibility(); };
        _innerTextBox.GotFocus += (_, _) => { _innerTextBox.Visible = true; Invalidate(); };
        _innerTextBox.LostFocus += (_, _) => { UpdateVisibility(); Invalidate(); };
        Controls.Add(_innerTextBox);

        Click += (_, _) => { _innerTextBox.Visible = true; _innerTextBox.Focus(); };

        Height = AppTheme.FieldHeight;
    }

    public override string Text
    {
        get => _innerTextBox.Text;
        set { _innerTextBox.Text = value ?? string.Empty; UpdateVisibility(); }
    }

    public string Placeholder
    {
        get => _placeholder;
        set { _placeholder = value ?? string.Empty; Invalidate(); }
    }

    public bool UsePasswordChar
    {
        get => _isPasswordField;
        set { _isPasswordField = value; _innerTextBox.UseSystemPasswordChar = value; }
    }

    public bool ReadOnly
    {
        get => _innerTextBox.ReadOnly;
        set => _innerTextBox.ReadOnly = value;
    }

    private bool ShowPlaceholder => !_innerTextBox.Focused && string.IsNullOrEmpty(_innerTextBox.Text) && _placeholder.Length > 0;

    private void UpdateVisibility()
    {
        _innerTextBox.Visible = !ShowPlaceholder;
        Invalidate();
    }

    protected override void OnLayout(LayoutEventArgs e)
    {
        base.OnLayout(e);
        if (_innerTextBox is null) return;
        _innerTextBox.Location = new Point(14, (Height - _innerTextBox.Height) / 2);
        _innerTextBox.Width = Width - 28;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // Фон родителя для прозрачности углов
        var parentBg = Parent?.BackColor ?? AppTheme.Background;
        using (var bgBrush = new SolidBrush(parentBg))
            g.FillRectangle(bgBrush, ClientRectangle);

        // Скруглённый фон поля
        var rect = new Rectangle(0, 0, Width - 1, Height - 1);
        using var path = RoundedRenderer.RoundedRect(rect, AppTheme.CornerRadius);
        using (var fillBrush = new SolidBrush(AppTheme.FieldBackground))
            g.FillPath(fillBrush, path);

        // Граница
        var borderColor = _innerTextBox is not null && _innerTextBox.Focused ? AppTheme.Primary : AppTheme.Border;
        using (var pen = new Pen(borderColor, 1.5f))
            g.DrawPath(pen, path);

        // Placeholder
        if (ShowPlaceholder)
        {
            var phY = (Height - g.MeasureString(_placeholder, AppTheme.FontRegular).Height) / 2;
            using var brush = new SolidBrush(AppTheme.TextMuted);
            g.DrawString(_placeholder, AppTheme.FontRegular, brush, 14, phY);
        }
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        Invalidate();
    }

    protected override void OnParentChanged(EventArgs e)
    {
        base.OnParentChanged(e);
        UpdateVisibility();
        Invalidate();
    }
}
