using System;
using System.Windows;

namespace LineRaceGame
{
	public partial class RoleSelectionWindow : Window
	{
		private GameServer _server;
		private GameClient _client;

		public RoleSelectionWindow()
		{
			InitializeComponent();
		}

		private void StartServerButton_Click(object sender, RoutedEventArgs e)
		{
			_server = new GameServer();
			_server.ClientMessageReceived += OnClientMessageReceived;
			_server.StartServer(12345);

			// Отображаем IP-адрес сервера
			ServerIpTextBlock.Text = $"IP сервера: {_server.GetLocalIpAddress()}";
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
			string serverIp = ClientIpTextBox.Text.Trim();

			if (string.IsNullOrEmpty(serverIp))
			{
				MessageBox.Show("Введите IP-адрес сервера.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
