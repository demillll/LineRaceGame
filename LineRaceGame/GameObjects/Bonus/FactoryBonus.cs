using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.DirectInput;


namespace LineRaceGame
{
	public static class FactoryBonus
	{
		public static Bonus CreateBonusLeft<T>() where T : Bonus
		{
			if (typeof(T) == typeof(BonusFuel))
			{
				BonusFuel bonusFuel = new BonusFuel(new Sprite("FuelBones"), new Vector2(0.2f, -5f), GameScene.CarScale);
				bonusFuel.AddMove(new MoveBonus(Key.W, Key.S, "FuelBones"));
				return bonusFuel;

			}
			else if (typeof(T) == typeof(BonusBarrel))
			{
				BonusBarrel bonusBarrel = new BonusBarrel(new Sprite("BarrelBonus"), new Vector2(0.2f, -10f), GameScene.CarScale);
				bonusBarrel.AddMove(new MoveBonus(Key.W, Key.S, "BarrelBonus"));
				return bonusBarrel;
			}
			return null;
		}

		public static Bonus CreateBonusRight<T>() where T : Bonus
		{
			if (typeof(T) == typeof(BonusFuel))
			{
				BonusFuel bonusFuel = new BonusFuel(new Sprite("FuelBones"), new Vector2(1.3f, -5f), GameScene.CarScale);
				bonusFuel.AddMove(new MoveBonus(Key.Up, Key.Down, "FuelBones"));
				return bonusFuel;

			}
			else if (typeof(T) == typeof(BonusBarrel))
			{
				BonusBarrel bonusBarrel = new BonusBarrel(new Sprite("BarrelBonus"), new Vector2(1.4f, -10f), GameScene.CarScale);
				bonusBarrel.AddMove(new MoveBonus(Key.Up, Key.Down, "BarrelBonus"));
				return bonusBarrel;
			}
			return null;
		}
	}
}
