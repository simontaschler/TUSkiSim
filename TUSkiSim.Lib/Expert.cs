using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUSkiSim.Lib
{
    public class Expert : Skier
    {
        private readonly double probHutBasic = .2; //in base verschieben

        public override double GetPropbabilityhut() =>
            probHutBasic * .5;  //if-Abfrage überflüssig

        public Expert(int number, int arrivingTime) : base(number, arrivingTime)
        { }

        public override int CalculateNeededTime(Track track) =>
            (int)(track.GetLength() / velocity * (1 - track.CalcWorkload() / 2));

        public override Track CalculateNextTrack(List<Track> tracks)
        {
            var rnd = new Random();
            Track defaultReturnValue = null;

            foreach (var track in tracks)
            {
                if (track.GetLevel() == 1 && rnd.Next(0, 9) < 2)
                    return track;
                else if (track.GetLevel() == 2 && rnd.Next(0, 9) < 3)
                    return track;
                else if (rnd.Next(0, 9) < 5)
                    return track;
                
                if (track.GetNumber() == 1)
                    defaultReturnValue = track;
            }

            return defaultReturnValue;
        }
    }
}
