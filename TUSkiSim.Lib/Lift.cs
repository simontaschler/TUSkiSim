using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUSkiSim.Lib
{
    public abstract class Lift
    {
        protected readonly int number;
        protected readonly int elements;
        protected readonly int length;
        protected readonly int velocity;
        private int waitingQueue;

        protected Lift(int number, int velocity, int length, int elements) 
        {
            this.number = number;
            this.velocity = velocity;
            this.length = length;
            this.elements = elements;
        }

        public int GetNumber() =>
            number;

        public int GetTravelTime() =>
            length / velocity;

        public int GetWaitingQueue() =>
            waitingQueue;

        public abstract int CalcFlowRate();

        public void AddQueue() => 
            waitingQueue++;

        public void RedWaitingQueue() 
        {
            if (waitingQueue - CalcFlowRate() <= 0)
                waitingQueue = 0;
            else
                waitingQueue -= CalcFlowRate();
        }

        public override string ToString() => 
            $"Lift {number}";
    }
}
