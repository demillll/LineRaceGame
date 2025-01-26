using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace LineRaceGame
{
	public class Car : GameObject
	{
		/// <summary>
		/// Переменная для колизии
		/// </summary>
		public bool IsCrash;

		/// <summary>
		/// Переменная для игрока
		/// </summary>
		public bool IsPlayer;

		/// <summary>
		/// переменная топливо
		/// </summary>
		public float fuel = 100;

		/// <summary>
		/// переменная объема топливного бака
		/// </summary>
		public float maxFuel = 100;

		/// <summary>
		/// Вызов базового класса
		/// </summary>
		public Car() : base() { }

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="sprite">параметр класса Sprite</param>
		/// <param name="startPos">стартовая позиция объекта</param>
		/// <param name="scale">масштаб объекта</param>
		/// <param name="site">Сторона игрока</param>
		/// <param name="IsPlayer">Игрок</param>
		public Car(Sprite sprite, Vector2 startPos, float scale, bool site, bool IsPlayer = false) : base(sprite, startPos, scale, site)
		{
			collider = new Collider(this, new Vector2(1f, 1f));
			this.IsPlayer = IsPlayer;
		}

		/// <summary>
		/// Рисование объекта
		/// </summary>
		public override void Draw(float opacity, float height, Direct2D dx2d)
		{
			// Если есть связанные объекты, обновляем их состояние
			if (moveObject != null)
			{
				foreach (var script in moveObject)
				{
					script.Update(gameObjects);
				}
			}

			Bitmap bitmap = sprite.animation.GetCurrentSprite(this.sprite);

			// Центр трансляции
			var translation = new Vector2(
				sprite.PositionOfCenter.X / bitmap.Size.Width + position.center.X * position.scale,
				sprite.PositionOfCenter.Y / bitmap.Size.Height + position.center.Y * position.scale
			);

			// Создаем матрицы преобразований вручную
			var rotationMatrix = new RawMatrix3x2(
				(float)Math.Cos(-position.angle), (float)Math.Sin(-position.angle),
				(float)-Math.Sin(-position.angle), (float)Math.Cos(-position.angle),
				0, 0
			);

			var scalingMatrix = new RawMatrix3x2(
				position.scale * scale / bitmap.Size.Width, 0,
				0, position.scale * scale / bitmap.Size.Height,
				0, 0
			);

			var translationMatrix = new RawMatrix3x2(
				1, 0,
				0, 1,
				translation.X * scale, translation.Y * scale
			);

			// Итоговая трансформация (умножение матриц)
			var finalMatrix = MultiplyMatrix3x2(MultiplyMatrix3x2(rotationMatrix, scalingMatrix), translationMatrix);

			WindowRenderTarget renderTarget = dx2d.RenderTarget;
			renderTarget.Transform = finalMatrix;

			// Рисуем объект
			renderTarget.DrawBitmap(bitmap, opacity, BitmapInterpolationMode.NearestNeighbor);
		}

		/// <summary>
		/// Метод для управления топливом
		/// </summary>
		public float Fuel
		{
			get { return fuel; }
			set
			{
				fuel = value;
				if (fuel > maxFuel) fuel = maxFuel;
				if (fuel < 0) fuel = 0;
			}
		}

		/// <summary>
		/// Вспомогательный метод для умножения матриц 3x2
		/// </summary>
		private RawMatrix3x2 MultiplyMatrix3x2(RawMatrix3x2 m1, RawMatrix3x2 m2)
		{
			return new RawMatrix3x2
			(
				m1.M11 * m2.M11 + m1.M12 * m2.M21,
				m1.M11 * m2.M12 + m1.M12 * m2.M22,
				m1.M21 * m2.M11 + m1.M22 * m2.M21,
				m1.M21 * m2.M12 + m1.M22 * m2.M22,
				m1.M31 * m2.M11 + m1.M32 * m2.M21 + m2.M31,
				m1.M31 * m2.M12 + m1.M32 * m2.M22 + m2.M32
			);
		}
	}
}
