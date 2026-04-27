using SklaDinya_desktop_UI_component.Helpers;

namespace SklaDinya_desktop_UI_component.Controls;

/// <summary>
/// Пункт бокового меню с подсветкой при выборе
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
        Height = 44;
        Cursor = Cursors.Hand;
        Dock = DockStyle.Top;
        Padding = new Padding(16, 0, 8, 0);

        _label = new Label
        {
            Text = title,
            Font = AppTheme.FontMedium,
            ForeColor = AppTheme.TextDark,
            AutoSize = false,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
        };
        _label.Click += (_, _) => ItemClicked?.Invoke(this, EventArgs.Empty);
        Click += (_, _) => ItemClicked?.Invoke(this, EventArgs.Empty);
        Controls.Add(_label);
    }
}
