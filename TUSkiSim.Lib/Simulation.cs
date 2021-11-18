using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUSkiSim.Lib
{
    public class Simulation
    {
        private List<Lift> addedLifts;
        private List<Skier> addedSkiers;
        private List<Track> addedTracks;
        private bool status;

        public Simulation(List<Lift> lifts, List<Skier> skiers, /*List<Hut> huts,*/ List<Track> tracks) 
        {
            addedLifts = lifts;
            addedSkiers = skiers;
            addedTracks = tracks;
            status = false;
        }

        public void Simulate(int startTime, int endTime) 
        {
            startTime *= 60;
            endTime *= 60;
            var lift1 = GetLift1();

            for (var time = startTime; time <= endTime; time++) 
            { 
                foreach (var skier in addedSkiers) 
                {
                    if (skier.GetTimeToNextStep() == 0 && skier.GetStatus() != 2)
                    {
                        if (skier.GetStatus() == -1 && time > skier.GetArrivingTime())
                        {
                            if (skier.GetWaitingNumber() == -1) //Skier wird mit WaitingNumber = -1 initialisiert, danach nur noch Werte >= 0
                            {
                                lift1.AddQueue();
                                skier.SetWaitingNumber(lift1.GetWaitingQueue());
                            }

                            if (lift1.CalcFlowRate() > skier.GetWaitingNumber())
                            {
                                skier.SetStatus(0);
                                skier.AddUsedLift(lift1);
                                skier.SetTimeToNextStep(lift1.GetTravelTime());
                                skier.SetWaitingNumber(0);
                            }
                            else 
                            {
                                skier.SetWaitingNumber(skier.GetWaitingNumber() - lift1.CalcFlowRate());
                            }
                        }
                        else if (skier.GetStatus() == 0)
                        {
                            var nextTrack = skier.CalculateNextTrack(addedTracks);
                            nextTrack.ChangePeopleOnTrack(nextTrack.GetPeopleOnTrack() + 1);
                            skier.SetStatus(1);
                            skier.AddUsedTrack(nextTrack);
                            skier.SetTimeToNextStep(skier.CalculateNeededTime(nextTrack));

                            if (nextTrack.GetHut() != null) 
                            {
                                var rnd = new Random();
                                var hut = nextTrack.GetHut();
                                if (skier.GetPropbabilityHut() >= rnd.NextDouble() && hut.GetMaxGuests() > hut.GetGuests()) 
                                {
                                    skier.SetTimeToNextStep(skier.GetTimeToNextStep() + hut.GetAverageStay());
                                    skier.AddVisitedHut(hut);
                                    hut.AddGuests(1);
                                }
                            }
                        }
                        else if (skier.GetStatus() == 1 && time > endTime - 90)
                        {
                            skier.SetStatus(2);

                            Track lastTrack = null;
                            do
                            {
                                lastTrack = skier.CalculateNextTrack(addedTracks);
                            } while (lastTrack.GetNumber() != 1 && lastTrack.GetNumber() != 2);

                            skier.SetLeavingTime(time + skier.CalculateNeededTime(lastTrack));
                        }
                        else 
                        {
                            var lastTrack = skier.GetUsedTracks().LastOrDefault();
                            var nextLift = lastTrack.GetLift();

                            lastTrack.ChangePeopleOnTrack(lastTrack.GetPeopleOnTrack());

                            nextLift.AddQueue();
                            skier.SetWaitingNumber(nextLift.GetWaitingQueue());
                            skier.SetStatus(0); //Status 0 für in Lift oder in Wartebereich von Lift, wird in Erklärungder Methode aber ins if gezogen

                            if (nextLift.CalcFlowRate() > skier.GetWaitingNumber())
                            {
                                skier.AddUsedLift(nextLift);
                                skier.SetTimeToNextStep(nextLift.GetTravelTime());
                                skier.SetWaitingNumber(0);
                            }
                            else
                            {
                                skier.SetWaitingNumber(skier.GetWaitingNumber() - nextLift.CalcFlowRate());
                            }
                        }
                    }
                    else 
                    {
                        skier.CountDownTime();
                    }
                }

                foreach (var lift in addedLifts)
                    lift.RedWaitingQueue();

                if (time % 30 == 0)
                    PrintTracks(time / 60.0);
            }

            PrintSkiers();
            status = true;
        }

        private void PrintTracks(double time) 
        {
            Console.WriteLine("-------------Halbstündliche Ausgabe der Auslastung der Strecken-------------");
            Console.WriteLine($"Uhrzeit: {time} h");
            foreach (var track in addedTracks)
                Console.WriteLine($"Strecke {track.GetNumber()} aktuelle Auslastung: {track.CalcWorkload():N0} %");
            Console.WriteLine();
        }

        private void PrintSkiers() 
        {
            Console.WriteLine("---------------------------Ausgabe aller Skifaher---------------------------");
            foreach(var skier in addedSkiers) 
            {
                var km = 0.0;
                foreach (var track in skier.GetUsedTracks())
                    km += track.GetLength() / 1000.0;

                Console.WriteLine($"No. {skier.GetNumber(),-4} Skill {skier.GetSkillLevel(),-2} Ankunft: {skier.GetArrivingTime()/60.0,-4} Abreise: {skier.GetLeavingTime()/60.0,-4} gefahrene km: {km}");
            }
        }

        private Lift GetLift1() 
        {
            foreach (var lift in addedLifts)
            {
                if (lift.GetNumber() == 1)
                    return lift;
            }
            return null;
        }

        public List<Lift> GetLifts() 
        {
            if (status)
                return addedLifts;
            return null;
        }

        public List<Skier> GetSkiers() 
        {
            if (status)
                return addedSkiers;
            return null;
        }

        public List<Track> GetTracks() 
        {
            if (status)
                return addedTracks;
            return null;
        }
    }
}
