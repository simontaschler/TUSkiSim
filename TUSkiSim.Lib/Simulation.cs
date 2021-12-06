using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUSkiSim.Lib
{
    public class Simulation
    {
        private readonly List<Lift> addedLifts;
        private readonly List<Skier> addedSkiers;
        private readonly List<Track> addedTracks;
        private bool status;

        private readonly Lift lift1;

        private readonly Logger logger;

        public Simulation(List<Lift> lifts, List<Skier> skiers, /*List<Hut> huts,*/ List<Track> tracks, Logger logger = null) 
        {
            addedLifts = lifts;
            addedSkiers = skiers;
            addedTracks = tracks;
            this.logger = logger;
            lift1 = addedLifts.FirstOrDefault(q => q.GetNumber() == 1) ?? addedLifts.First();
        }

        public void Simulate(int startTime, int endTime) 
        {
            startTime *= 60;
            endTime *= 60;

            status = false;

            for (var time = startTime; time <= endTime; time++) 
            {
                foreach (var skier in addedSkiers) 
                {
                    if (skier.GetTimeToNextStep() == 0)
                    {
                        if (skier.GetStatus() != Status.LastRun && time >= skier.GetArrivingTime())
                        {
                            if (skier.GetStatus() == Status.PreArrival)
                            {
                                //4.1
                                HandleBeforeFirstRun(skier);
                            }
                            else if (skier.GetStatus() == 0 && time < endTime - 90)
                            {
                                //4.2
                                HandleExitLift(skier);
                            }
                            else if (skier.GetStatus() == 0 /*&& time >= endTime - 90*/)
                            {
                                //4.3
                                HandleLastRun(skier, time);
                            }
                            else
                            {
                                //4.4
                                HandleEndOfTrack(skier);
                            }
                        }
                        else if (skier.GetStatus() == Status.LastRun && time == skier.GetLeavingTime())
                        {
                            //3.2: 10.
                            var lastTrack = skier.GetUsedTracks().Last();
                            lastTrack.ChangePeopleOnTrack(lastTrack.GetPeopleOnTrack() - 1);

                            logger?.AppendTask("Skigebiet verlassen");
                        }
                        else
                        {
                            logger?.AppendTask("keine Aktion");
                        }
                    }
                    else
                    {
                        logger?.AppendTask("keine Aktion");
                    }
                    skier.CountDownTime();

                    logger?.Log(time, skier);
                }

                foreach (var lift in addedLifts)
                    lift.RedWaitingQueue();

                if (time % 30 == 0 && time > startTime)
                    PrintTracks(time);
            }

            PrintSkiers();

            status = true;
        }

        private void HandleBeforeFirstRun(Skier skier) 
        {
            //4.1: 1.
            if (skier.GetWaitingNumber() == -1) 
            {
                lift1.AddQueue();
                skier.SetWaitingNumber(lift1.GetWaitingQueue());

                logger?.AppendTask($"4.1 Wartenr: {skier.GetWaitingNumber()}");
            }

            if (lift1.CalcFlowRate() >= skier.GetWaitingNumber()) 
            {
                //4.1.1: 2.
                skier.SetStatus(0);
                skier.AddUsedLift(lift1);
                skier.SetTimeToNextStep(lift1.GetTravelTime());
                skier.SetWaitingNumber(-1);

                logger?.AppendTask($"4.1.1 Lift wählen: {lift1}");
            }
            else 
            {
                //4.1.2: 3.
                skier.SetWaitingNumber(skier.GetWaitingNumber() - lift1.CalcFlowRate());

                logger?.AppendTask($"4.1.2 Wartenummer reduzieren zu: {skier.GetWaitingNumber()}");
            }
        }

        private void HandleExitLift(Skier skier) 
        {
            //4.2: 4.
            var nextTrack = skier.CalculateNextTrack(addedTracks);
            nextTrack.ChangePeopleOnTrack(nextTrack.GetPeopleOnTrack() + 1);
            skier.SetStatus(Status.OnTrack);
            skier.AddUsedTrack(nextTrack);
            skier.SetTimeToNextStep(skier.CalculateNeededTime(nextTrack));

            logger?.AppendTask($"4.2 nächste Strecke Track: {nextTrack.GetNumber()}");

            if (nextTrack.GetHut() != null)
            {
                //4.2.1
                var rnd = new Random();
                var hut = nextTrack.GetHut();
                if (skier.GetPropbabilityHut() > rnd.NextDouble() && hut.GetMaxGuests() > hut.GetGuests())
                {
                    //4.2.1.1: 5.
                    skier.SetTimeToNextStep(skier.GetTimeToNextStep() + hut.GetAverageStay());
                    skier.AddVisitedHut(hut);
                    hut.AddGuests();
                }
            }
        }

        private void HandleLastRun(Skier skier, int time) 
        {
            //4.3: 6.
            skier.SetStatus(Status.LastRun);
            var lastTrack = skier.CalculateNextTrack(addedTracks.Where(q => q.GetNumber() <= 2).ToList());
            lastTrack.ChangePeopleOnTrack(lastTrack.GetPeopleOnTrack() + 1);
            var neededTime = skier.CalculateNeededTime(lastTrack);
            skier.SetTimeToNextStep(neededTime);
            skier.SetLeavingTime(time + neededTime);

            logger?.AppendTask("6. letzte Abfahrt");
        }

        private void HandleEndOfTrack(Skier skier) 
        {
            //4.4: 7.
            var lastTrack = skier.GetUsedTracks().Last();
            var nextLift = lastTrack.GetLift();

            if (skier.GetWaitingNumber() == -1) 
            {
                nextLift.AddQueue();
                skier.SetWaitingNumber(nextLift.GetWaitingQueue());
            }

            logger?.AppendTask($"4.4 nächsten Lift wählen Track: {lastTrack.GetNumber()} Wartenr: {skier.GetWaitingNumber()}");

            if (nextLift.CalcFlowRate() >= skier.GetWaitingNumber()) 
            {
                //4.4.1: 8.
                skier.SetStatus(0);
                skier.AddUsedLift(nextLift);
                skier.SetTimeToNextStep(nextLift.GetTravelTime());
                skier.SetWaitingNumber(-1);
                lastTrack.ChangePeopleOnTrack(lastTrack.GetPeopleOnTrack() - 1);

                logger?.AppendTask("4.4.1 Lift nehmen ");
            }
            else 
            {
                //4.4.2: 9.
                skier.SetWaitingNumber(skier.GetWaitingNumber() - nextLift.CalcFlowRate());
            }
        }

        private void PrintTracks(double time) 
        {
            Console.WriteLine("-------------Halbstündliche Ausgabe der Auslastung der Strecken-------------");
            Console.WriteLine($"Uhrzeit: {TimeSpan.FromMinutes(time):hh\\:mm}");
            foreach (var track in addedTracks)
                Console.WriteLine($"Strecke {track.GetNumber(),-2} aktuelle Auslastung: {track.CalcWorkload():P}");
            Console.WriteLine();
        }

        private void PrintSkiers() 
        {
            Console.WriteLine("---------------------------Ausgabe aller Skifaher---------------------------");
            foreach(var skier in addedSkiers) 
                Console.WriteLine($"No. {skier.GetNumber(),-4} Skill {skier.GetSkillLevel(),-10} Ankunft: {TimeSpan.FromMinutes(skier.GetArrivingTime()),-7:hh\\:mm} Abreise: {TimeSpan.FromMinutes(skier.GetLeavingTime()),-7:hh\\:mm} gefahrene km: {skier.GetUsedTracks().Sum(q => q.GetLength() / 1000.0)}");
        }

        public List<Lift> GetLifts() => status ? addedLifts : null;

        public List<Skier> GetSkiers() => status ? addedSkiers : null;

        public List<Track> GetTracks() => status ? addedTracks : null;
    }
}
