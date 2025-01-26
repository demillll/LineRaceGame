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
	class MoveCarX : MoveObject
	{
		private const float speed = 0.015f;

		private Key MoveRight;
		private Key MoveLeft;

		private float PressRight;
		private float PressLeft;
		private int Number;



		private string Animation;
		public MoveCarX(Key MoveRight, Key MoveLeft, string Animation, int Number)
		{
			this.MoveRight = MoveRight;
			this.MoveLeft = MoveLeft;
			this.Animation = Animation;
			this.Number = Number;

		}
		public override void Update(List<GameObject> gameObjects)
		{
			inputDirectX.UpdateKeyboardState();
			if (inputDirectX.KeyboardUpdated)
			{


				if (inputDirectX.KeyboardState.IsPressed(MoveLeft) && !inputDirectX.KeyboardState.IsPressed(MoveRight))
				{

					PressLeft = speed;
					@object.sprite.SetAnimation(Animation);
					@object.position.center.X -= PressLeft;

				}

				if (inputDirectX.KeyboardState.IsPressed(MoveRight) && !inputDirectX.KeyboardState.IsPressed(MoveLeft))
				{
					PressRight = speed;
					@object.sprite.SetAnimation(Animation);
					@object.position.center.X += PressRight;
				}

				if (Number == 1)
				{
					BorderLeft();
				}
				else if (Number == 2)
				{
					BorderRight();
				}
			}


		}
		private void BorderLeft()
		{
			if (@object.position.center.X < -0.43)
			{
				@object.position.center.X += PressLeft;
			}
			if (@object.position.center.X > 0.55)
			{
				@object.position.center.X -= PressRight;
			}
		}

		private void BorderRight()
		{
			if (@object.position.center.X < 0.83)
			{
				@object.position.center.X += PressLeft;
			}
			if (@object.position.center.X > 1.81)
			{
				@object.position.center.X -= PressRight;
			}
		}
	}
}
