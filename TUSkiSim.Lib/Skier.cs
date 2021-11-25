using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUSkiSim.Lib
{
    //enum Status : int
    //{ 
    //    PreArrival = -1,
    //    InLift = 0,
    //    OnTrack = 1,
    //    LeftResort = 2
    //}

    //enum Skill : int
    //{ 
    //    Beginner = 1,
    //    Advanced = 2,
    //    Expert = 3
    //}

    public abstract class Skier
    {
        protected int velocity;

        //abstrakte Property statt abstrakter Get-Methode
        public abstract double PropbabilityHut { get; }
        public abstract int SkillLevel { get; }

        #region Auto-Properties ersetzen Get- & Set-Methoden und private Felder
        public List<Lift> UsedLifts { get; }
        public List<Track> UsedTracks { get; }
        public List<Hut> VisitedHuts { get; }
        public int ArrivingTime { get; }
        public int Number { get; }

        public int Status { get; set; }
        public int TimeToNextStep { get; set; }
        public int WaitingNumber { get; set; }
        public int LeavingTime { get; set; }
        #endregion

        public abstract Track CalculateNextTrack(List<Track> tracks);

        protected Skier(int number, int arrivingTime) 
        {
            Number = number;
            ArrivingTime = arrivingTime * 60;
            UsedLifts = new List<Lift>();
            UsedTracks = new List<Track>();
            VisitedHuts = new List<Hut>();
            Status = -1;
            WaitingNumber = 0;
        }

        public virtual int CalculateNeededTime(Track track) =>
            (int)Math.Ceiling((double)track.Length / velocity);

        public void CountDownTime() =>
            TimeToNextStep -= TimeToNextStep - 1 < 0
                ? 0
                : 1;

        public void AddUsedLift(Lift lift) =>
            UsedLifts.Add(lift);

        public void AddUsedTrack(Track track) =>
            UsedTracks.Add(track);

        public void AddVisitedHut(Hut hut) =>
            VisitedHuts.Add(hut);

        public override string ToString() =>
            $"Skier No. {Number}: {SkillLevelToString()}, arrived at {ArrivingTime}, left at {LeavingTime}";

        private string SkillLevelToString() 
        {
            switch (SkillLevel) 
            {
                case 1: return "Beginner";
                case 2: return "Advanced";
                case 3: return "Expert";
                default: return "N/A";
            }
        }
    }
}
