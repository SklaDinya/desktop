using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_UI_component.Controls;
using SklaDinya_desktop_UI_component.Helpers;

namespace SklaDinya_desktop_UI_component.Screens.Admin;

/// <summary>
/// Вкладка «Управление пользователями» — для администратора
/// </summary>
public class UsersManagementTab : UserControl
{
    private readonly DataGridView _grid;
    private readonly RoundedTextBox _searchField;
    private readonly RoundedButton _searchButton;
    private readonly RoundedButton _banButton;
    private readonly RoundedButton _createButton;

    public UsersManagementTab()
    {
        Dock = DockStyle.Fill;
        BackColor = AppTheme.Background;

        var title = new Label
        {
            Text = "Управление пользователями",
            Font = AppTheme.FontTitle,
            ForeColor = AppTheme.Primary,
            Dock = DockStyle.Top,
            Height = 50,
            Padding = new Padding(24, 14, 0, 0),
        };

        var toolbar = new Panel { Dock = DockStyle.Top, Height = 50, Padding = new Padding(24, 8, 24, 8) };
        _searchField = new RoundedTextBox
        {
            Placeholder = "Поиск по имени...",
            Width = 220,
            Location = new Point(0, 6),
        };
        _searchButton = new RoundedButton
        {
            Text = "Найти",
            BackColor = AppTheme.Primary,
            Width = 90, Height = 34,
            Location = new Point(232, 8),
        };
        _searchButton.Click += async (_, _) => await LoadAsync();

        _banButton = new RoundedButton
        {
            Text = "Заблокировать",
            BackColor = AppTheme.Danger,
            Width = 150, Height = 34,
            Location = new Point(336, 8),
        };
        _banButton.Click += OnBanClick;

        _createButton = new RoundedButton
        {
            Text = "Создать пользователя",
            BackColor = AppTheme.Secondary,
            Width = 180, Height = 34,
            Location = new Point(500, 8),
        };
        _createButton.Click += OnCreateClick;

        toolbar.Controls.AddRange([_searchField, _searchButton, _banButton, _createButton]);

        _grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            BackgroundColor = AppTheme.PanelBackground,
            BorderStyle = BorderStyle.None,
            Font = AppTheme.FontRegular,
            RowHeadersVisible = false,
        };
        _grid.Columns.AddRange(
            new DataGridViewTextBoxColumn { HeaderText = "ID", Visible = false },
            new DataGridViewTextBoxColumn { HeaderText = "Логин" },
            new DataGridViewTextBoxColumn { HeaderText = "Имя" },
            new DataGridViewTextBoxColumn { HeaderText = "Почта" },
            new DataGridViewTextBoxColumn { HeaderText = "Роль" },
            new DataGridViewTextBoxColumn { HeaderText = "Заблокирован" }
        );

        Controls.Add(_grid);
        Controls.Add(toolbar);
        Controls.Add(title);

        Load += async (_, _) => await LoadAsync();
    }

    private async Task LoadAsync()
    {
        var query = new UserSearchQuery
        {
            Name = string.IsNullOrWhiteSpace(_searchField.Text) ? null : _searchField.Text,
            PageNumber = 0,
            PageSize = 100,
        };

        var users = await ErrorHelper.TryAsync(
            () => ServiceLocator.UserService.GetUsersAsync(query));

        if (users is null) return;

        _grid.Rows.Clear();
        foreach (var u in users)
        {
            var roleText = u.Role switch
            {
                UserRole.Client => "Клиент",
                UserRole.StorageOperator => "Оператор",
                UserRole.Admin => "Администратор",
                _ => u.Role.ToString()
            };
            _grid.Rows.Add(u.Id.ToString(), u.Username, u.Name, u.Email ?? "—", roleText, u.Banned ? "Да" : "Нет");
        }
    }

    private async void OnBanClick(object? sender, EventArgs e)
    {
        if (_grid.CurrentRow is null) return;
        var idStr = _grid.CurrentRow.Cells[0].Value?.ToString();
        if (!Guid.TryParse(idStr, out var userId)) return;

        var name = _grid.CurrentRow.Cells[2].Value?.ToString() ?? "";
        var confirm = MessageBox.Show(
            $"Заблокировать пользователя «{name}»?",
            "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        var form = new UserUpdateForm { Banned = true };
        await ErrorHelper.TryAsync(
            () => ServiceLocator.UserService.UpdateUserAsync(userId, form),
            "Пользователь заблокирован.");
        await LoadAsync();
    }

    private void OnCreateClick(object? sender, EventArgs e)
    {
        using var dialog = new CreateUserDialog();
        if (dialog.ShowDialog() == DialogResult.OK)
            _ = LoadAsync();
    }
}

/// <summary>
/// Диалог создания пользователя администратором
/// </summary>
internal class CreateUserDialog : Form
{
    private readonly RoundedTextBox _usernameField;
    private readonly RoundedTextBox _passwordField;
    private readonly RoundedTextBox _nameField;
    private readonly RoundedTextBox _emailField;
    private readonly ComboBox _roleCombo;

    public CreateUserDialog()
    {
        Text = "Новый пользователь";
        Size = new Size(400, 380);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = AppTheme.Background;

        int y = 16;
        _usernameField = MakeField("Логин", ref y);
        _passwordField = MakeField("Пароль", ref y);
        _nameField = MakeField("Имя", ref y);
        _emailField = MakeField("Почта", ref y);

        var roleLabel = new Label { Text = "Роль:", Font = AppTheme.FontSmall, AutoSize = true, Location = new Point(20, y) };
        Controls.Add(roleLabel);
        y += 20;
        _roleCombo = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Location = new Point(20, y), Width = 340,
            Font = AppTheme.FontRegular,
        };
        _roleCombo.Items.AddRange(["Клиент", "Оператор", "Администратор"]);
        _roleCombo.SelectedIndex = 0;
        Controls.Add(_roleCombo);
        y += 44;

        var okBtn = new RoundedButton
        {
            Text = "Создать",
            BackColor = AppTheme.Secondary,
            Width = 340,
            Location = new Point(20, y),
        };
        okBtn.Click += OnOkClick;
        Controls.Add(okBtn);
    }

    private RoundedTextBox MakeField(string placeholder, ref int y)
    {
        var f = new RoundedTextBox { Placeholder = placeholder, Width = 340, Location = new Point(20, y) };
        Controls.Add(f);
        y += 48;
        return f;
    }

    private async void OnOkClick(object? sender, EventArgs e)
    {
        var role = _roleCombo.SelectedIndex switch
        {
            1 => UserRole.StorageOperator,
            2 => UserRole.Admin,
            _ => UserRole.Client,
        };
        var form = new UserCreateForm
        {
            Username = _usernameField.Text,
            Password = _passwordField.Text,
            Name = _nameField.Text,
            Email = string.IsNullOrWhiteSpace(_emailField.Text) ? null : _emailField.Text,
            Role = role,
        };

        var ok = await ErrorHelper.TryAsync(
            () => ServiceLocator.UserService.CreateUserAsync(form),
            "Пользователь создан.");

        if (ok)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
