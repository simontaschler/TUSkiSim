using System;
using System.Collections.Generic;
using System.Linq;

namespace TUSkiSim.Lib
{
    public enum Status : int
    {
        PreArrival = -1,
        InLift = 0,
        OnTrack = 1,
        LastRun = 2
    }

    public enum Skill : int
    {
        Beginner = 1,
        Advanced = 2,
        Expert = 3
    }

    public abstract class Skier
    {
        protected int velocity;

        //abstrakte Property statt abstrakter Get-Methode
        public abstract double PropbabilityHut { get; }
        public abstract Skill SkillLevel { get; }

        #region Auto-Properties ersetzen Get- & Set-Methoden und private Felder
        public List<Lift> UsedLifts { get; }
        public List<Track> UsedTracks { get; }
        public List<Hut> VisitedHuts { get; }
        public int ArrivingTime { get; }
        public int Number { get; }

        public Status Status { get; set; }
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
            Status = Status.PreArrival;
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
            $"No. {Number,-4} Skill {SkillLevel,-9} Ankunft: {ArrivingTime / 60.0,-6:N2} Abreise: {LeavingTime / 60.0,-6:N2} gefahrene km: {UsedTracks.Sum(q => q.Length / 1000.0)}";
    }
}
