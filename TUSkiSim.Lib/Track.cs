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
        private readonly int level;
        private readonly Lift lift;
        private readonly int number;
        private int peopleOnTrack;
        //private readonly double workload;

        public Track(int number, int length, int level, int capacity, Lift lift) : this(number, length, level, null, capacity, lift)
        { }

        public Track(int number, int length, int level, Hut hut, int capacity, Lift lift) 
        {
            this.number = number;
            this.length = length;
            this.level = level;
            this.capacity = capacity;
            this.lift = lift;
            this.hut = hut;
        }

        public Hut GetHut() =>
            hut;

        public int GetLength() =>
            length;

        public int GetLevel() =>
            level;

        public Lift GetLift() =>
            lift;

        public int GetNumber() =>
            number;

        public int GetPeopleOnTrack() =>
            peopleOnTrack;

        public double CalcWorkload() =>
            ((double)peopleOnTrack) / capacity;

        public void ChangePeopleOnTrack(int numPeople) =>
            peopleOnTrack = numPeople;
    }
}
