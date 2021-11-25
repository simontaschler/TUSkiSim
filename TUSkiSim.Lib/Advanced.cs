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

        //Properties ersetzen Get-Methoden
        public override double PropbabilityHut =>
            VisitedHuts.Count < 2
                ? probHutBasic * (2 - VisitedHuts.Count) / 2
                : probHutBasic * .5;

        public override int SkillLevel => 2;

        public Advanced(int number, int arrivingTime) : base(number, arrivingTime) => 
            velocity = 150;

        public override Track CalculateNextTrack(List<Track> tracks)
        {
            var rnd = new Random();
            var tracksMatchingSkill = tracks.Where(q => q.Level <= SkillLevel).ToList();
            var nextTrack = tracksMatchingSkill.SingleOrDefault(q =>
            {
                if (q.Level == 1)
                    return rnd.Next(0, 9) < 7;
                else
                    return rnd.Next(0, 9) > 2;
            });

            return nextTrack ?? tracksMatchingSkill.SingleOrDefault(q => q.Number == 1);
        }

    }
}
