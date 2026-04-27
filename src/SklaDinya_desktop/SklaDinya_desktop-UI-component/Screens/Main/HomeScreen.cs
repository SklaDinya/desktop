using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_UI_component.Controls;
using SklaDinya_desktop_UI_component.Helpers;

namespace SklaDinya_desktop_UI_component.Screens.Main;

public class HomeScreen : UserControl
{
    private readonly FlowLayoutPanel _resultsPanel;
    private readonly Label _statusLabel;

    public event EventHandler<StorageModel>? BookingRequested;

    public HomeScreen()
    {
        Dock = DockStyle.Fill;
        BackColor = AppTheme.Background;

        var heroPanel = new Panel { Dock = DockStyle.Top, Height = 140, BackColor = AppTheme.Primary };
        heroPanel.Controls.Add(new Label
        {
            Text = "Добро пожаловать в SklaDinya!", Font = AppTheme.FontTitle,
            ForeColor = AppTheme.TextLight, AutoSize = true, Location = new Point(40, 30),
        });
        heroPanel.Controls.Add(new Label
        {
            Text = "Сервис бронирования ячеек хранения. Найдите удобный пункт и забронируйте ячейку.",
            Font = AppTheme.FontRegular, ForeColor = Color.FromArgb(200, 255, 255, 255),
            Location = new Point(40, 70), AutoSize = false, Size = new Size(700, 50),
        });

        _statusLabel = new Label
        {
            Text = "Загрузка...", Font = AppTheme.FontRegular, ForeColor = AppTheme.TextMuted,
            Dock = DockStyle.Top, Height = 36, Padding = new Padding(40, 10, 0, 0),
        };

        // FlowLayoutPanel с центрированием
        _resultsPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            BackColor = AppTheme.Background,
            Padding = new Padding(0, 8, 0, 20),
        };
        // Центрируем элементы при изменении размера
        _resultsPanel.Resize += (_, _) => CenterCards();

        Controls.Add(_resultsPanel);
        Controls.Add(_statusLabel);
        Controls.Add(heroPanel);
    }

    private void CenterCards()
    {
        foreach (Control c in _resultsPanel.Controls)
        {
            if (c is StorageCard card)
            {
                var left = Math.Max(0, (_resultsPanel.ClientSize.Width - card.Width) / 2);
                card.Margin = new Padding(left, 0, 0, 12);
            }
        }
    }

    public void ShowResults(List<StorageModel> storages)
    {
        _resultsPanel.Controls.Clear();
        if (storages.Count == 0) { _statusLabel.Text = "Ничего не найдено."; return; }

        _statusLabel.Text = $"Найдено пунктов: {storages.Count}";

        foreach (var storage in storages)
        {
            var card = new StorageCard(storage);
            card.BookClicked += (_, _) => BookingRequested?.Invoke(this, card.Storage);
            _resultsPanel.Controls.Add(card);
        }
        CenterCards();
    }

    public async Task LoadDefaultStoragesAsync()
    {
        _statusLabel.Text = "Загрузка пунктов хранения...";
        try
        {
            var storages = await ServiceLocator.StorageService.GetStoragesAsync(new StorageSearchQuery { PageNumber = 0, PageSize = 20 });
            ShowResults(storages);
        }
        catch (HttpRequestException) { _statusLabel.Text = "Не удалось подключиться к серверу."; }
        catch { _statusLabel.Text = "Не удалось загрузить пункты хранения."; }
    }
}
