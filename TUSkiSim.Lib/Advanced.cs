using System;
using System.Collections.Generic;
using System.Linq;

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

        public override Skill SkillLevel => Skill.Advanced;

        public Advanced(int number, int arrivingTime) : base(number, arrivingTime) =>
            velocity = 150;

        public override Track CalculateNextTrack(List<Track> tracks)
        {
            var rnd = new Random();
            var tracksMatchingSkill = tracks.Where(q => q.Level <= SkillLevel);
            var nextTrack = tracksMatchingSkill.FirstOrDefault(q =>
            {
                if (q.Level == Skill.Beginner)
                    return rnd.NextDouble() < .7;
                else
                    return rnd.NextDouble() < .3;
            });

            return nextTrack ?? tracksMatchingSkill.FirstOrDefault(q => q.Number == 1);
        }

    }
}
