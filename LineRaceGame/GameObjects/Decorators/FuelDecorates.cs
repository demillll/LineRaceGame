using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRaceGame
{
    public class FuelDecorates : CarDecorators
    {
        public FuelDecorates(Car car) : base(car)
        {
            car.Fuel += 50;
        }
    }
}
