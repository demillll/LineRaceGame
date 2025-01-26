﻿using System;
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

		public Direct2D(IntPtr hwnd, int width, int height)
		{
			// Создание фабрик для 2D объектов
			factory = new SharpDX.Direct2D1.Factory();

			// Настройка свойств рендеринга
			RenderTargetProperties renderProp = new RenderTargetProperties
			{
				DpiX = 0,
				DpiY = 0,
				MinLevel = FeatureLevel.Level_DEFAULT,
				PixelFormat = new SharpDX.Direct2D1.PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied),
				Type = RenderTargetType.Hardware,
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

			// Инициализация фабрики WIC
			imagingFactory = new ImagingFactory();
		}

		public List<SharpDX.Direct2D1.Bitmap> LoadBitmap(params string[] paths)
		{
			var bitmaps = new List<SharpDX.Direct2D1.Bitmap>();

			foreach (var path in paths)
			{
				try
				{
					// Логирование пути и проверки существования файла
					Console.WriteLine($"Загружается изображение: {path}");
					if (!File.Exists(path))
					{
						Console.WriteLine($"Ошибка: файл не найден по пути: {path}");
						continue; // Пропустить этот путь, если файл не найден
					}

					// Создание декодера для изображения
					BitmapDecoder decoder = new BitmapDecoder(imagingFactory, path, DecodeOptions.CacheOnDemand);
					BitmapFrameDecode frame = decoder.GetFrame(0);

					// Конвертация изображения в нужный формат
					FormatConverter converter = new FormatConverter(imagingFactory);
					converter.Initialize(frame, SharpDX.WIC.PixelFormat.Format32bppPRGBA, BitmapDitherType.None, null, 0.0, BitmapPaletteType.Custom);

					// Создание Bitmap на основе WIC
					var bitmap = SharpDX.Direct2D1.Bitmap.FromWicBitmap(RenderTarget, converter);

					// Очистка ресурсов
					Utilities.Dispose(ref converter);
					Utilities.Dispose(ref frame);
					Utilities.Dispose(ref decoder);

					bitmaps.Add(bitmap);
				}
				catch (Exception ex)
				{
					// Логирование ошибки в случае исключения
					Console.WriteLine($"Ошибка при загрузке изображения {path}: {ex.Message}");
				}
			}

			return bitmaps;
		}

		public void Dispose()
		{
			if (RenderTarget != null)
			{
				RenderTarget.Dispose();
				RenderTarget = null;
			}
			Utilities.Dispose(ref imagingFactory);
			Utilities.Dispose(ref factory);
		}
	}
}