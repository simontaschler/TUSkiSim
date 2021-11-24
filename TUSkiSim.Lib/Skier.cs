using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUSkiSim.Lib
{
    public abstract class Skier
    {
        private readonly int arrivingTime;
        private int leavingTime;
        private readonly int number;
        protected int skillLevel;   //1,2,3 => mit enum zu ersetzen, außerdem überflüssig weil SkillLevel durch erbende Klassen schon definiert
        private int status;         //-1,0,1,2 => mit enum zu ersetzen
        private int timeToNextStep;
        private readonly List<Lift> usedLifts;
        private readonly List<Track> usedTracks;
        protected int velocity;
        protected List<Hut> visitedHuts;
        private int waitingNumber;

        public abstract double GetPropbabilityHut();
        public abstract Track CalculateNextTrack(List<Track> tracks);

        protected Skier(int number, int arrivingTime) 
        {
            this.number = number;
            this.arrivingTime = arrivingTime * 60;
            usedLifts = new List<Lift>();
            usedTracks = new List<Track>();
            visitedHuts = new List<Hut>();
            status = -1;
            waitingNumber = -1; //Wert für in keiner Warteschlange
        }

        public int GetSkillLevel() =>
            skillLevel;

        public List<Lift> GetUsedLifts() =>
            usedLifts;

        public List<Track> GetUsedTracks() =>
            usedTracks;

        public List<Hut> GetVisitedHuts() =>
            visitedHuts;

        public int GetArrivingTime() =>
            arrivingTime;

        public int GetNumber() =>
            number;

        public int GetStatus() =>
            status;

        public void SetStatus(int value) =>
            status = value;

        public int GetTimeToNextStep() =>
            timeToNextStep;

        public void SetTimeToNextStep(int value) =>
            timeToNextStep = value;

        public int GetWaitingNumber() =>
            waitingNumber;

        public void SetWaitingNumber(int value) =>
            waitingNumber = value;

        public int GetLeavingTime() =>
            leavingTime;

        public void SetLeavingTime(int value) =>
            leavingTime = value;

        public virtual int CalculateNeededTime(Track track) =>
            track.GetLength() / velocity;

        public void CountDownTime() 
        {
            if (timeToNextStep >= 1)
                timeToNextStep--;
        }

        public void AddUsedLift(Lift lift) =>
            usedLifts.Add(lift);

        public void AddUsedTrack(Track track) =>
            usedTracks.Add(track);

        public void AddVisitedHut(Hut hut) =>
            visitedHuts.Add(hut);

        public override string ToString() =>
            $"Skier No. {number}: {SkillLevelToString()}, arrived at {arrivingTime}, left at {leavingTime}";

        private string SkillLevelToString() 
        {
            switch (skillLevel) 
            {
                case 1: return "Beginner";
                case 2: return "Advanced";
                case 3: return "Expert";
                default: return "N/A";
            }
        }
    }
}
