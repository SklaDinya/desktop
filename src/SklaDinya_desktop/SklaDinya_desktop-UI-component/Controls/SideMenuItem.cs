using SklaDinya_desktop_UI_component.Helpers;

namespace SklaDinya_desktop_UI_component.Controls;

/// <summary>
/// Пункт бокового меню с подсветкой при выборе.
/// Текст переносится если не помещается в ширину.
/// </summary>
public class SideMenuItem : Panel
{
    private readonly Label _label;
    private bool _isSelected;

    public string Title
    {
        get => _label.Text;
        set => _label.Text = value;
    }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            BackColor = value ? AppTheme.Secondary : Color.Transparent;
            _label.ForeColor = value ? AppTheme.TextLight : AppTheme.TextDark;
        }
    }

    public event EventHandler? ItemClicked;

    public SideMenuItem(string title)
    {
        Height = 40;
        Dock = DockStyle.Top;
        Cursor = Cursors.Hand;
        Padding = new Padding(16, 4, 8, 4);

        _label = new Label
        {
            Text = title,
            Font = AppTheme.FontRegular,
            ForeColor = AppTheme.TextDark,
            AutoSize = false,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            AutoEllipsis = true,
        };
        _label.Click += (_, _) => ItemClicked?.Invoke(this, EventArgs.Empty);
        Click += (_, _) => ItemClicked?.Invoke(this, EventArgs.Empty);
        Controls.Add(_label);
    }
}

/// <summary>
/// Разделитель групп в боковом меню: заголовок группы + серая линия
/// </summary>
public class SideMenuGroupHeader : Panel
{
    public SideMenuGroupHeader(string groupName)
    {
        Height = 32;
        Dock = DockStyle.Top;
        Padding = new Padding(16, 10, 8, 2);

        var lbl = new Label
        {
            Text = groupName.ToUpper(),
            Font = new Font("Segoe UI", 8f, FontStyle.Bold),
            ForeColor = AppTheme.TextMuted,
            AutoSize = false,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.BottomLeft,
        };
        Controls.Add(lbl);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        using var pen = new Pen(AppTheme.Border, 1f);
        e.Graphics.DrawLine(pen, 16, 8, Width - 16, 8);
    }
}
