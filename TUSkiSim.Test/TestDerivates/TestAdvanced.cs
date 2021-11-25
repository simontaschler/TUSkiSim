using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUSkiSim.Lib;

namespace TUSkiSim.Test.TestDerivates
{
    internal class TestAdvanced : Advanced
    {
        private readonly Track track1;

        public override double PropbabilityHut =>
            0;

        public TestAdvanced(int number, int arrivingTime, Track track1) : base(number, arrivingTime) =>
            this.track1 = track1;

        public override Track CalculateNextTrack(List<Track> tracks) =>
            track1;
    }
}
