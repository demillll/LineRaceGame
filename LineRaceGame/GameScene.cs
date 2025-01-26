using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using Newtonsoft.Json;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Net.Sockets;
using System.Net;

namespace LineRaceGame
{
	public class GameScene : IDisposable
	{
		public static float WorldScale;
		public static float CarScale;
		public static float FonScale;

		private static float unitsPerHeight = 80.0f;
		public Window mainWindow;

		public static Car Car1;
		public static Car Car2;

		public static Direct2D dx2d;
		public static Background background;
		public static List<GameObject> gameObjects = new List<GameObject>();

		public static RawRectangleF clientRect;
		public static InputDirectX inputDirectX;

		// Сетевые объекты
		private TcpClient client;
		private TcpListener server;
		private NetworkStream stream;
		private bool isHost;

		private DispatcherTimer renderTimer;

		public GameScene(bool isHost)
		{
			this.isHost = isHost;
			mainWindow = new Window
			{
				Title = "Line Race",
				Width = 1950,
				Height = 1080,
				WindowStartupLocation = WindowStartupLocation.CenterScreen
			};

			// Получаем дескриптор окна WPF
			IntPtr hwnd = new WindowInteropHelper(mainWindow).EnsureHandle();

			WorldScale = (float)mainWindow.Height / unitsPerHeight;
			CarScale = (float)mainWindow.Height / 40f;
			FonScale = (float)mainWindow.Height / 23f;

			dx2d = new Direct2D(hwnd, (int)mainWindow.Width, (int)mainWindow.Height); // Указаны размеры окна
			inputDirectX = new InputDirectX(hwnd);

			AddImages.AddAnimations();
			background = new Background(new Sprite("Background"), new Vector2(0, 0), WorldScale);
			AddImages.CreateObjects();

			Car1 = AddImages.CreateCars()[0];
			Car2 = AddImages.CreateCars()[1];

			InitializeNetwork();

			// Настроим таймер для рендеринга
			renderTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromMilliseconds(16) // 60 FPS
			};
			renderTimer.Tick += (s, e) => RenderCallback();
		}

		private void InitializeNetwork()
		{
			if (isHost)
			{
				server = new TcpListener(IPAddress.Any, 12345);
				server.Start();
				Task.Run(() =>
				{
					client = server.AcceptTcpClient();
					stream = client.GetStream();
				});
			}
			else
			{
				client = new TcpClient("127.0.0.1", 12345);
				stream = client.GetStream();
			}
		}

		private void RenderCallback()
		{
			TimeHelper.Update();
			inputDirectX.UpdateKeyboardState();

			WindowRenderTarget target = dx2d.RenderTarget;
			Size2F targetSize = target.Size;
			clientRect = new RawRectangleF(0, 0, targetSize.Width, targetSize.Height); // Преобразовано в RawRectangleF

			// Синхронизация состояния
			if (isHost)
			{
				SendGameState();
			}
			else
			{
				ReceiveGameState();
			}

			// Отрисовка
			target.BeginDraw();
			target.Clear(new RawColor4(0.941f, 0.973f, 1.0f, 1.0f)); // RGB для AliceBlue
			background.Draw(1.0f, WorldScale, unitsPerHeight / 980.0f, clientRect.Bottom, dx2d);

			foreach (var gameObject in gameObjects)
			{
				gameObject.Draw(1.0f, clientRect.Bottom, dx2d);
			}

			// Установка единичной матрицы трансформации
			target.Transform = new RawMatrix3x2
			{
				M11 = 1,
				M12 = 0,
				M21 = 0,
				M22 = 1,
				M31 = 0,
				M32 = 0
			};

			TextRender textRender = new TextRender(dx2d.RenderTarget, 20, SharpDX.DirectWrite.ParagraphAlignment.Near, SharpDX.DirectWrite.TextAlignment.Leading, System.Drawing.Color.Red);
			target.DrawText(
				$"\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n Fuel: {(int)Car1.Fuel}%" +
				$"\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t     Fuel: {(int)Car2.Fuel}%",
				textRender.textFormat, clientRect, textRender.Brush);

			target.EndDraw();
		}

		private void SendGameState()
		{
			var gameStates = CollectGameStates();
			var json = JsonConvert.SerializeObject(gameStates);
			var data = Encoding.UTF8.GetBytes(json);
			stream.Write(data, 0, data.Length);
		}

		private void ReceiveGameState()
		{
			byte[] buffer = new byte[1024];
			int bytesRead = stream.Read(buffer, 0, buffer.Length);
			if (bytesRead > 0)
			{
				var json = Encoding.UTF8.GetString(buffer, 0, bytesRead);
				var gameStates = JsonConvert.DeserializeObject<List<GameObjectState>>(json);

				for (int i = 0; i < gameObjects.Count; i++)
				{
					var state = gameStates[i];
					var obj = gameObjects[i];
					obj.Position.center = state.Position;
					obj.Position.scale = state.Scale;
					obj.Sprite.SetAnimation(state.AnimationTitle);
					obj.Sprite.animation.currentSprite = state.CurrentSprite;
				}
			}
		}

		public List<GameObjectState> CollectGameStates()
		{
			return gameObjects.Select(obj => new GameObjectState
			{
				Position = obj.Position.center,
				Scale = obj.Position.scale,
				AnimationTitle = obj.Sprite.animation.title,
				CurrentSprite = obj.Sprite.animation.currentSprite
			}).ToList();
		}

		public void Run()
		{
			mainWindow.Show();
			renderTimer.Start();
			System.Windows.Application app = new System.Windows.Application();
			app.Run(mainWindow);
		}

		public void Dispose()
		{
			dx2d.Dispose();
			stream?.Dispose();
			server?.Stop();
			client?.Close();
		}
	}
}
