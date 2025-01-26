using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;

namespace LineRaceGame
{
	// Код содержит определение класса "GameAnimation", который представляет анимацию игрового объекта.
	public class GameAnimation
	{
		// Список изображений (SharpDX.Direct2D1.Bitmap), которые представляют кадры анимации
		public List<SharpDX.Direct2D1.Bitmap> sprites;
		// Индекс текущего кадра анимации
		public int currentSprite;
		// Время между сменой кадров анимации
		private float timeCounter;
		// Время последней смены кадра анимации
		private float animationTime;
		// Словарь, который содержит все анимации игры
		public static Dictionary<string, GameAnimation> animations = new Dictionary<string, GameAnimation>();
		// Флаг, который указывает, будет ли анимация повторяться бесконечно
		public bool endless;
		// Заголовок (название) анимации
		public string title;

		public GameAnimation(List<SharpDX.Direct2D1.Bitmap> sprites, float timeCounter, string title, bool endless)
		{
			this.endless = endless;
			this.sprites = sprites;
			currentSprite = 0;
			this.timeCounter = timeCounter;
			animationTime = timeCounter;
			animations.Add(title, this);
			this.title = title;
		}

		public SharpDX.Direct2D1.Bitmap GetCurrentSprite(Sprite sprite)
		{
			// Проверка на корректность времени для смены кадра
			if (animationTime <= TimeHelper.Time)
			{
				currentSprite++;  // Переход к следующему кадру
				animationTime += timeCounter;
			}

			// Если индекс кадра выходит за пределы списка, сбрасываем его в 0 или в конец (если анимация бесконечная)
			if (currentSprite >= sprites.Count)
			{
				if (!endless)
				{
					sprite.animation = sprite.defaultAnimation; // Смена анимации по умолчанию, если анимация не бесконечная
				}
				currentSprite = 0;  // Возврат на первый кадр, если анимация не бесконечная
			}

			// Проверка на допустимость индекса
			if (currentSprite >= 0 && currentSprite < sprites.Count)
			{
				return sprites[currentSprite];  // Возвращаем кадр, если индекс в пределах списка
			}
			else
			{
				Console.WriteLine($"Ошибка: индекс кадра ({currentSprite}) выходит за пределы списка.");
				return null;  // Возвращаем null, если произошла ошибка с индексом
			}
		}

	}
}
