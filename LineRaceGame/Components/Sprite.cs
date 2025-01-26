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
		private Vector2 positionOfCenter;
		public Vector2 PositionOfCenter { get => positionOfCenter; set => positionOfCenter = value; }

		public GameAnimation animation { get; set; }
		public GameAnimation defaultAnimation { get; set; }

		public Sprite(string animationTitle)
		{
			SetAnimation(animationTitle);
			defaultAnimation = animation;

			// Убедимся, что анимация не null и содержит хотя бы один спрайт
			if (animation != null && animation.sprites.Count > 0)
			{
				// Получаем первый спрайт для установки позиции
				var currentSprite = animation.GetCurrentSprite(this);
				positionOfCenter.X = currentSprite.Size.Width / 2;
				positionOfCenter.Y = currentSprite.Size.Height / 2;
			}
			else
			{
				Console.WriteLine($"Ошибка: Анимация '{animationTitle}' не инициализирована или не содержит кадров.");
			}
		}

		public void SetAnimation(string title)
		{
			if (GameAnimation.animations.ContainsKey(title))
			{
				animation = GameAnimation.animations[title];

				// Если анимация бесконечная, то устанавливаем её как дефолтную
				if (animation.endless)
				{
					defaultAnimation = animation;
				}
			}
			else
			{
				Console.WriteLine($"Ошибка: Анимация с названием '{title}' не найдена.");
			}
		}
	}

}

