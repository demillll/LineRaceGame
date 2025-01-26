using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRaceGame
{
	public abstract class MoveObject
	{
		protected InputDirectX inputDirectX = GameScene.inputDirectX;

		protected GameObject @object;

		public void SignToObject(GameObject @object)
		{
			this.@object = @object;
		}

		public abstract void Update(List<GameObject> gameObjects);
	}

}

