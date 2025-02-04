using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Windows;
using LineRaceGame;

namespace WpfApp2
{
	public partial class RoleSelectionWindow : Window
	{
		private GameServer _server;
		private GameClient _client;

		public RoleSelectionWindow()
		{
			InitializeComponent();

			// Поле для ввода IP-адреса доступно с самого начала
			ClientIpInputContainer.Visibility = Visibility.Visible;
		}

		private void StartServerButton_Click(object sender, RoutedEventArgs e)
		{
			_server = new GameServer();
			_server.ClientMessageReceived += OnClientMessageReceived;
			_server.StartServer(12345);

			// Получение и отображение IPv4-адресов
			var wifiIp = GetLocalIpAddress("Wi-Fi");
			var ethernetIp = GetLocalIpAddress("Ethernet");

			string ipInfo = "IP сервера:";
			if (!string.IsNullOrEmpty(wifiIp))
				ipInfo += $"\nWi-Fi: {wifiIp}";
			if (!string.IsNullOrEmpty(ethernetIp))
				ipInfo += $"\nEthernet: {ethernetIp}";

			ServerIpTextBlock.Text = ipInfo;
			ServerIpTextBlock.Visibility = Visibility.Visible;

			// Показываем статус ожидания клиента
			ServerStatusTextBlock.Text = "Ожидание подключения клиента...";
			ServerStatusTextBlock.Visibility = Visibility.Visible;

			_server.ClientConnected += () =>
			{
				Dispatcher.Invoke(() =>
				{
					ServerStatusTextBlock.Text = "Клиент подключился! Игра запускается...";
					StartGame(isHost: true); // Сервер — это хост
					this.Close(); // Закрываем окно выбора роли
				});
			};
		}

		private void ConnectButton_Click(object sender, RoutedEventArgs e)
		{
			// Проверяем, был ли введен IP-адрес
			string serverIp = ClientIpTextBox.Text.Trim();

			if (string.IsNullOrEmpty(serverIp))
			{
				System.Windows.MessageBox.Show("Введите IP-адрес сервера.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			_client = new GameClient();
			_client.ServerMessageReceived += OnServerMessageReceived;

			_client.ConnectToServer(serverIp, 12345);

			ClientStatusTextBlock.Text = "Попытка подключения...";
			ClientStatusTextBlock.Visibility = Visibility.Visible;

			_client.ConnectionEstablished += () =>
			{
				Dispatcher.Invoke(() =>
				{
					ClientStatusTextBlock.Text = "Соединение с сервером установлено! Игра запускается...";
					StartGame(isHost: false); // Клиент не является хостом
					this.Close(); // Закрываем окно выбора роли
				});
			};
		}

		private string GetLocalIpAddress(string adapterName)
		{
			try
			{
				var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
					.Where(ni => ni.OperationalStatus == OperationalStatus.Up && ni.Name.Contains(adapterName));

				foreach (var adapter in networkInterfaces)
				{
					var ipProperties = adapter.GetIPProperties();
					var ipv4Address = ipProperties.UnicastAddresses
						.FirstOrDefault(ua => ua.Address.AddressFamily == AddressFamily.InterNetwork);

					if (ipv4Address != null)
					{
						return ipv4Address.Address.ToString();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка при получении IP-адреса для {adapterName}: {ex.Message}");
			}

			return null;
		}

		private void OnClientMessageReceived(string message)
		{
			Console.WriteLine($"Сообщение от клиента: {message}");
		}

		private void OnServerMessageReceived(string message)
		{
			Console.WriteLine($"Сообщение от сервера: {message}");
		}

		private void StartGame(bool isHost)
		{
			GameScene gameScene = new GameScene(isHost);
			gameScene.Run();
		}
	}
}
