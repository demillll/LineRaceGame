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
	public class GameObject
	{
		/// <summary>
		/// Поле класса Sprite
		/// </summary>
		protected internal Sprite sprite;
		/// <summary>
		/// Поле класса Position
		/// </summary>
		protected internal Position position;

		/// <summary>
		/// Свойство для получения позиции
		/// </summary>
		public Position Position => position;

		/// <summary>
		/// Свойство для получения спрайта
		/// </summary>
		public Sprite Sprite => sprite;

		/// <summary>
		/// Список объектов класса MoveObject
		/// </summary>
		protected internal List<MoveObject> moveObject = new List<MoveObject>();

		/// <summary>
		/// Переменная для маштаба
		/// </summary>
		public float scale;

		/// <summary>
		/// Поле класса Collider
		/// </summary>
		protected internal Collider collider;

		/// <summary>
		/// Список объектов класса GameObject
		/// </summary>
		public static List<GameObject> gameObjects = new List<GameObject>();

		/// <summary>
		/// Переменная стороны игрока
		/// </summary>
		public bool Site;

		/// <summary>
		/// Конструктор класса
		/// </summary>
		/// <param name="sprite">Параметр класса Sprite</param>
		/// <param name="startPos">Стартовая позиция</param>
		/// <param name="scale">Маштаб</param>
		/// <param name="site">Сторона игрока</param>
		public GameObject(Sprite sprite, Vector2 startPos, float scale, bool site)
		{
			this.sprite = sprite;
			this.position = new Position(startPos.X, startPos.Y, 0.0f, scale);
			this.scale = scale;
			gameObjects.Add(this);
			if (collider == null)
			{
				collider = new Collider(this, new Vector2(1, 1));
			}
			Site = site;
		}

		/// <summary>
		/// Метод отрисовки
		/// </summary>
		/// <param name="opacity">непрозрачность</param>
		/// <param name="height">ширина</param>
		/// <param name="dx2d">параметр класса dx2d</param>
		public virtual void Draw(float opacity, float height, Direct2D dx2d)
		{
			RawMatrix3x2 matrix;

			if (moveObject != null)
			{
				foreach (var script in moveObject)
				{
					script.Update(gameObjects);
				}
			}

			Bitmap bitmap = sprite.animation.GetCurrentSprite(this.sprite);
			Vector2 translation = new Vector2();
			translation.X = sprite.PositionOfCenter.X / bitmap.Size.Width + position.center.X * position.scale;
			translation.Y = sprite.PositionOfCenter.Y / bitmap.Size.Height + position.center.Y * position.scale;

			// Создаем матрицу для поворота
			matrix = new RawMatrix3x2(
				(float)Math.Cos(position.angle), (float)Math.Sin(position.angle),
				(float)-Math.Sin(position.angle), (float)Math.Cos(position.angle),
				translation.X, translation.Y);

			// Масштабирование
			RawMatrix3x2 scaleMatrix = new RawMatrix3x2(
				position.scale * scale / bitmap.Size.Width, 0,
				0, position.scale * scale / bitmap.Size.Height,
				translation.X, translation.Y);

			// Трансляция
			RawMatrix3x2 translationMatrix = new RawMatrix3x2(
				1, 0,
				0, 1,
				translation.X * scale, translation.Y * scale);

			// Умножаем матрицы вручную
			matrix = MultiplyRawMatrix3x2(matrix, scaleMatrix);
			matrix = MultiplyRawMatrix3x2(matrix, translationMatrix);

			WindowRenderTarget r = dx2d.RenderTarget;
			r.Transform = matrix;
			r.DrawBitmap(bitmap, opacity, BitmapInterpolationMode.NearestNeighbor);
		}

		// Функция для умножения двух RawMatrix3x2
		private RawMatrix3x2 MultiplyRawMatrix3x2(RawMatrix3x2 m1, RawMatrix3x2 m2)
		{
			return new RawMatrix3x2(
				m1.M11 * m2.M11 + m1.M21 * m2.M12,
				m1.M11 * m2.M21 + m1.M21 * m2.M22,
				m1.M31 * m2.M11 + m1.M32 * m2.M12 + m1.M31,  // Трансляция X
				m1.M11 * m2.M31 + m1.M21 * m2.M32 + m1.M32,  // Трансляция Y
				m1.M31,
				m1.M32
			);
		}


		/// <summary>
		/// Обновление списка скриптов
		/// </summary>
		/// <param name="moveAdd">параметр массива</param>
		public virtual void AddMove(params MoveObject[] moveAdd)
		{
			for (int i = 0; i < moveAdd.Length; i++)
			{
				moveAdd[i].SignToObject(this);
				moveObject.Add(moveAdd[i]);
			}
		}

		/// <summary>
		/// Получение состояния объекта для передачи по сети
		/// </summary>
		public virtual GameObjectState GetState()
		{
			return new GameObjectState
			{
				Position = position.center,
				Scale = position.scale,
				AnimationTitle = sprite.animation.title,
				CurrentSprite = sprite.animation.currentSprite
			};
		}

		/// <summary>
		/// Обновление состояния объекта на основе данных от сети
		/// </summary>
		/// <param name="state">Состояние объекта</param>
		public virtual void UpdateState(GameObjectState state)
		{
			position.center = state.Position;
			position.scale = state.Scale;
			sprite.SetAnimation(state.AnimationTitle);
			sprite.animation.currentSprite = state.CurrentSprite;
		}

		/// <summary>
		/// Конструктор базового класса 
		/// </summary>
		public GameObject() { }
	}
}
