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

        public override double GetPropbabilityHut() =>
            visitedHuts.Count < 2
                ? probHutBasic * (2 - visitedHuts.Count) / 2
                : probHutBasic * .5;

        public Advanced(int number, int arrivingTime) : base(number, arrivingTime)
        {
            velocity = 150;
            skillLevel = Skill.Advanced;
        }

        public override Track CalculateNextTrack(List<Track> tracks)
        {
            var rnd = new Random();
            var tracksMatchingSkill = tracks.Where(q => q.GetLevel() <= skillLevel).ToList();
            var nextTrack = tracksMatchingSkill.FirstOrDefault(q =>
            {
                if (q.GetLevel() == Skill.Beginner)
                    return rnd.Next(0, 9) < 7;
                else
                    return rnd.Next(0, 9) < 2;
            });

            return nextTrack ?? tracksMatchingSkill.FirstOrDefault(q => q.GetNumber() == 1);
        }

    }
}
