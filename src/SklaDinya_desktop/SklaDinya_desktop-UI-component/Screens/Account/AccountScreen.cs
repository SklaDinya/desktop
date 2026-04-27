using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_UI_component.Controls;
using SklaDinya_desktop_UI_component.Helpers;
using SklaDinya_desktop_UI_component.Screens.Admin;
using SklaDinya_desktop_UI_component.Screens.Storage;

namespace SklaDinya_desktop_UI_component.Screens.Account;

/// <summary>
/// Личный кабинет пользователя с вертикальным боковым меню.
/// Набор вкладок зависит от роли (Client / StorageOperator / Admin).
/// </summary>
public class AccountScreen : UserControl
{
    private readonly Panel _sideMenu;
    private readonly Panel _contentPanel;
    private readonly List<SideMenuItem> _menuItems = [];
    private readonly Dictionary<string, Func<UserControl>> _screenFactories = new();

    public AccountScreen()
    {
        Dock = DockStyle.Fill;
        BackColor = AppTheme.Background;

        // ── Боковое меню ────────────────────────────────────────────────
        _sideMenu = new Panel
        {
            Dock = DockStyle.Left,
            Width = AppTheme.SideMenuWidth,
            BackColor = AppTheme.PanelBackground,
            Padding = new Padding(0, 12, 0, 0),
        };

        var menuTitle = new Label
        {
            Text = "Личный кабинет",
            Font = AppTheme.FontHeader,
            ForeColor = AppTheme.Primary,
            Dock = DockStyle.Top,
            Height = 44,
            Padding = new Padding(16, 12, 0, 0),
        };
        _sideMenu.Controls.Add(menuTitle);

        // ── Контент ─────────────────────────────────────────────────────
        _contentPanel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(0),
        };

        Controls.Add(_contentPanel);
        Controls.Add(_sideMenu);

        BuildMenu();
    }

    private void BuildMenu()
    {
        // Очистить предыдущие пункты
        foreach (var item in _menuItems)
            _sideMenu.Controls.Remove(item);
        _menuItems.Clear();
        _screenFactories.Clear();

        // Общие для всех ролей
        AddMenuItem("Мои данные", () => new ProfileTab());
        AddMenuItem("Активные бронирования", () => new ActiveBookingsTab());
        AddMenuItem("История бронирований", () => new BookingHistoryTab());
        AddMenuItem("Создать пункт", () => new CreateStorageTab());

        var role = ServiceLocator.SessionService.CurrentRole;

        if (role is UserRole.StorageOperator)
        {
            AddMenuItem("Мой пункт хранения", () => new MyStorageInfoTab());
            AddMenuItem("Ячейки пункта", () => new MyStorageCellsTab());
            AddMenuItem("Бронирования пункта", () => new MyStorageBookingsTab());
            AddMenuItem("Операторы пункта", () => new MyStorageOperatorsTab());
        }

        if (role is UserRole.Admin)
        {
            AddMenuItem("Управление пользователями", () => new UsersManagementTab());
            AddMenuItem("Заявки на пункты", () => new StorageApplicationsTab());
        }

        // Вставить пункты в меню (reverse, потому что Dock=Top)
        for (int i = _menuItems.Count - 1; i >= 0; i--)
            _sideMenu.Controls.Add(_menuItems[i]);
        _sideMenu.Controls.SetChildIndex(_sideMenu.Controls[^1], 0); // title first

        if (_menuItems.Count > 0)
            SelectMenuItem(_menuItems[0]);
    }

    private void AddMenuItem(string title, Func<UserControl> factory)
    {
        var item = new SideMenuItem(title);
        item.ItemClicked += (s, _) => SelectMenuItem((SideMenuItem)s!);
        _menuItems.Add(item);
        _screenFactories[title] = factory;
    }

    private void SelectMenuItem(SideMenuItem selected)
    {
        foreach (var item in _menuItems)
            item.IsSelected = item == selected;

        _contentPanel.Controls.Clear();
        if (_screenFactories.TryGetValue(selected.Title, out var factory))
        {
            var screen = factory();
            screen.Dock = DockStyle.Fill;
            _contentPanel.Controls.Add(screen);
        }
    }
}
