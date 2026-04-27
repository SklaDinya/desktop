using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_UI_component.Controls;
using SklaDinya_desktop_UI_component.Helpers;

namespace SklaDinya_desktop_UI_component.Screens.Account;

/// <summary>
/// Вкладка «Создать пункт хранения» — подача заявки
/// </summary>
public class CreateStorageTab : UserControl
{
    private readonly RoundedTextBox _storageNameField;
    private readonly RoundedTextBox _addressField;
    private readonly RoundedTextBox _descriptionField;
    private readonly RoundedTextBox _operatorUsernameField;
    private readonly RoundedTextBox _operatorPasswordField;
    private readonly RoundedTextBox _operatorNameField;
    private readonly RoundedTextBox _operatorEmailField;
    private readonly RoundedButton _submitButton;

    public CreateStorageTab()
    {
        Dock = DockStyle.Fill;
        BackColor = AppTheme.Background;
        AutoScroll = true;

        var title = new Label
        {
            Text = "Создание пункта хранения",
            Font = AppTheme.FontTitle,
            ForeColor = AppTheme.Primary,
            AutoSize = true,
            Location = new Point(32, 24),
        };
        Controls.Add(title);

        int y = 70;
        const int w = 400;

        var storageSectionLabel = new Label
        {
            Text = "Данные пункта хранения",
            Font = AppTheme.FontHeader,
            ForeColor = AppTheme.Secondary,
            AutoSize = true,
            Location = new Point(32, y),
        };
        Controls.Add(storageSectionLabel);
        y += 32;

        _storageNameField = MakeField("Название пункта", ref y, w);
        _addressField = MakeField("Адрес", ref y, w);
        _descriptionField = MakeField("Описание (необязательно)", ref y, w);

        y += 8;
        var opSectionLabel = new Label
        {
            Text = "Данные главного оператора",
            Font = AppTheme.FontHeader,
            ForeColor = AppTheme.Secondary,
            AutoSize = true,
            Location = new Point(32, y),
        };
        Controls.Add(opSectionLabel);
        y += 32;

        _operatorUsernameField = MakeField("Логин оператора", ref y, w);
        _operatorPasswordField = MakeField("Пароль оператора", ref y, w, isPassword: true);
        _operatorNameField = MakeField("Имя оператора", ref y, w);
        _operatorEmailField = MakeField("Почта оператора", ref y, w);

        y += 12;
        _submitButton = new RoundedButton
        {
            Text = "Подать заявку",
            BackColor = AppTheme.Primary,
            Width = w,
            Location = new Point(32, y),
        };
        _submitButton.Click += OnSubmitClick;
        Controls.Add(_submitButton);
    }

    private RoundedTextBox MakeField(string placeholder, ref int y, int width, bool isPassword = false)
    {
        var field = new RoundedTextBox
        {
            Placeholder = placeholder,
            Width = width,
            Location = new Point(32, y),
        };
        if (isPassword) field.UsePasswordChar = true;
        Controls.Add(field);
        y += 50;
        return field;
    }

    private async void OnSubmitClick(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_storageNameField.Text) ||
            string.IsNullOrWhiteSpace(_addressField.Text) ||
            string.IsNullOrWhiteSpace(_operatorUsernameField.Text) ||
            string.IsNullOrWhiteSpace(_operatorPasswordField.Text) ||
            string.IsNullOrWhiteSpace(_operatorNameField.Text) ||
            string.IsNullOrWhiteSpace(_operatorEmailField.Text))
        {
            MessageBox.Show("Заполните все обязательные поля.", "Внимание",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _submitButton.Enabled = false;

        var form = new StorageCreateForm
        {
            StorageName = _storageNameField.Text,
            Address = _addressField.Text,
            Description = string.IsNullOrWhiteSpace(_descriptionField.Text) ? null : _descriptionField.Text,
            Username = _operatorUsernameField.Text,
            Password = _operatorPasswordField.Text,
            Name = _operatorNameField.Text,
            Email = _operatorEmailField.Text,
        };

        var ok = await ErrorHelper.TryAsync(
            () => ServiceLocator.StorageService.CreateStorageAsync(form),
            "Заявка на создание пункта хранения отправлена!");

        _submitButton.Enabled = true;
    }
}
