using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUSkiSim.Lib
{
    public class ChairLift : Lift
    {
        private readonly int seats;

        public ChairLift(int number, int velocity, int length, /*double probFailure,*/ int elements, int seats) : base(number, velocity, length, elements) => 
            this.seats = seats;

        public override int CalcFlowRate() =>
            (int)(seats * velocity * (((double)elements) / length));
    }
}
