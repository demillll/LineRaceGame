using SharpDX;
using System.Numerics;
using SharpDX.DirectInput;


namespace LineRaceGame
{
	public static class AddImages
	{//Метод AddAnimations() создает объекты GameAnimation для анимированных изображений,
	 //таких как фон, изображения победы и поражения, полосы дороги, автомобили и т.д.
		public static void AddAnimations()
		{
			new GameAnimation(GameScene.dx2d.LoadBitmap(@"Elements\Image\Background.png"), 0.5f, "Background", true);
			new GameAnimation(GameScene.dx2d.LoadBitmap(@"Elements\Image\Lose.png"), 0.5f, "Lose", true);
			new GameAnimation(GameScene.dx2d.LoadBitmap(@"Elements\Image\Win.png"), 0.5f, "Win", true);
			new GameAnimation(GameScene.dx2d.LoadBitmap(@"Elements\Image\Line1.png"), 0.5f, "Line1", true);
			new GameAnimation(GameScene.dx2d.LoadBitmap(@"Elements\Image\Car1.png"), 0.5f, "Car1", true);
			new GameAnimation(GameScene.dx2d.LoadBitmap(@"Elements\Image\Car2.png"), 0.5f, "Car2", true);
			new GameAnimation(GameScene.dx2d.LoadBitmap(@"Elements\Image\Car3.png"), 0.5f, "Car3", true);
			new GameAnimation(GameScene.dx2d.LoadBitmap(@"Elements\Image\Finish.png"), 0.5f, "Finish", true);
			new GameAnimation(GameScene.dx2d.LoadBitmap(@"Elements\Image\Fuel.png"), 0.5f, "FuelBones", true);
			new GameAnimation(GameScene.dx2d.LoadBitmap(@"Elements\Image\Barrel.png"), 0.5f, "BarrelBonus", true);
		}

		//Метод CreateObjects() создает объекты игры, такие как полосы для автомобилей, автомобили, финишные линии и бонусы. 
		public static void CreateObjects()
		{
			//Крайня левая полоса левго игрока
			Line Line1 = new Line(new Sprite("Line1"), new Vector2(0.05f, 0.0f), GameScene.CarScale, false);
			Line1.AddMove(new MoveYLine(Key.W, Key.S, "Line1"));
			Line Line2 = new Line(new Sprite("Line1"), new Vector2(0.05f, 0.4f), GameScene.CarScale, false);
			Line2.AddMove(new MoveYLine(Key.W, Key.S, "Line1"));
			Line Line3 = new Line(new Sprite("Line1"), new Vector2(0.05f, 0.8f), GameScene.CarScale, false);
			Line3.AddMove(new MoveYLine(Key.W, Key.S, "Line1"));
			Line Line4 = new Line(new Sprite("Line1"), new Vector2(0.05f, 1.2f), GameScene.CarScale, false);
			Line4.AddMove(new MoveYLine(Key.W, Key.S, "Line1"));

			//Крайняя правая полоса левого игрока
			Line Line5 = new Line(new Sprite("Line1"), new Vector2(0.45f, 0.0f), GameScene.CarScale, false);
			Line5.AddMove(new MoveYLine(Key.W, Key.S, "Line1"));
			Line Line6 = new Line(new Sprite("Line1"), new Vector2(0.45f, 0.4f), GameScene.CarScale, false);
			Line6.AddMove(new MoveYLine(Key.W, Key.S, "Line1"));
			Line Line7 = new Line(new Sprite("Line1"), new Vector2(0.45f, 0.8f), GameScene.CarScale, false);
			Line7.AddMove(new MoveYLine(Key.W, Key.S, "Line1"));
			Line Line8 = new Line(new Sprite("Line1"), new Vector2(0.45f, 1.2f), GameScene.CarScale, false);
			Line8.AddMove(new MoveYLine(Key.W, Key.S, "Line1"));


			//Крайня левая полоса правого игрока
			Line Line9 = new Line(new Sprite("Line1"), new Vector2(1.35f, 0.0f), GameScene.CarScale, true);
			Line9.AddMove(new MoveYLine(Key.Up, Key.Down, "Line1"));
			Line Line10 = new Line(new Sprite("Line1"), new Vector2(1.35f, 0.4f), GameScene.CarScale, true);
			Line10.AddMove(new MoveYLine(Key.Up, Key.Down, "Line1"));
			Line Line11 = new Line(new Sprite("Line1"), new Vector2(1.35f, 0.8f), GameScene.CarScale, true);
			Line11.AddMove(new MoveYLine(Key.Up, Key.Down, "Line1"));
			Line Line12 = new Line(new Sprite("Line1"), new Vector2(1.35f, 1.2f), GameScene.CarScale, true);
			Line12.AddMove(new MoveYLine(Key.Up, Key.Down, "Line1"));

			//Крайняя правая полоса правого игрока
			Line Line13 = new Line(new Sprite("Line1"), new Vector2(1.75f, 0.0f), GameScene.CarScale, true);
			Line13.AddMove(new MoveYLine(Key.Up, Key.Down, "Line1"));
			Line Line14 = new Line(new Sprite("Line1"), new Vector2(1.75f, 0.4f), GameScene.CarScale, true);
			Line14.AddMove(new MoveYLine(Key.Up, Key.Down, "Line1"));
			Line Line15 = new Line(new Sprite("Line1"), new Vector2(1.75f, 0.8f), GameScene.CarScale, true);
			Line15.AddMove(new MoveYLine(Key.Up, Key.Down, "Line1"));
			Line Line16 = new Line(new Sprite("Line1"), new Vector2(1.75f, 1.2f), GameScene.CarScale, true);
			Line16.AddMove(new MoveYLine(Key.Up, Key.Down, "Line1"));



			// Авто левого персонажа
			Car Player1 = new Car(new Sprite("Car1"), new Vector2(0.31f, 0.4f), GameScene.CarScale, false, true);
			Player1.AddMove(new MoveCarX(Key.D, Key.A, "Car1", 1));
			Car Car2 = new Car(new Sprite("Car3"), new Vector2(0.42f, -0.6f), GameScene.CarScale, false, false);
			Car2.AddMove(new MoveCarY(Key.W, Key.S, "Car3", 2));


			// Авто правого персонажа
			Car Player2 = new Car(new Sprite("Car2"), new Vector2(1.6f, 0.4f), GameScene.CarScale, true, true);
			Player2.AddMove(new MoveCarX(Key.Right, Key.Left, "Car2", 2));
			Car Car4 = new Car(new Sprite("Car3"), new Vector2(1.65f, -0.65f), GameScene.CarScale, true, false);
			Car4.AddMove(new MoveCarY(Key.Up, Key.Down, "Car3", 4));

			FactoryBonus.CreateBonusLeft<BonusFuel>();
			FactoryBonus.CreateBonusLeft<BonusBarrel>();

			FactoryBonus.CreateBonusRight<BonusFuel>();
			FactoryBonus.CreateBonusRight<BonusBarrel>();

			Car FinishRight = new Car(new Sprite("Finish"), new Vector2(1.50f, -50.0f), GameScene.CarScale, true, false);
			FinishRight.AddMove(new MoveFinishLine(Key.Up, Key.Down, "Finish"));

			Car FinishLeft = new Car(new Sprite("Finish"), new Vector2(0.25f, -50.0f), GameScene.CarScale, false, false);
			FinishLeft.AddMove(new MoveFinishLine(Key.W, Key.S, "Finish"));

		}

		//Метод CreateCars() создает массив из двух объектов Car, которые представляют автомобили игроков. Оба объекта имеют спрайт "Car3",
		//который представляет общий автомобиль, но каждый объект имеет свою позицию и управление.
		public static Car[] CreateCars()
		{

			Car[] cars = new Car[2];


			Car Car1 = new Car(new Sprite("Car3"), new Vector2(-0.42f, -0.8f), GameScene.CarScale, false, false);
			cars[0] = Car1;
			Car1.AddMove(new MoveCarY(Key.W, Key.S, "Car3", 1));

			Car Car3 = new Car(new Sprite("Car3"), new Vector2(1f, -0.8f), GameScene.CarScale, true, false);
			cars[1] = Car3;
			Car3.AddMove(new MoveCarY(Key.Up, Key.Down, "Car3", 3));

			return cars;

		}
		//Метод CreateBackgroundLoses() создает массив из четырех объектов BackgroundLoseWin, которые представляют изображения для экрана победы и поражения.
		public static BackgroundLoseWin[] CreateBackgroundLoses()
		{
			BackgroundLoseWin[] backgroundLosesWIn = new BackgroundLoseWin[4];

			BackgroundLoseWin backgroundLoseLeft = new BackgroundLoseWin(new Sprite("Lose"), new Vector2(-0.019f, -0.2f), GameScene.FonScale, false);
			backgroundLosesWIn[0] = backgroundLoseLeft;

			BackgroundLoseWin backgroundLoseRight = new BackgroundLoseWin(new Sprite("Lose"), new Vector2(0.408f, -0.2f), GameScene.FonScale, true);
			backgroundLosesWIn[1] = backgroundLoseRight;

			BackgroundLoseWin backgroundWinLeft = new BackgroundLoseWin(new Sprite("Win"), new Vector2(-0.019f, -0.2f), GameScene.FonScale, false);
			backgroundLosesWIn[2] = backgroundWinLeft;

			BackgroundLoseWin backgroundWinRight = new BackgroundLoseWin(new Sprite("Win"), new Vector2(0.408f, -0.2f), GameScene.FonScale, true);
			backgroundLosesWIn[3] = backgroundWinRight;

			return backgroundLosesWIn;
		}
	}
}
