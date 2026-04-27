using SklaDinya_desktop_UI_component.Helpers;
using System.Drawing.Drawing2D;

namespace SklaDinya_desktop_UI_component.Controls;

/// <summary>
/// Текстовое поле со скруглёнными краями.
/// Содержит внутренний TextBox для ввода текста.
/// </summary>
public class RoundedTextBox : UserControl
{
    private readonly TextBox _innerTextBox;
    private string _placeholder = string.Empty;
    private bool _showPlaceholder;

    public RoundedTextBox()
    {
        Height = AppTheme.FieldHeight;
        BackColor = AppTheme.FieldBackground;
        Padding = new Padding(14, 0, 14, 0);
        DoubleBuffered = true;

        _innerTextBox = new TextBox
        {
            BorderStyle = BorderStyle.None,
            Font = AppTheme.FontRegular,
            BackColor = AppTheme.FieldBackground,
            ForeColor = AppTheme.TextDark,
            Anchor = AnchorStyles.Left | AnchorStyles.Right,
        };
        _innerTextBox.TextChanged += (_, _) => OnTextChanged(EventArgs.Empty);
        _innerTextBox.GotFocus += (_, _) => { HidePlaceholder(); Invalidate(); };
        _innerTextBox.LostFocus += (_, _) => { ShowPlaceholderIfEmpty(); Invalidate(); };
        Controls.Add(_innerTextBox);
    }

    public override string Text
    {
        get => _showPlaceholder ? string.Empty : _innerTextBox.Text;
        set
        {
            _innerTextBox.Text = value;
            if (string.IsNullOrEmpty(value) && !_innerTextBox.Focused)
                ShowPlaceholderIfEmpty();
            else
                HidePlaceholder();
        }
    }

    public string Placeholder
    {
        get => _placeholder;
        set
        {
            _placeholder = value;
            if (!_innerTextBox.Focused && string.IsNullOrEmpty(_innerTextBox.Text))
                ShowPlaceholderIfEmpty();
        }
    }

    public bool UsePasswordChar
    {
        get => _innerTextBox.UseSystemPasswordChar;
        set => _innerTextBox.UseSystemPasswordChar = value;
    }

    public bool ReadOnly
    {
        get => _innerTextBox.ReadOnly;
        set => _innerTextBox.ReadOnly = value;
    }

    private void ShowPlaceholderIfEmpty()
    {
        if (string.IsNullOrEmpty(_innerTextBox.Text) && !string.IsNullOrEmpty(_placeholder))
        {
            _showPlaceholder = true;
            _innerTextBox.UseSystemPasswordChar = false;
            _innerTextBox.Text = _placeholder;
            _innerTextBox.ForeColor = AppTheme.TextMuted;
        }
    }

    private void HidePlaceholder()
    {
        if (_showPlaceholder)
        {
            _showPlaceholder = false;
            _innerTextBox.Text = string.Empty;
            _innerTextBox.ForeColor = AppTheme.TextDark;
        }
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
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        e.Graphics.Clear(Parent?.BackColor ?? AppTheme.Background);

        var rect = new Rectangle(0, 0, Width - 1, Height - 1);
        using var path = RoundedRenderer.RoundedRect(rect, AppTheme.CornerRadius);
        using var fillBrush = new SolidBrush(AppTheme.FieldBackground);
        e.Graphics.FillPath(fillBrush, path);

        if (_innerTextBox is null) return;
        var borderColor = _innerTextBox.Focused ? AppTheme.Primary : AppTheme.Border;
        using var pen = new Pen(borderColor, 1.5f);
        e.Graphics.DrawPath(pen, path);
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        Invalidate();
    }
}