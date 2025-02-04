using System;
using System.Collections.Generic;
using System.IO;
using SharpDX.WIC;
using SharpDX;
using SharpDX.Direct2D1;

namespace LineRaceGame
{
	public class Direct2D : IDisposable
	{
		private SharpDX.Direct2D1.Factory factory;
		public WindowRenderTarget RenderTarget { get; private set; }
		private ImagingFactory imagingFactory;

		// Конструктор для инициализации Direct2D и WIC
		public Direct2D(IntPtr hwnd, int width, int height)
		{
			factory = new SharpDX.Direct2D1.Factory();

			// Свойства рендера
			RenderTargetProperties renderProp = new RenderTargetProperties
			{
				DpiX = 0,
				DpiY = 0,
				MinLevel = FeatureLevel.Level_DEFAULT,
				PixelFormat = new SharpDX.Direct2D1.PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied),
				Type = RenderTargetType.Default,
				Usage = RenderTargetUsage.None
			};

			// Свойства окна
			HwndRenderTargetProperties winProp = new HwndRenderTargetProperties
			{
				Hwnd = hwnd,
				PixelSize = new Size2(width, height),
				PresentOptions = PresentOptions.None
			};

			// Создание цели рендеринга
			RenderTarget = new WindowRenderTarget(factory, renderProp, winProp)
			{
				AntialiasMode = AntialiasMode.PerPrimitive,
				TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Cleartype
			};

			Console.WriteLine(RenderTarget == null ? "Ошибка: RenderTarget не создан." : "RenderTarget успешно создан.");

			// Инициализация WIC (Windows Imaging Component)
			imagingFactory = new ImagingFactory();
			
		}

		// Метод для загрузки битмапов из файлов
		public List<SharpDX.Direct2D1.Bitmap> LoadBitmap(params string[] paths)
		{
			var bitmaps = new List<SharpDX.Direct2D1.Bitmap>();

			foreach (var path in paths)
			{
				// Проверка пути и существования файла
				if (!File.Exists(path))
				{
					Console.WriteLine($"Ошибка: файл не найден по пути {path}");
					continue;
				}

				try
				{
					Console.WriteLine($"Загрузка изображения: {path}");

					// Создание декодера для изображения
					using (var decoder = new BitmapDecoder(imagingFactory, path, DecodeOptions.CacheOnDemand))
					using (var frame = decoder.GetFrame(0))
					using (var converter = new FormatConverter(imagingFactory))
					{
						// Конвертация изображения в формат 32bpp PRGBA
						converter.Initialize(frame, SharpDX.WIC.PixelFormat.Format32bppPRGBA, BitmapDitherType.None, null, 0.0, BitmapPaletteType.Custom);

						// Создание Bitmap для Direct2D
						var bitmap = SharpDX.Direct2D1.Bitmap.FromWicBitmap(RenderTarget, converter);
						bitmaps.Add(bitmap);

						Console.WriteLine($"Изображение успешно загружено: {path}");
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Ошибка при загрузке изображения {path}: {ex.Message}");
				}
			}

			return bitmaps;
		}

		// Метод освобождения ресурсов
		public void Dispose()
		{
			try
			{
				// Освобождение целей рендеринга и фабрик
				RenderTarget?.Dispose();
				RenderTarget = null;

				imagingFactory?.Dispose();
				imagingFactory = null;

				factory?.Dispose();
				factory = null;

				Console.WriteLine("Direct2D успешно освобождён.");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка при освобождении ресурсов Direct2D: {ex.Message}");
			}
		}
	}
}
