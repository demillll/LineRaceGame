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

			// Итоговая матрица преобразований (вручную объединяем вращение, масштабирование и перенос)
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

			// Получаем текущий спрайт и рисуем его
			Bitmap bitmap = sprite.animation.GetCurrentSprite(this.sprite);
			r.DrawBitmap(bitmap, opacity, BitmapInterpolationMode.Linear);
		}
	}
}
