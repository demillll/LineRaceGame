using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace LineRaceGame
{
	public abstract class Bonus : GameObject
	{
		/// <summary>
		/// констурктор бонуса
		/// </summary>
		/// <param name="sprite">параметр класса Sprite</param>
		/// <param name="startPos">стартовая позиция объекта</param>
		/// <param name="scale">масштаб объекта</param>
		public Bonus(Sprite sprite, Vector2 startPos, float scale) : base(sprite, startPos, scale, true) { }

		public abstract void BonusEction(GameObject @object);
	}
}
