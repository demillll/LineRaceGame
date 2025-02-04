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
			if (animationTime <= TimeHelper.Time)
            {
                currentSprite++;
                animationTime += timeCounter;
            }
            if (currentSprite >= sprites.Count)
            {
                if (endless == false)
                {
                    sprite.animation = sprite.defaultAnimation;
                }
                currentSprite = 0;
            }
            return sprites[currentSprite];
		}

	}
}
