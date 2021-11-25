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
        //private readonly double workload;

        //Auto-Properties ersetzen Get-Methoden und private Felder
        public Hut Hut { get; }
        public int Length { get; }
        public Skill Level { get; }
        public Lift Lift { get; }
        public int Number { get; }
        public int PeopleOnTrack { get; set; } //ChangePeopleOnTrack durch set ersetzt

        public Track(int number, int length, Skill level, int capacity, Lift lift, Hut hut = null)
        {
            Number = number;
            Length = length;
            Level = level;
            this.capacity = capacity;
            Lift = lift;
            Hut = hut;
            PeopleOnTrack = 0;
        }

        public double CalcWorkload() =>
            ((double)PeopleOnTrack) / capacity;
    }
}
