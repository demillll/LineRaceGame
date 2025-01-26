using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.DirectInput;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace LineRaceGame
{

	class MoveBonus : MoveObject
	{
		public float speed;

		private Random random;
		private Key MoveUp;
		private Key MoveDown;
		private string Animation;

		public MoveBonus(Key MoveUp, Key MoveDown, string Animation)
		{
			this.MoveUp = MoveUp;
			this.MoveDown = MoveDown;
			this.Animation = Animation;

			random = new Random();
			speed = 0f;

		}
		public override void Update(List<GameObject> gameObjects)
		{
			inputDirectX.UpdateKeyboardState();
			@object.position.center.Y += speed;
			for (int i = 0; i < gameObjects.Count; i++)
			{
				if (gameObjects[i] is Car && ((Car)gameObjects[i]).IsPlayer && (@object.position.center - gameObjects[i].position.center).Length() < 0.17f && (@object is BonusFuel))
				{
					FuelDecorates decorator;
					if (MoveUp == Key.W)
					{
						@object.position.center = new Vector2((float)(random.NextDouble() * (0.54 + 0.43) - 0.43), (float)(random.NextDouble() * (-15 + 20) - 20));
						decorator = new FuelDecorates(GameScene.Car1);
					}
					else
					{
						@object.position.center = new Vector2((float)(random.NextDouble() * (1.82 - 0.83) + 0.83), (float)(random.NextDouble() * (-15 + 20) - 20));
						decorator = new FuelDecorates(GameScene.Car2);
					}
				}

				if (gameObjects[i] is Car && ((Car)gameObjects[i]).IsPlayer && (@object.position.center - gameObjects[i].position.center).Length() < 0.17f && (@object is BonusBarrel))
				{
					BarrelDecorates decorator;
					if (MoveUp == Key.W)
					{
						@object.position.center = new Vector2((float)(random.NextDouble() * (0.54 + 0.43) - 0.43), (float)(random.NextDouble() * (-30 + 40) - 40));
						decorator = new BarrelDecorates(GameScene.Car1);
					}
					else
					{
						@object.position.center = new Vector2((float)(random.NextDouble() * (1.82 - 0.83) + 0.83), (float)(random.NextDouble() * (-30 + 40) - 40));
						decorator = new BarrelDecorates(GameScene.Car2);
					}
				}
			}

			if (inputDirectX.KeyboardUpdated)
			{
				if (inputDirectX.KeyboardState.IsPressed(MoveUp) && !inputDirectX.KeyboardState.IsPressed(MoveDown))
				{

					if (speed < 0.2f)
						speed += 0.00005f;

				}

				if (!inputDirectX.KeyboardState.IsPressed(MoveUp))
				{
					if (speed > 0)
					{
						speed -= 0.00005f;
					}
					else if (speed < 0)
					{
						speed = 0;
					}
				}


				// Движение вверх (торможение)
				if (inputDirectX.KeyboardState.IsPressed(MoveDown) && !inputDirectX.KeyboardState.IsPressed(MoveUp))
				{
					speed -= 0.0001f;
					@object.sprite.SetAnimation(Animation);


				}
			}

		}



	}
}
