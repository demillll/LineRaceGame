using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LineRaceGame
{
	public class GameServer
	{
		private TcpListener _listener;
		private TcpClient _client;
		private NetworkStream _stream;

		public event Action<string> ClientMessageReceived;
		public event Action ClientConnected;

		public void StartServer(int port)
		{
			_listener = new TcpListener(IPAddress.Any, port);
			_listener.Start();

			// Асинхронное ожидание подключения клиента
			Task.Run(async () =>
			{
				_client = await _listener.AcceptTcpClientAsync();
				_stream = _client.GetStream();
				ClientConnected?.Invoke();

				// Начало чтения данных от клиента
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
					ClientMessageReceived?.Invoke(message);
				}
			}
		}

		public void StopServer()
		{
			_stream?.Close();
			_client?.Close();
			_listener?.Stop();
		}

		public string GetLocalIpAddress()
		{
			var host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (var ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
				{
					return ip.ToString();
				}
			}
			return "Не удалось найти IP";
		}
	}
}
