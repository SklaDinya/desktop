using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_UI_component.Controls;
using SklaDinya_desktop_UI_component.Helpers;

namespace SklaDinya_desktop_UI_component.Screens.Admin;

public class UsersManagementTab : UserControl
{
    private readonly DataGridView _grid;
    private readonly RoundedTextBox _searchField;

    public UsersManagementTab()
    {
        Dock = DockStyle.Fill;
        BackColor = AppTheme.Background;

        var title = new Label { Text = "Управление пользователями", Font = AppTheme.FontTitle, ForeColor = AppTheme.Primary, Dock = DockStyle.Top, Height = 50, Padding = new Padding(24, 14, 0, 0) };

        // Тулбар с отступом слева 24px
        var toolbar = new Panel { Dock = DockStyle.Top, Height = 52 };
        _searchField = new RoundedTextBox { Placeholder = "Поиск по имени...", Width = 220, Height = 34, Location = new Point(24, 8) };
        var searchBtn = new RoundedButton { Text = "Найти", BackColor = AppTheme.Primary, Size = new Size(90, 34), Location = new Point(256, 9) };
        searchBtn.Click += async (_, _) => await LoadAsync();
        var banBtn = new RoundedButton { Text = "Заблокировать", BackColor = AppTheme.Danger, Size = new Size(150, 34), Location = new Point(358, 9) };
        banBtn.Click += OnBanClick;
        var createBtn = new RoundedButton { Text = "Создать", BackColor = AppTheme.Secondary, Size = new Size(110, 34), Location = new Point(520, 9) };
        createBtn.Click += OnCreateClick;
        toolbar.Controls.AddRange([_searchField, searchBtn, banBtn, createBtn]);

        _grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            BackgroundColor = AppTheme.PanelBackground, BorderStyle = BorderStyle.None,
            Font = AppTheme.FontRegular, RowHeadersVisible = false,
            ColumnHeadersDefaultCellStyle = { BackColor = AppTheme.Primary, ForeColor = Color.White, Font = AppTheme.FontMedium },
            EnableHeadersVisualStyles = false,
        };
        _grid.Columns.AddRange(
            new DataGridViewTextBoxColumn { HeaderText = "ID", Visible = false },
            new DataGridViewTextBoxColumn { HeaderText = "Логин" },
            new DataGridViewTextBoxColumn { HeaderText = "Имя" },
            new DataGridViewTextBoxColumn { HeaderText = "Почта" },
            new DataGridViewTextBoxColumn { HeaderText = "Роль" },
            new DataGridViewTextBoxColumn { HeaderText = "Заблокирован" }
        );

        var gridWrapper = new Panel { Dock = DockStyle.Fill, Padding = new Padding(24, 0, 24, 16) };
        gridWrapper.Controls.Add(_grid);

        Controls.Add(gridWrapper);
        Controls.Add(toolbar);
        Controls.Add(title);

        Load += async (_, _) => await LoadAsync();
    }

    private async Task LoadAsync()
    {
        var query = new UserSearchQuery { Name = string.IsNullOrWhiteSpace(_searchField.Text) ? null : _searchField.Text, PageNumber = 0, PageSize = 100 };
        var users = await ErrorHelper.TryAsync(() => ServiceLocator.UserService.GetUsersAsync(query));
        if (users is null) return;
        _grid.Rows.Clear();
        foreach (var u in users)
        {
            var roleText = u.Role switch { UserRole.Client => "Клиент", UserRole.StorageOperator => "Оператор", UserRole.Admin => "Админ", _ => u.Role.ToString() };
            _grid.Rows.Add(u.Id.ToString(), u.Username, u.Name, u.Email ?? "—", roleText, u.Banned ? "Да" : "Нет");
        }
    }

    private async void OnBanClick(object? sender, EventArgs e)
    {
        if (_grid.CurrentRow is null) return;
        if (!Guid.TryParse(_grid.CurrentRow.Cells[0].Value?.ToString(), out var id)) return;
        if (MessageBox.Show("Заблокировать?", "Подтверждение", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
        await ErrorHelper.TryAsync(() => ServiceLocator.UserService.UpdateUserAsync(id, new UserUpdateForm { Banned = true }), "Заблокирован.");
        await LoadAsync();
    }

    private void OnCreateClick(object? sender, EventArgs e)
    {
        using var d = new CreateUserDialog();
        if (d.ShowDialog() == DialogResult.OK) _ = LoadAsync();
    }
}

internal class CreateUserDialog : Form
{
    private readonly RoundedTextBox _username, _password, _name, _email;
    private readonly ComboBox _role;

    public CreateUserDialog()
    {
        Text = "Новый пользователь"; Size = new Size(400, 370); StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog; MaximizeBox = false; MinimizeBox = false; BackColor = AppTheme.Background;
        int y = 16;
        _username = F("Логин", ref y); _password = F("Пароль", ref y); _name = F("Имя", ref y); _email = F("Почта", ref y);
        Controls.Add(new Label { Text = "Роль:", Font = AppTheme.FontSmall, AutoSize = true, Location = new Point(20, y) }); y += 20;
        _role = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(20, y), Width = 340, Font = AppTheme.FontRegular };
        _role.Items.AddRange(["Клиент", "Оператор", "Администратор"]); _role.SelectedIndex = 0; Controls.Add(_role); y += 44;
        var ok = new RoundedButton { Text = "Создать", BackColor = AppTheme.Secondary, Width = 340, Location = new Point(20, y) };
        ok.Click += async (_, _) =>
        {
            var role = _role.SelectedIndex switch { 1 => UserRole.StorageOperator, 2 => UserRole.Admin, _ => UserRole.Client };
            var r = await ErrorHelper.TryAsync(() => ServiceLocator.UserService.CreateUserAsync(new UserCreateForm { Username = _username.Text, Password = _password.Text, Name = _name.Text, Email = string.IsNullOrWhiteSpace(_email.Text) ? null : _email.Text, Role = role }), "Создан.");
            if (r) { DialogResult = DialogResult.OK; Close(); }
        };
        Controls.Add(ok);
    }
    private RoundedTextBox F(string p, ref int y) { var f = new RoundedTextBox { Placeholder = p, Width = 340, Location = new Point(20, y) }; Controls.Add(f); y += 48; return f; }
}
