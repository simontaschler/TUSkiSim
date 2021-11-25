using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUSkiSim.Lib
{
    public abstract class Lift
    {
        protected readonly int elements;
        protected readonly int length;
        protected readonly int velocity;

        //Auto-Properties ersetzen Get-Methoden und private Felder
        public int Number { get; }
        public int WaitingQueue { get; private set; }
        //Property ersetzt Get-Methoden
        public int TravelTime => length / velocity;

        protected Lift(int number, int velocity, int length, int elements) 
        {
            Number = number;
            this.velocity = velocity;
            this.length = length;
            this.elements = elements;
        }

        public abstract int CalcFlowRate();

        public void AddQueue() => 
            WaitingQueue++;

        public void RedWaitingQueue() =>
            WaitingQueue -= WaitingQueue - CalcFlowRate() <= 0
                ? WaitingQueue
                : CalcFlowRate();

        //nur für Logger
        public override string ToString() => 
            $"Lift {Number}";
    }
}
