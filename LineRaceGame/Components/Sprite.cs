using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;

namespace LineRaceGame
{//код содержит определение класса "Sprite", который представляет собой игровой спрайт. Класс имеет следующие поля:

	public class Sprite
	{
		//"positionOfCenter" - вектор, который содержит координаты центра спрайта в игровом пространстве;
		// Положение центра спрайта в игровом пространстве (20 единиц измерения на высоту поля отображения)
		private Vector2 positionOfCenter;
		public Vector2 PositionOfCenter { get => positionOfCenter; set => positionOfCenter = value; }

		//"animation" - объект класса "GameAnimation", который представляет анимацию спрайта;
		public GameAnimation animation { get; set; }
		//"defaultAnimation" - объект класса "GameAnimation", который содержит анимацию по умолчанию.
		public GameAnimation defaultAnimation { get; set; }

		public Sprite(string animationTitle)
		{
			SetAnimation(animationTitle);
			defaultAnimation = animation;
			var currentSprite = animation.GetCurrentSprite(this);
			positionOfCenter.X = currentSprite.Size.Width / 2;
			positionOfCenter.Y = currentSprite.Size.Height / 2;
			
		}

		public void SetAnimation(string title)
		{
			if (GameAnimation.animations.ContainsKey(title))
			{
				if (GameAnimation.animations[title].endless == false)
				{
					this.animation = GameAnimation.animations[title];
				}
				else
				{
					this.animation = GameAnimation.animations[title];
					this.defaultAnimation = GameAnimation.animations[title];
				}
			}
		}
	}

}

