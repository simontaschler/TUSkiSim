using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUSkiSim.Lib
{
    public class Beginner : Skier
    {
        private readonly double probHutBasic = 1; //in base verschieben

        public override double GetPropbabilityHut() 
        {
            if (visitedHuts.Count < 3)
                return probHutBasic * (3 - visitedHuts.Count);
            else
                return probHutBasic * .5;
        }

        public Beginner(int number, int arrivingTime) : base(number, arrivingTime)
        { }

        public override Track CalculateNextTrack(List<Track> tracks) 
        {
            var rnd = new Random();
            Track defaultReturnValue = null;

            foreach (var track in tracks) 
            { 
                if (track.GetLevel() <= skillLevel) 
                {
                    if (rnd.Next(0, 1) == 1)
                        return track;

                    if (track.GetNumber() == 1)
                        defaultReturnValue = track;
                }
            }

            return defaultReturnValue;
        }
    }
}
