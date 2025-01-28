using System;
using System.Numerics; // Для Vector2
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace LineRaceGame
{
	public class Background
	{
		protected internal Sprite sprite;
		protected internal Position position;
		public float scale;
		protected internal RawMatrix3x2 matrix;

		public Background(Sprite sprite, Vector2 startPos, float scale)
		{
			this.sprite = sprite;
			this.position = new Position(startPos.X, startPos.Y, 0.0f, scale);
			this.scale = scale;
		}

		public void Draw(float opacity, float scale, float textureScale, float height, Direct2D dx2d)
		{
			// Проверки масштаба
			if (scale <= 0 || textureScale <= 0)
			{
				Console.WriteLine("Ошибка: Некорректный масштаб.");
				return;
			}

			if (GameScene.WorldScale <= 0)
			{
				Console.WriteLine("Ошибка: Некорректное значение WorldScale.");
				return;
			}

			// Перемещение для преобразования
			Vector2 translation = new Vector2
			{
				X = sprite.PositionOfCenter.X / scale - (1000f / GameScene.WorldScale),
				Y = -sprite.PositionOfCenter.Y / scale - (500f / GameScene.WorldScale)
			};

			// Создаем матрицу вручную
			float rotationAngle = -position.angle;
			float cosAngle = (float)Math.Cos(rotationAngle);
			float sinAngle = (float)Math.Sin(rotationAngle);

			// Матрица масштабирования
			float scaleX = scale * textureScale;
			float scaleY = scale * textureScale;

			// Итоговая матрица преобразований
			matrix = new RawMatrix3x2
			{
				M11 = cosAngle * scaleX,
				M12 = sinAngle * scaleY,
				M21 = -sinAngle * scaleX,
				M22 = cosAngle * scaleY,
				M31 = translation.X,
				M32 = translation.Y
			};

			WindowRenderTarget r = dx2d.RenderTarget;
			r.Transform = matrix;

			// Проверка на null перед использованием анимации
			if (sprite?.animation == null || sprite.animation.sprites.Count == 0)
			{
				Console.WriteLine("Ошибка: Анимация не инициализирована или не содержит кадров.");
				return;
			}

			try
			{
				// Получаем текущий спрайт и рисуем его
				SharpDX.Direct2D1.Bitmap bitmap = sprite.animation.GetCurrentSprite(this.sprite);

				if (bitmap == null)
				{
					Console.WriteLine("Ошибка: Bitmap не загружен.");
					return;
				}

				if (opacity < 0.0f || opacity > 1.0f)
				{
					Console.WriteLine("Ошибка: Прозрачность вне допустимого диапазона (0.0 - 1.0).");
					return;
				}

				r.DrawBitmap(bitmap, opacity, BitmapInterpolationMode.Linear);
			}
			catch (AccessViolationException ex)
			{
				Console.WriteLine($"AccessViolationException: {ex.Message}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка при рисовании спрайта: {ex.Message}");
			}
		}



	}
}
