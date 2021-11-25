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

        //Properties ersetzen Get-Methoden
        public int Number => number;
        public int TravelTime => length / velocity;
        public int WaitingQueue => waitingQueue;

        public abstract int CalcFlowRate();

        public void AddQueue() => 
            waitingQueue++;

        public void RedWaitingQueue() =>
            waitingQueue -= waitingQueue - CalcFlowRate() <= 0
                ? waitingQueue
                : CalcFlowRate();

        //nur für Logger
        public override string ToString() => 
            $"Lift {number}";
    }
}
