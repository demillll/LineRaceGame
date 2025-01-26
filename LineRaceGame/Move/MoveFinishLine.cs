using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.DirectInput;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LineRaceGame
{
	class MoveFinishLine : MoveObject
	{
		public float speed;

		private Key MoveUp;
		private Key MoveDown;

		public static BackgroundLoseWin LoseLeft;
		public static BackgroundLoseWin LoseRight;
		public static BackgroundLoseWin WinLeft;
		public static BackgroundLoseWin WinRight;

		private string Animation;

		private bool IsStop = false;
		private bool IsWin = false;
		public MoveFinishLine(Key MoveUp, Key MoveDown, string Animation)
		{
			this.MoveUp = MoveUp;
			this.MoveDown = MoveDown;
			this.Animation = Animation;
			speed = 0f;

		}

		public override void Update(List<GameObject> gameObjects)
		{
			inputDirectX.UpdateKeyboardState();
			@object.position.center.Y += speed;
			foreach (var obj in gameObjects)
			{
				if (obj is Car && ((Car)obj).IsPlayer && @object.position.center.Y > obj.position.center.Y + 0.8)
				{
					speed = 0;
					((Car)obj).IsCrash = true;
					IsWin = true;
				}

			}

			if (IsWin)
			{
				if (@object.Site == false)
				{
					AddImages.CreateBackgroundLoses();
					LoseLeft = AddImages.CreateBackgroundLoses()[1];
					LoseLeft.IsActiv = true;

					WinRight = AddImages.CreateBackgroundLoses()[2];
					WinRight.IsActiv = true;
				}
				else
				{
					AddImages.CreateBackgroundLoses();
					LoseRight = AddImages.CreateBackgroundLoses()[0];
					LoseRight.IsActiv = true;


					WinLeft = AddImages.CreateBackgroundLoses()[3];
					WinLeft.IsActiv = true;
				}
			}

			if (@object.Site == true && IsStop == true)
			{
				speed = 0;
			}

			if (inputDirectX.KeyboardUpdated)
			{
				if (inputDirectX.KeyboardState.IsPressed(MoveUp) && !inputDirectX.KeyboardState.IsPressed(MoveDown))
				{
					speed += 0.00003f;
					@object.sprite.SetAnimation(Animation);

				}
				if (inputDirectX.KeyboardState.IsPressed(MoveDown) && !inputDirectX.KeyboardState.IsPressed(MoveUp))
				{
					speed -= 0.00003f;
					@object.sprite.SetAnimation(Animation);

				}

				if (!inputDirectX.KeyboardState.IsPressed(MoveUp))
				{
					if (speed > 0)
					{
						speed -= 0.00003f;
					}
					else if (speed < 0)
					{
						speed = 0;
					}
				}




			}


		}




	}
}