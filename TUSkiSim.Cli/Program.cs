using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUSkiSim.Lib;

namespace TUSkiSim.Cli
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            var skiers = GetTicketList(@"C:\Users\SimonT\Documents\Uni\WS21-22\Ingenieurinformatik 2\Basisprojekte\1\v3\Test.CSV");
            
            var hut1 = new Hut("1", 200, 40);
            var hut2 = new Hut("2", 150, 45);
            var hut3 = new Hut("3", 100, 25);

            var lift1 = new ChairLift(1, 100, 1500, 40, 2/*4*/);
            var lift2 = new ChairLift(2, 90, 1200, 30, 2);
            var lift3 = new SkiTow(3, 50, 600, 30, 2);
            
            var lifts = new List<Lift> { lift1, lift2, lift3 };
            var huts = new List<Hut> { hut1, hut2, hut3 };
            var tracks = new List<Track> 
            { 
                new Track(1, 2500, 1, hut1, 120, lift1),
                new Track(2, 2200, 2, hut2, 50, lift1),
                new Track(3, 1700, 1, hut3, 40, lift2),
                new Track(4, 1600, 2, 40, lift2),
                new Track(5, 800, 3, 20, lift3),
            };

            var logger = new Logger();
            var simulation = new Simulation(lifts, skiers, tracks, logger);
            simulation.Simulate(8, 17);

            logger.WriteToFile(@"C:\Users\SimonT\Documents\Uni\WS21-22\Ingenieurinformatik 2\Basisprojekte\1\v3\Result_logfile.txt");

            Console.ReadLine();
        }

        private static List<Skier> GetTicketList(string file)
        {
            var lines = File.ReadAllLines(file);
            var skiers = new List<Skier>();
            foreach (var line in lines)
            {
                var fields = line.Split(';');
                var number = int.Parse(fields[0]);
                var arrivalTime = int.Parse(fields[1]);
                var skill = int.Parse(fields[2]);
                switch (skill)
                {
                    case 1: skiers.Add(new Beginner(number, arrivalTime)); break;
                    case 2: skiers.Add(new Advanced(number, arrivalTime)); break;
                    case 3: skiers.Add(new Expert(number, arrivalTime)); break;
                }
            }
            return skiers;
        }
    }
}
