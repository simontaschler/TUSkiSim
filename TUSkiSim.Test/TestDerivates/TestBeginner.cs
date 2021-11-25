using System.Collections.Generic;
using TUSkiSim.Lib;

namespace TUSkiSim.Test.TestDerivates
{
    internal class TestBeginner : Beginner
    {
        private readonly Track track1;

        public override double PropbabilityHut =>
            0;

        public TestBeginner(int number, int arrivingTime, Track track1) : base(number, arrivingTime) =>
            this.track1 = track1;

        public override Track CalculateNextTrack(List<Track> tracks) =>
            track1;
    }
}
