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

        //Properties ersetzen Get-Methoden
        public List<Lift> UsedLifts => usedLifts;
        public List<Track> UsedTracks => usedTracks;
        public List<Hut> VisitedHuts => visitedHuts;
        public int ArrivingTime => arrivingTime;
        public int Number => number;

        //abstrakte Property statt abstrakter Get-Methode
        public abstract double Propbabilityhut { get; }

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

        protected Skier(int number, int arrivingTime) 
        {
            this.number = number;
            this.arrivingTime = arrivingTime;
            usedLifts = new List<Lift>();
            usedTracks = new List<Track>();
            visitedHuts = new List<Hut>();
        }

        public abstract Track CalculateNextTrack(List<Track> tracks);

        public virtual int CalculateNeededTime(Track track) =>
            track.Length / velocity;

        public void CountDownTime() =>
            TimeToNextStep -= TimeToNextStep - 1 < 0
                ? 0
                : 1;

        public void AddUsedLift(Lift lift) =>
            usedLifts.Add(lift);

        public void AddUsedTrack(Track track) =>
            usedTracks.Add(track);

        public void AddVisitedHut(Hut hut) =>
            visitedHuts.Add(hut);

        public override string ToString() =>
            $"Skier No. {number}: {MapSkillLevelToString()}, arrived at {ArrivingTime}, left at {LeavingTime}";

        private string MapSkillLevelToString() 
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
