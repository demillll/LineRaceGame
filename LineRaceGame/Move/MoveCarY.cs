using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.DirectInput;

namespace LineRaceGame
{
	class MoveCarY : MoveObject
	{
		private float speed;

		private Key MoveUp;
		private Key MoveDown;

		private float PressDown;

		public static BackgroundLoseWin LoseLeft;
		public static BackgroundLoseWin LoseRight;

		private int FuelKoef = 4;

		private string Animation;
		private int Numer;

		private bool IsStop = false;

		public MoveCarY(Key MoveUp, Key MoveDown, string Animation, int Numer)
		{
			this.MoveUp = MoveUp;
			this.MoveDown = MoveDown;
			this.Animation = Animation;
			this.Numer = Numer;
			speed = 0f;
		}

		public override void Update(List<GameObject> gameObjects)
		{
			inputDirectX.UpdateKeyboardState();
			@object.position.center.Y += speed;

			if (inputDirectX.KeyboardUpdated)
			{
				if (Numer == 1 || Numer == 2 || Numer == 3 || Numer == 4)
				{
					foreach (var obj in gameObjects)
					{
						if (obj is Car && ((Car)obj).IsPlayer && (@object.position.center - obj.position.center).Length() < 0.17f)
						{
							speed = 0;
							((Car)obj).IsCrash = true;
							IsStop = true;
						}
					}

					if (((Numer == 1 || Numer == 2) && @object.Site == false && IsStop) ||
						((Numer == 3 || Numer == 4) && @object.Site == true && IsStop))
					{
						speed = 0;
						AddImages.CreateBackgroundLoses();
						LoseLeft = AddImages.CreateBackgroundLoses()[Numer <= 2 ? 0 : 1];
						LoseLeft.IsActiv = true;
					}

					if (!((Car)@object).IsPlayer)
					{
						// Движение вниз (ускорение)
						if (inputDirectX.KeyboardState.IsPressed(MoveUp) && !inputDirectX.KeyboardState.IsPressed(MoveDown))
						{
							if (speed < 0.2f)
								speed += 0.00005f;
							if (((Car)@object).Fuel > 0)
							{
								((Car)@object).Fuel -= speed * FuelKoef;
								if (((Car)@object).Fuel <= 0)
								{
									speed = 0;
									AddImages.CreateBackgroundLoses();
									LoseLeft = AddImages.CreateBackgroundLoses()[Numer <= 2 ? 0 : 1];
									LoseLeft.IsActiv = true;
								}
							}
						}

						// Торможение
						if (inputDirectX.KeyboardState.IsPressed(MoveDown) && !inputDirectX.KeyboardState.IsPressed(MoveUp))
						{
							speed -= 0.0001f;
							@object.sprite.SetAnimation(Animation);
							@object.position.center.Y -= PressDown;
						}

						if (!inputDirectX.KeyboardState.IsPressed(MoveUp))
						{
							if (speed > 0)
								speed -= 0.00005f;
							else if (speed < 0)
								speed = 0;
						}
					}

					// Перемещение в случайную зону
					Random rnd = new Random();
					float zoneX = Numer switch
					{
						1 => RandomFloat(rnd, -0.43, 0),
						2 => RandomFloat(rnd, 0, 0.54),
						3 => RandomFloat(rnd, 0.83, 1.36),
						4 => RandomFloat(rnd, 1.36, 1.82),
						_ => 0
					};
					float zoneYLimit = RandomFloat(rnd, 2, 15);

					if (@object.position.center.Y > zoneYLimit)
					{
						@object.position.center.Y = -0.8f;
						@object.position.center.X = zoneX;
					}
					if (@object.position.center.Y < -0.8f)
					{
						@object.position.center.Y = zoneYLimit;
						@object.position.center.X = zoneX;
					}
				}
			}
		}

		private float RandomFloat(Random random, double min, double max)
		{
			return (float)(min + random.NextDouble() * (max - min));
		}
	}
}
