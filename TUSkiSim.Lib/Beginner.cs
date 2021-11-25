using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUSkiSim.Lib
{
    public class Beginner : Skier
    {
        private readonly double probHutBasic = 1; //in base verschieben, laut Angabe aber hier

        //Properties ersetzen Get-Methoden
        public override double PropbabilityHut =>
            VisitedHuts.Count < 3
                ? probHutBasic * (3 - VisitedHuts.Count)
                : probHutBasic * .5;

        public override int SkillLevel => 1;

        public Beginner(int number, int arrivingTime) : base(number, arrivingTime) => 
            velocity = 50;

        public override Track CalculateNextTrack(List<Track> tracks) 
        {
            var rnd = new Random();
            var tracksMatchingSkill = tracks.Where(q => q.Level <= SkillLevel).ToList();
            var nextTrack = tracksMatchingSkill.SingleOrDefault(q => rnd.Next(0, 1) == 1);

            return nextTrack ?? tracksMatchingSkill.SingleOrDefault(q => q.Number == 1);
        }
    }
}
