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
            var csvLines = ResourceHelper.GetEmbeddedResourceLines(executingAssembly, "TUSkiSim.Test.Resources.Test.csv");

            var hut1 = new Hut("1", 200, 40);
            var lift1 = new ChairLift(1, 100, 1500, 40, 2);
            var track1 = new Track(1, 2500, Skill.Beginner, 120, lift1, hut1);


            var lifts = new List<Lift> { lift1 };
            var tracks = new List<Track> { track1 };

            var skiers = GetTicketList(csvLines, track1);

            var logLines = new List<string>();
            var logger = new Logger(logLines);
            var simulation = new Simulation(lifts, skiers, tracks, logger);
            simulation.Simulate(8, 17);

            var compareLogLines = ResourceHelper.GetEmbeddedResourceLines(executingAssembly, "TUSkiSim.Test.Resources.CompLogFile.txt").Skip(1);

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
