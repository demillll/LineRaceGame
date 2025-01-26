using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace LineRaceGame
{
	public class Collider
	{//*collidersOfGameObjects" - список игровых объектов, к которым относятся коллайдеры;
		private static List<GameObject> collidersOfGameObjects = new List<GameObject>();
		//"boundCorners" - массив векторов, который содержит координаты четырех углов коллайдера;
		public Vector2[] boundCorners = new Vector2[4];
		//"gameObject" - ссылка на игровой объект, которому принадлежит данный коллайдер;
		private GameObject gameObject;
		//"colliderScale" - вектор, который содержит информацию об увеличении/уменьшении коллайдера.
		private Vector2 colliderScale;

		public Collider(GameObject gameObject, Vector2 scale)
		{
			collidersOfGameObjects.Add(gameObject);
			this.gameObject = gameObject;
			colliderScale = scale;
		}
	}
}
