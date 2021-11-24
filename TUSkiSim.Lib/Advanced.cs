using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUSkiSim.Lib
{
    public class Advanced : Skier
    {
        private readonly double probHutBasic = .8; //in base verschieben

        public override double GetPropbabilityHut()
        {
            //if (visitedHuts.Count < 2)
            //    return probHutBasic * (2 - visitedHuts.Count) / 2;
            //else
            //    return probHutBasic * .5;
            return 0;
        }

        public Advanced(int number, int arrivingTime) : base(number, arrivingTime)
        {
            velocity = 150;
            skillLevel = 2;
        }

        public override Track CalculateNextTrack(List<Track> tracks)
        {
            var rnd = new Random();
            Track defaultReturnValue = null;

            foreach (var track in tracks)
            {
                if (track.GetLevel() <= skillLevel)
                {
                    //if (track.GetLevel() == 1 && rnd.Next(0, 9) < 7)
                    //    return track;
                    //else if (rnd.Next(0, 9) > 2)
                    //    return track;

                    if (track.GetNumber() == 1)
                        defaultReturnValue = track;
                }
            }

            return defaultReturnValue;
        }

    }
}
