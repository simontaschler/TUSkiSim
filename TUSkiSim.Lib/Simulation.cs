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

            var dummy = addedSkiers.Single(q => q.GetNumber() == 85);

            for (var time = startTime; time <= endTime; time++) 
            {
                if (time == 540) 
                { }

                foreach (var skier in addedSkiers) 
                {
                    if (skier.GetTimeToNextStep() == 0 && skier.GetStatus() != 2 && time >= skier.GetArrivingTime())
                    {
                        if (skier == dummy) 
                        { }

                        if (skier.GetStatus() == -1)
                        {
                            HandleBeforeFirstRun(skier);
                        }
                        else if (skier.GetStatus() == 0)
                        {
                            HandleExitsLift(skier);
                        }
                        else if (skier.GetStatus() == 1 && time > endTime - 90)
                        {
                            HandleLastRun(skier, time);
                        }
                        else 
                        {
                            if (skier.GetStatus() == 1 && skier.GetWaitingNumber() > 0)
                            { }

                            if (skier.CalculateNextTrack(addedTracks).GetPeopleOnTrack() != addedSkiers.Count(q => q.GetStatus() == 1)) 
                            { }

                            HandleEndOfTrack(skier);
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

        private void HandleBeforeFirstRun(Skier skier) 
        {
            var lift1 = GetLift1();

            if (skier.GetWaitingNumber() == -1) //1. Warteschlange aktualisieren, WaitingNumber nur Reset -1
            {
                lift1.AddQueue();
                skier.SetWaitingNumber(lift1.GetWaitingQueue());
            }

            TryEnterLift(skier, lift1);
        }

        private void HandleExitsLift(Skier skier) 
        {
            var nextTrack = skier.CalculateNextTrack(addedTracks); //nächste Strecke auswählen
            nextTrack.ChangePeopleOnTrack(nextTrack.GetPeopleOnTrack() + 1); //Skifahrer auf Strecke um 1 erhöhen
            skier.SetStatus(1); //auf Piste
            skier.AddUsedTrack(nextTrack);
            skier.SetTimeToNextStep(skier.CalculateNeededTime(nextTrack));

            //if (nextTrack.GetHut() != null)
            //{
            //    var rnd = new Random();
            //    var hut = nextTrack.GetHut();
            //    if (skier.GetPropbabilityHut() >= rnd.NextDouble() && hut.GetMaxGuests() > hut.GetGuests())
            //    {
            //        skier.SetTimeToNextStep(skier.GetTimeToNextStep() + hut.GetAverageStay());
            //        skier.AddVisitedHut(hut);
            //        hut.AddGuests(1);
            //    }
            //}
        }

        private void HandleLastRun(Skier skier, int time) 
        {
            skier.SetStatus(2); //hat Skigebiet verlassen
            var lastTrack = GetTrack1Or2(skier);
            skier.SetLeavingTime(time + skier.CalculateNeededTime(lastTrack));
        }

        private void HandleEndOfTrack(Skier skier) 
        {
            var lastTrack = skier.GetUsedTracks().Last();
            var nextLift = lastTrack.GetLift(); //nächster Lift ist der der letzten Piste

            if (skier.GetWaitingNumber() == -1) 
            {
                lastTrack.ChangePeopleOnTrack(lastTrack.GetPeopleOnTrack() - 1); //Skifahrer auf Piste um 1 reduzieren
                nextLift.AddQueue();
                skier.SetWaitingNumber(nextLift.GetWaitingQueue());
            }

            TryEnterLift(skier, nextLift);
        }

        private void TryEnterLift(Skier skier, Lift lift) 
        {
            if (lift.CalcFlowRate() >= skier.GetWaitingNumber()) //2. Lift wählen und Status setzen
            {
                skier.SetStatus(0); // im Lift
                skier.AddUsedLift(lift);
                skier.SetTimeToNextStep(lift.GetTravelTime()); //nächster Step wenn Lift oben angekommen
                skier.SetWaitingNumber(-1);  //WaitingNumber zurücksetzen
            }
            else    //3. Wartenummer reduzieren 
            {
                skier.SetWaitingNumber(skier.GetWaitingNumber() - lift.CalcFlowRate());
            }
        }

        private void PrintTracks(double time) 
        {
            Console.WriteLine("-------------Halbstündliche Ausgabe der Auslastung der Strecken-------------");
            Console.WriteLine($"Uhrzeit: {time} h");
            foreach (var track in addedTracks)
                Console.WriteLine($"Strecke {track.GetNumber()} aktuelle Auslastung: {track.CalcWorkload():P}");
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

                Console.WriteLine($"No. {skier.GetNumber(),-4} Skill {skier.GetSkillLevel(),-2} Ankunft: {skier.GetArrivingTime()/60.0,-4:N2} Abreise: {skier.GetLeavingTime()/60.0,-4:N2} gefahrene km: {km}");
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

        private Track GetTrack1Or2(Skier skier) 
        {
            Track track = null;
            do
            {
                track = skier.CalculateNextTrack(addedTracks);
            } while (track.GetNumber() != 1 && track.GetNumber() != 2);

            return track;
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
