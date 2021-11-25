using System;
using System.Collections.Generic;
using System.Linq;

namespace TUSkiSim.Lib
{
    public class Expert : Skier
    {
        private readonly double probHutBasic = .2; //in base verschieben

        //Properties ersetzen Get-Methoden
        public override double PropbabilityHut => probHutBasic * .5;  //if-Abfrage überflüssig
        //VisitedHuts.Count < 1
        //    ? probHutBasic * (1 - VisitedHuts.Count) / 2
        //    : probHutBasic * .5;

        public override Skill SkillLevel => Skill.Expert;

        public Expert(int number, int arrivingTime) : base(number, arrivingTime) =>
            velocity = 250;

        public override int CalculateNeededTime(Track track) =>
            (int)Math.Ceiling(track.Length / velocity * (1 + track.CalcWorkload() / 2));

        public override Track CalculateNextTrack(List<Track> tracks)
        {
            var rnd = new Random();
            var tracksMatchingSkill = tracks.Where(q => q.Level <= SkillLevel);
            var nextTrack = tracksMatchingSkill.FirstOrDefault(q =>
            {
                if (q.Level == Skill.Beginner)
                    return rnd.NextDouble() < .2;
                else if (q.Level == Skill.Advanced)
                    return rnd.NextDouble() < .3;
                else
                    return rnd.NextDouble() < .5;
            });

            return nextTrack ?? tracksMatchingSkill.FirstOrDefault(q => q.Number == 1);
        }
    }
}
