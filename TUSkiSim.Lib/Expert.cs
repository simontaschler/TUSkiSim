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

        public override double GetPropbabilityHut() =>
            probHutBasic * .5;  //if-Abfrage überflüssig
            //VisitedHuts.Count < 1
            //    ? probHutBasic * (1 - VisitedHuts.Count) / 2
            //    : probHutBasic * .5;

        public Expert(int number, int arrivingTime) : base(number, arrivingTime)
        {
            velocity = 250;
            skillLevel = Skill.Expert;
        }

        public override int CalculateNeededTime(Track track) =>
            (int)Math.Ceiling(track.GetLength() / velocity * (1 + track.CalcWorkload() / 2));

        public override Track CalculateNextTrack(List<Track> tracks)
        {
            var rnd = new Random();
            var tracksMatchingSkill = tracks.Where(q => q.GetLevel() <= skillLevel).ToList();
            var nextTrack = tracksMatchingSkill.FirstOrDefault(q =>
            {
                if (q.GetLevel() == Skill.Beginner)
                    return rnd.NextDouble() < .2;
                else if (q.GetLevel() == Skill.Expert)
                    return rnd.NextDouble() < .3;
                else
                    return rnd.NextDouble() < .5;
            });

            return nextTrack ?? tracksMatchingSkill.FirstOrDefault(q => q.GetNumber() == 1);
        }
    }
}
