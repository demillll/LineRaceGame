using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRaceGame
{
    public class BarrelDecorates : CarDecorators
    {
        public BarrelDecorates(Car car) : base(car)
        {
            car.maxFuel += 25;
        }
    }
}
