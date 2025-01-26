using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace LineRaceGame
{
    public abstract class CarDecorators : Car
    {
        public Car car;

        public CarDecorators(Car car) : base()
        {
            this.car = car;
        }
    }
}
