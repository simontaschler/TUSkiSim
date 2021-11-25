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

        //abstrakte Property statt abstrakter Get-Methode
        public abstract double PropbabilityHut { get; }

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

        //Properties ersetzen Get-Methoden
        public List<Lift> UsedLifts => usedLifts;
        public List<Track> UsedTracks => usedTracks;
        public List<Hut> VisitedHuts => visitedHuts;
        public int ArrivingTime => arrivingTime;
        public int Number => number;
        public int SkillLevel => skillLevel;

        #region Properties ersetzen Get- und Set-Methoden (durch Auto-Property zu ersetzen)
        public int Status
        {
            get => status;
            set => status = value;
        }

        public int TimeToNextStep
        {
            get => timeToNextStep;
            set => timeToNextStep = value;
        }

        public int WaitingNumber
        {
            get => waitingNumber;
            set => waitingNumber = value;
        }

        public int LeavingTime
        {
            get => leavingTime;
            set => leavingTime = value;
        }
        #endregion

        public virtual int CalculateNeededTime(Track track) =>
            (int)Math.Ceiling((double)track.Length / velocity);

        public void CountDownTime() =>
            timeToNextStep -= timeToNextStep - 1 < 0
                ? 0
                : 1;

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
