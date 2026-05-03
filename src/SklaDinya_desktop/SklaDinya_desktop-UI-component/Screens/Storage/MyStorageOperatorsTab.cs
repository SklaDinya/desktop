using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_UI_component.Controls;
using SklaDinya_desktop_UI_component.Helpers;

namespace SklaDinya_desktop_UI_component.Screens.Storage;

/// <summary>
/// Вкладка «Операторы пункта» — управление сотрудниками
/// </summary>
public class MyStorageOperatorsTab : UserControl
{
    private readonly DataGridView _grid;
    private readonly RoundedButton _addButton;
    private readonly RoundedButton _banButton;
    private readonly RoundedButton _refreshButton;

    public MyStorageOperatorsTab()
    {
        Dock = DockStyle.Fill;
        BackColor = AppTheme.Background;
        // Внешний отступ от бокового меню и краёв страницы.
        Padding = new Padding(24, 0, 24, 24);

        var title = new Label
        {
            Text = "Операторы пункта",
            Font = AppTheme.FontTitle,
            ForeColor = AppTheme.Primary,
            Dock = DockStyle.Top,
            Height = 50,
            Padding = new Padding(0, 14, 0, 0),
        };

        var toolbar = new Panel { Dock = DockStyle.Top, Height = 50, Padding = new Padding(0, 8, 0, 8) };
        _addButton = new RoundedButton
        {
            Text = "Добавить оператора",
            BackColor = AppTheme.Secondary,
            Width = 180, Height = 34,
            Location = new Point(0, 8),
        };
        _addButton.Click += OnAddClick;

        _banButton = new RoundedButton
        {
            Text = "Заблокировать",
            BackColor = AppTheme.Danger,
            Width = 150, Height = 34,
            Location = new Point(192, 8),
        };
        _banButton.Click += OnBanClick;

        _refreshButton = new RoundedButton
        {
            Text = "Обновить",
            BackColor = AppTheme.Primary,
            Width = 110, Height = 34,
            Location = new Point(354, 8),
        };
        _refreshButton.Click += async (_, _) => await LoadAsync();

        toolbar.Controls.AddRange([_addButton, _banButton, _refreshButton]);

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
        var query = new OperatorSearchQuery { PageNumber = 0, PageSize = 100 };
        var operators = await ErrorHelper.TryAsync(
            () => ServiceLocator.OperatorService.GetOperatorsAsync(query));

        if (operators is null) return;

        _grid.Rows.Clear();
        foreach (var op in operators)
        {
            var roleText = op.Role == OperatorRole.MainOperator ? "Главный оператор" : "Оператор";
            _grid.Rows.Add(op.Id.ToString(), op.Username, op.Name, roleText, op.Banned ? "Да" : "Нет");
        }
    }

    private void OnAddClick(object? sender, EventArgs e)
    {
        using var dialog = new AddOperatorDialog();
        if (dialog.ShowDialog() == DialogResult.OK)
            _ = LoadAsync();
    }

    private async void OnBanClick(object? sender, EventArgs e)
    {
        if (_grid.CurrentRow is null) return;

        var idStr = _grid.CurrentRow.Cells[0].Value?.ToString();
        if (!Guid.TryParse(idStr, out var operatorId)) return;

        var name = _grid.CurrentRow.Cells[2].Value?.ToString() ?? "";
        var confirm = MessageBox.Show(
            $"Заблокировать оператора «{name}»?",
            "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        var form = new OperatorUpdateForm { Banned = true };
        await ErrorHelper.TryAsync(
            () => ServiceLocator.OperatorService.UpdateOperatorAsync(operatorId, form),
            "Оператор заблокирован.");
        await LoadAsync();
    }
}

/// <summary>
/// Диалог добавления нового оператора
/// </summary>
internal class AddOperatorDialog : Form
{
    private readonly RoundedTextBox _usernameField;
    private readonly RoundedTextBox _passwordField;
    private readonly RoundedTextBox _nameField;
    private readonly RoundedTextBox _emailField;
    private readonly ComboBox _roleCombo;

    public AddOperatorDialog()
    {
        Text = "Новый оператор";
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
        _roleCombo.Items.AddRange(["Обычный оператор", "Главный оператор"]);
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
        var form = new OperatorCreateForm
        {
            Username = _usernameField.Text,
            Password = _passwordField.Text,
            Name = _nameField.Text,
            Email = string.IsNullOrWhiteSpace(_emailField.Text) ? null : _emailField.Text,
            Role = _roleCombo.SelectedIndex == 1 ? OperatorRole.MainOperator : OperatorRole.OrdinaryOperator,
        };

        var ok = await ErrorHelper.TryAsync(
            () => ServiceLocator.OperatorService.CreateOperatorAsync(form),
            "Оператор создан.");

        if (ok)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
