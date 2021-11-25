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

        private readonly Logger logger;

        //Properties ersetzen Get-Methoden
        public List<Lift> Lifts => status ? addedLifts : null;
        public List<Skier> Skiers => status ? addedSkiers : null;
        public List<Track> Tracks => status ? addedTracks : null;

        public Simulation(List<Lift> lifts, List<Skier> skiers, /*List<Hut> huts,*/ List<Track> tracks, Logger logger = null) 
        {
            addedLifts = lifts;
            addedSkiers = skiers;
            addedTracks = tracks;
            this.logger = logger;
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
                    if (skier.TimeToNextStep == 0)
                    {
                        if (skier.Status != 2 && time >= skier.ArrivingTime)
                        {
                            if (skier.Status == -1)
                            {
                                //4.1
                                HandleBeforeFirstRun(skier);
                            }
                            else if (skier.Status == 0 && time < endTime - 90)
                            {
                                //4.2
                                HandleExitLift(skier);
                            }
                            else if (skier.Status == 0 /*&& time >= endTime - 90*/)
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
                        else if (skier.Status == 2 && time == skier.LeavingTime)
                        {
                            //3.2: 10.
                            var lastTrack = skier.UsedTracks.Last();
                            lastTrack.PeopleOnTrack--;

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

                addedLifts.ForEach(q => q.RedWaitingQueue());

                if (time % 30 == 0 && time > startTime)
                    PrintTracks(time / 60.0);
            }

            PrintSkiers();

            status = true;
        }

        #region 4.1 - 4.4
        private void HandleBeforeFirstRun(Skier skier) 
        {
            var lift1 = GetLift1();

            //4.1: 1.
            if (skier.WaitingNumber == -1) 
            {
                lift1.AddQueue();
                skier.WaitingNumber = lift1.WaitingQueue;

                logger?.AppendTask($"4.1 Wartenr: {skier.WaitingNumber}");
            }

            if (lift1.CalcFlowRate() >= skier.WaitingNumber) 
            {
                //4.1.1: 2.
                skier.Status = 0;
                skier.AddUsedLift(lift1);
                skier.TimeToNextStep = lift1.TravelTime;
                skier.WaitingNumber = 0;

                logger?.AppendTask($"4.1.1 Lift wählen: {lift1}");
            }
            else 
            {
                //4.1.2: 3.
                skier.WaitingNumber -= lift1.CalcFlowRate();

                logger?.AppendTask($"4.1.2 Wartenummer reduzieren zu: {skier.WaitingNumber}");
            }
        }

        private void HandleExitLift(Skier skier) 
        {
            //4.2: 4.
            var nextTrack = skier.CalculateNextTrack(addedTracks);
            nextTrack.PeopleOnTrack++;
            skier.Status = 1;
            skier.AddUsedTrack(nextTrack);
            skier.TimeToNextStep = skier.CalculateNeededTime(nextTrack);

            logger?.AppendTask($"4.2 nächste Strecke Track: {nextTrack.Number}");

            if (nextTrack.Hut != null)
            {
                //4.2.1
                var rnd = new Random();
                if (skier.PropbabilityHut >= rnd.NextDouble() && nextTrack.Hut.MaxGuests > nextTrack.Hut.Guests)
                {
                    //4.2.1.1: 5.
                    skier.TimeToNextStep += nextTrack.Hut.AverageStay;
                    skier.AddVisitedHut(nextTrack.Hut);
                    nextTrack.Hut.AddGuest();
                }
            }
        }

        private void HandleLastRun(Skier skier, int time) 
        {
            //4.3: 6.
            skier.Status = 2;
            var lastTrack = GetTrack1Or2(skier);
            lastTrack.PeopleOnTrack++;
            var neededTime = skier.CalculateNeededTime(lastTrack);
            skier.TimeToNextStep = neededTime;
            skier.LeavingTime = time + neededTime;

            logger?.AppendTask("6. letzte Abfahrt");
        }

        private void HandleEndOfTrack(Skier skier) 
        {
            //4.4: 7.
            var lastTrack = skier.UsedTracks.Last();

            if (skier.WaitingNumber == 0) 
            {
                lastTrack.Lift.AddQueue();
                skier.WaitingNumber = lastTrack.Lift.WaitingQueue;
            }

            logger?.AppendTask($"4.4 nächsten Lift wählen Track: {lastTrack.Number} Wartenr: {skier.WaitingNumber}");

            if (lastTrack.Lift.CalcFlowRate() >= skier.WaitingNumber) 
            {
                //4.4.1: 8.
                skier.Status = 0;
                skier.AddUsedLift(lastTrack.Lift);
                skier.TimeToNextStep = lastTrack.Lift.TravelTime;
                skier.WaitingNumber = 0;
                lastTrack.PeopleOnTrack--;

                logger?.AppendTask("4.4.1 Lift nehmen ");
            }
            else 
            {
                //4.4.2: 9.
                skier.WaitingNumber -= lastTrack.Lift.CalcFlowRate();
            }
        }
        #endregion

        #region Ausgabe
        private void PrintTracks(double time) 
        {
            Console.WriteLine("-------------Halbstündliche Ausgabe der Auslastung der Strecken-------------");
            Console.WriteLine($"Uhrzeit: {time} h");
            foreach (var track in addedTracks)
                Console.WriteLine($"Strecke {track.Number} aktuelle Auslastung: {track.CalcWorkload():P}");
            Console.WriteLine();
        }

        private void PrintSkiers() 
        {
            Console.WriteLine("---------------------------Ausgabe aller Skifaher---------------------------");
            foreach(var skier in addedSkiers) 
            {
                var km = skier.UsedTracks.Sum(q => q.Length / 1000.0);
                Console.WriteLine($"No. {skier.Number,-4} Skill {skier.SkillLevel,-2} Ankunft: {skier.ArrivingTime/60.0,-4:N2} Abreise: {skier.LeavingTime/60.0,-4:N2} gefahrene km: {km}");
            }
        }
        #endregion

        #region Helper-Methoden
        private Lift GetLift1() =>
            addedLifts.FirstOrDefault(q => q.Number == 1);

        private Track GetTrack1Or2(Skier skier) 
        {
            Track track;
            do
            {
                track = skier.CalculateNextTrack(addedTracks);
            } while (track.Number != 1 && track.Number != 2);

            return track;
        }
        #endregion
    }
}
