using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LineRaceGame
{
	public class GameClient
	{
		private TcpClient _client;
		private NetworkStream _stream;

		public event Action<string> ServerMessageReceived;
		public event Action ConnectionEstablished;

		public void ConnectToServer(string serverIp, int port)
		{
			_client = new TcpClient();

			Task.Run(async () =>
			{
				await _client.ConnectAsync(serverIp, port);
				_stream = _client.GetStream();
				ConnectionEstablished?.Invoke();

				// Начало чтения данных от сервера
				await ReceiveDataAsync();
			});
		}

		public void SendMessage(string message)
		{
			if (_stream != null && _client.Connected)
			{
				byte[] data = Encoding.UTF8.GetBytes(message);
				_stream.Write(data, 0, data.Length);
			}
		}

		private async Task ReceiveDataAsync()
		{
			byte[] buffer = new byte[1024];
			while (_client.Connected)
			{
				int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
				if (bytesRead > 0)
				{
					string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
					ServerMessageReceived?.Invoke(message);
				}
			}
		}

		public void Disconnect()
		{
			_stream?.Close();
			_client?.Close();
		}
	}
}
