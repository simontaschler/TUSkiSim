using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUSkiSim.Lib
{
    public class Hut
    {
        private int avgStay;
        private int guests;
        private int maxGuests;
        private string name;

        public Hut(string name, int maxGuests, int avgStay) 
        {
            this.name = name;
            this.maxGuests = maxGuests;
            this.avgStay = avgStay;
            guests = 0;
        }

        public int GetAverageStay() =>
            avgStay;

        public int GetGuests() =>
            guests;

        public int GetMaxGuests() =>
            maxGuests;

        public void AddGuests(int numGuests = 1) =>
            guests += numGuests;
    }
}
