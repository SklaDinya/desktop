using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_UI_component.Controls;
using SklaDinya_desktop_UI_component.Helpers;
using SklaDinya_desktop_UI_component.Screens.Admin;
using SklaDinya_desktop_UI_component.Screens.Storage;

namespace SklaDinya_desktop_UI_component.Screens.Account;

/// <summary>
/// Личный кабинет с боковым меню, разделённым на группы по роли.
/// </summary>
public class AccountScreen : UserControl
{
    private readonly Panel _sideMenu;
    private readonly Panel _contentPanel;
    private readonly List<SideMenuItem> _menuItems = [];
    private readonly Dictionary<string, Func<UserControl>> _screenFactories = new();
    private string? _initialTab;

    public AccountScreen(string? initialTab = null)
    {
        _initialTab = initialTab;
        Dock = DockStyle.Fill;
        BackColor = AppTheme.Background;

        _sideMenu = new Panel
        {
            Dock = DockStyle.Left,
            Width = 230,
            BackColor = AppTheme.PanelBackground,
            AutoScroll = true,
            Padding = new Padding(0, 0, 0, 8),
        };

        _contentPanel = new Panel { Dock = DockStyle.Fill };

        Controls.Add(_contentPanel);
        Controls.Add(_sideMenu);

        BuildMenu();
    }

    private void BuildMenu()
    {
        _sideMenu.Controls.Clear();
        _menuItems.Clear();
        _screenFactories.Clear();

        var role = ServiceLocator.SessionService.CurrentRole;
        var controls = new List<Control>();

        // ── Группа: Личные данные ───────────────────────────────────────
        controls.Add(new SideMenuGroupHeader("Личные данные"));
        controls.Add(MakeItem("Мои данные", () => new ProfileTab()));

        // ── Группа: Бронирования ────────────────────────────────────────
        controls.Add(new SideMenuGroupHeader("Бронирования"));
        controls.Add(MakeItem("Активные", () => new ActiveBookingsTab()));
        controls.Add(MakeItem("История", () => new BookingHistoryTab()));

        // ── Группа: Пункты ──────────────────────────────────────────────
        controls.Add(new SideMenuGroupHeader("Пункты"));
        controls.Add(MakeItem("Создать пункт", () => new CreateStorageTab()));

        if (role is UserRole.StorageOperator)
        {
            controls.Add(MakeItem("Мой пункт", () => new MyStorageInfoTab()));
            controls.Add(MakeItem("Ячейки", () => new MyStorageCellsTab()));
            controls.Add(MakeItem("Бронирования пункта", () => new MyStorageBookingsTab()));
            controls.Add(MakeItem("Операторы", () => new MyStorageOperatorsTab()));
        }

        // ── Группа: Администрирование ───────────────────────────────────
        if (role is UserRole.Admin)
        {
            controls.Add(new SideMenuGroupHeader("Администрирование"));
            controls.Add(MakeItem("Пользователи", () => new UsersManagementTab()));
            controls.Add(MakeItem("Заявки на пункты", () => new StorageApplicationsTab()));
        }

        // Добавляем в обратном порядке (Dock.Top)
        for (int i = controls.Count - 1; i >= 0; i--)
        {
            controls[i].Dock = DockStyle.Top;
            _sideMenu.Controls.Add(controls[i]);
        }

        // Выбрать начальную вкладку
        SideMenuItem? target = null;
        if (_initialTab is not null)
            target = _menuItems.FirstOrDefault(m => m.Title == _initialTab);
        target ??= _menuItems.FirstOrDefault();
        if (target is not null)
            SelectMenuItem(target);
    }

    private SideMenuItem MakeItem(string title, Func<UserControl> factory)
    {
        var item = new SideMenuItem(title);
        item.ItemClicked += (s, _) => SelectMenuItem((SideMenuItem)s!);
        _menuItems.Add(item);
        _screenFactories[title] = factory;
        return item;
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
