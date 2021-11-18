using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUSkiSim.Lib
{
    public class Track
    {
        private readonly int capacity;
        private readonly Hut hut;
        private readonly int length;
        private readonly int level;  //1,2,3 => durch enum ersetzen
        private readonly Lift lift;
        private readonly int number;
        private int peopleOnTrack;
        //private readonly double workload;

        public Hut Hut => hut;
        public int Length => length;
        public int Level => level;
        public Lift Lift => lift;
        public int Number => number;
        public int PeopleOnTrack => peopleOnTrack;

        public Track(int number, int length, int level, int capacity, Lift lift, Hut hut = null) 
        {
            this.number = number;
            this.length = length;
            this.level = level;
            this.capacity = capacity;
            this.lift = lift;
            this.hut = hut;
        }

        public double CalcWorkload() =>
            ((double)peopleOnTrack) / capacity;

        public void ChangePeopleOnTrack(int numPeople) =>
            peopleOnTrack = numPeople;
    }
}
