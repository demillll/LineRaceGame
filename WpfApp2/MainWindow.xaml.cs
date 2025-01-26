using System;
using System.Threading.Tasks;
using System.Windows;
using LineRaceGame;

namespace WpfApp2
{
	public partial class MainWindow : Window
	{
		private bool _isServerReady = false;
		private bool _isClientReady = false;
		private bool _isClientConnected = false;

		public MainWindow()
		{
			InitializeComponent();
		}

		// Обработчик для кнопки "Играть на одном компьютере"
		private void PlayOnOneComputerButton_Click(object sender, RoutedEventArgs e)
		{
			StartGame(isHost: true); // Для игры на одном компьютере можно указать любое значение
		}


		// Обработчик для кнопки "Выход"
		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			System.Windows.Application.Current.Shutdown();
		}

		// Обработчик для кнопки "Играть по локальной сети"
		private void PlayOnLANButton_Click(object sender, RoutedEventArgs e)
		{
			// Открытие нового окна для выбора роли
			RoleSelectionWindow roleWindow = new RoleSelectionWindow();
			roleWindow.Show();
		}

		// Получение локального IP-адреса для сервера
		private string GetLocalIpAddress()
		{
			var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
			foreach (var ip in host.AddressList)
			{
				if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
				{
					return ip.ToString();
				}
			}
			return "Не удалось найти IP";
		}

		// Запуск игры
		private void StartGame(bool isHost)
		{
			GameScene gameScene = new GameScene(isHost);
			gameScene.Run();
		}

	}
}
