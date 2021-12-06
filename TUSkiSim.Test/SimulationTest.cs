using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TUSkiSim.Lib;
using TUSkiSim.Test.TestDerivates;

namespace TUSkiSim.Test
{
    [TestClass]
    public class SimulationTest
    {
        [TestMethod]
        public void TestScenarioSmall()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var csvLines = ResourceHelper.GetEmbeddedResourceLines(executingAssembly, "TUSkiSim.Test.Test.csv");

            var hut1 = new Hut("1", 200, 40);
            var hut2 = new Hut("2", 150, 45);
            var hut3 = new Hut("3", 100, 25);

            var lift1 = new ChairLift(1, 100, 1500, 40, 2);
            var lift2 = new ChairLift(2, 90, 1200, 30, 2);
            var lift3 = new SkiTow(3, 50, 600, 30, 2);

            var lifts = new List<Lift> { lift1, lift2, lift3 };
            var tracks = new List<Track>
            {
                new Track(1, 2500, Skill.Beginner, 120, lift1, hut1),
                new Track(2, 2200, Skill.Advanced, 50, lift1, hut2),
                new Track(3, 1700, Skill.Beginner, 40, lift2, hut3),
                new Track(4, 1600, Skill.Advanced, 40, lift2),
                new Track(5, 800, Skill.Expert, 20, lift3)
            };

            var track1 = tracks.First();
            var skiers = GetTicketList(csvLines, track1);

            var logLines = new List<string>();
            var logger = new Logger(logLines);
            var simulation = new Simulation(lifts, skiers, tracks, logger);
            simulation.Simulate(8, 17);

            var compareLogLines = ResourceHelper.GetEmbeddedResourceLines(executingAssembly, "TUSkiSim.Test.CompLogFile.txt").Skip(1);

            Assert.AreEqual(logLines.Count, compareLogLines.Count());

            for (var i = 1; i < logLines.Count; i++)
                Assert.AreEqual(logLines[i], compareLogLines.ElementAt(i));
        }

        private static List<Skier> GetTicketList(IEnumerable<string> lines, Track track1)
        {
            var skiers = new List<Skier>();
            foreach (var line in lines)
            {
                var fields = line.Split(';');
                var number = int.Parse(fields[0]);
                var arrivalTime = int.Parse(fields[1]);
                var skill = int.Parse(fields[2]);
                switch (skill)
                {
                    case 1: skiers.Add(new TestBeginner(number, arrivalTime, track1)); break;
                    case 2: skiers.Add(new TestAdvanced(number, arrivalTime, track1)); break;
                    case 3: skiers.Add(new TestExpert(number, arrivalTime, track1)); break;
                }
            }
            return skiers;
        }
    }
}
