using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private readonly int arrivingTime;
        private int leavingTime;
        private readonly int number;
        protected Skill skillLevel;
        private Status status;
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
            status = Status.PreArrival;
            waitingNumber = -1; //Wert für in keiner Warteschlange
        }

        public Skill GetSkillLevel() =>
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

        public Status GetStatus() =>
            status;

        public void SetStatus(Status value) =>
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
            (int)Math.Ceiling((double)track.GetLength() / velocity);

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
            $"Skier No. {number}: {nameof(status)}, arrived at {arrivingTime}, left at {leavingTime}";
    }
}
