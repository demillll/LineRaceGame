using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;

namespace LineRaceGame
{
	public class BackgroundLoseWin : GameObject
	{
		/// <summary>
		/// Переменная активации заставки
		/// </summary>
		public bool IsActiv;

		protected internal Matrix3x2 matrix;
		/// <summary>
		/// Конструктор заставки проигрыша
		/// </summary>
		/// <param name="sprite">параметр класса Sprite</param>
		/// <param name="startPos">стартовая позиция объекта</param>
		/// <param name="scale">масштаб объекта</param>
		/// <param name="site">Сторона игрока</param>
		public BackgroundLoseWin(Sprite sprite, Vector2 startPos, float scale, bool site) : base(sprite, startPos, scale, site)
		{
			this.sprite = sprite;
			this.position = new Position(startPos.X, startPos.Y, 0.0f, scale);
			this.scale = scale;
			IsActiv = false;
		}
		/// <summary>
		/// Метод отрисовки
		/// </summary>
		/// <param name="opacity">непрозрачность</param>
		/// <param name="height">ширина</param>
		/// <param name="dx2d">параметр класса dx2d</param>
		public override void Draw(float opacity, float height, Direct2D dx2d)
		{
			if (IsActiv)
			{
				base.Draw(opacity, height, dx2d);
			}
		}
	}
}
