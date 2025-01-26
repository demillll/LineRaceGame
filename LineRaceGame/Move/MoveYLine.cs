using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.DirectInput;

namespace LineRaceGame
{
	class MoveYLine : MoveObject
	{
		public float speed;

		private Key MoveUp;
		private Key MoveDown;

		private string Animation;



		public MoveYLine(Key MoveUp, Key MoveDown, string Animation)
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


				if (@object.position.center.Y > 1.4f)
				{
					{
						@object.position.center.Y = -0.2f;

					}
				}
				if (@object.position.center.Y < -0.2f)
				{
					{
						@object.position.center.Y = 1.4f;

					}
				}



			}


		}




	}
}