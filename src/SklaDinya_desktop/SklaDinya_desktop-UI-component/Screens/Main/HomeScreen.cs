using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;
using SklaDinya_desktop_UI_component.Controls;
using SklaDinya_desktop_UI_component.Helpers;

namespace SklaDinya_desktop_UI_component.Screens.Main;

/// <summary>
/// Главная страница — информация и результаты поиска пунктов хранения
/// </summary>
public class HomeScreen : UserControl
{
    private readonly FlowLayoutPanel _resultsPanel;
    private readonly Label _heroTitle;
    private readonly Label _heroSubtitle;
    private readonly Label _statusLabel;

    /// <summary>Вызывается при нажатии «Забронировать» на карточке</summary>
    public event EventHandler<StorageModel>? BookingRequested;

    public HomeScreen()
    {
        Dock = DockStyle.Fill;
        BackColor = AppTheme.Background;
        AutoScroll = true;

        // ── Блок-приветствие ────────────────────────────────────────────
        var heroPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 160,
            BackColor = AppTheme.Primary,
            Padding = new Padding(40, 30, 40, 20),
        };

        _heroTitle = new Label
        {
            Text = "Добро пожаловать в SklaDinya!",
            Font = AppTheme.FontTitle,
            ForeColor = AppTheme.TextLight,
            AutoSize = true,
            Location = new Point(40, 36),
        };

        _heroSubtitle = new Label
        {
            Text = "Сервис бронирования ячеек хранения. Найдите удобный пункт и забронируйте ячейку прямо сейчас.",
            Font = AppTheme.FontRegular,
            ForeColor = Color.FromArgb(200, 255, 255, 255),
            AutoSize = false,
            Size = new Size(600, 50),
            Location = new Point(40, 80),
        };

        heroPanel.Controls.AddRange([_heroTitle, _heroSubtitle]);

        // ── Результаты поиска ───────────────────────────────────────────
        _statusLabel = new Label
        {
            Text = "Воспользуйтесь поиском вверху, чтобы найти пункт хранения.",
            Font = AppTheme.FontRegular,
            ForeColor = AppTheme.TextMuted,
            AutoSize = false,
            Height = 40,
            Dock = DockStyle.Top,
            Padding = new Padding(40, 12, 0, 0),
        };

        _resultsPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            Padding = new Padding(40, 8, 40, 20),
        };

        Controls.Add(_resultsPanel);
        Controls.Add(_statusLabel);
        Controls.Add(heroPanel);
    }

    /// <summary>
    /// Отобразить результаты поиска
    /// </summary>
    public void ShowResults(List<StorageModel> storages)
    {
        _resultsPanel.Controls.Clear();

        if (storages.Count == 0)
        {
            _statusLabel.Text = "Ничего не найдено. Попробуйте другой запрос.";
            return;
        }

        _statusLabel.Text = $"Найдено пунктов: {storages.Count}";

        foreach (var storage in storages)
        {
            var card = new StorageCard(storage)
            {
                Width = _resultsPanel.Width - 100,
            };
            card.BookClicked += (_, _) => BookingRequested?.Invoke(this, storage);
            _resultsPanel.Controls.Add(card);
        }
    }

    /// <summary>
    /// Загрузить все активные пункты при открытии
    /// </summary>
    public async Task LoadDefaultStoragesAsync()
    {
        _statusLabel.Text = "Загрузка пунктов хранения...";
        var query = new StorageSearchQuery { PageNumber = 0, PageSize = 20 };
        try
        {
            var storages = await ServiceLocator.StorageService.GetStoragesAsync(query);
            ShowResults(storages);
        }
        catch (HttpRequestException)
        {
            _statusLabel.Text = "Не удалось подключиться к серверу. Проверьте подключение и адрес API.";
        }
        catch (Exception)
        {
            _statusLabel.Text = "Не удалось загрузить пункты хранения. Сервер недоступен.";
        }
    }
}
