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

        public override double GetPropbabilityHut() =>
            visitedHuts.Count < 3
                ? probHutBasic * (3 - visitedHuts.Count)
                : probHutBasic * .5;

        public Beginner(int number, int arrivingTime) : base(number, arrivingTime)
        {
            velocity = 50;
            skillLevel = Skill.Beginner;
        }

        public override Track CalculateNextTrack(List<Track> tracks) 
        {
            var rnd = new Random();
            var tracksMatchingSkill = tracks.Where(q => q.GetLevel() <= skillLevel).ToList();
            var nextTrack = tracksMatchingSkill.FirstOrDefault(q => rnd.Next(0, 1) == 1);

            return nextTrack ?? tracksMatchingSkill.FirstOrDefault(q => q.GetNumber() == 1);
        }
    }
}
