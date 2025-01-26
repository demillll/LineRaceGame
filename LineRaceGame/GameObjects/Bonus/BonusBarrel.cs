using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace LineRaceGame
{
	class BonusBarrel : Bonus
	{
		/// <summary>
		/// Констурктор бонуса топлива
		/// </summary>

		/// /// <param name="sprite">параметр класса Sprite</param>
		/// <param name="position">позиция объекта</param>
		/// <param name="scale">масштаб объекта</param>

		public BonusBarrel(Sprite sprite, Vector2 position, float scale) : base(sprite, position, scale)
		{
			collider = new Collider(this, new Vector2(0.4f, 0.4f));
		}

		public override void BonusEction(GameObject @object)
		{
			@object = new BarrelDecorates((Car)@object);
		}
	}
}
