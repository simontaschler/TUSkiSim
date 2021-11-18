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

        //Properties ersetzen Get-Methoden
        public int AverageStay => avgStay;
        public int Guests => guests;
        public int MaxGuests => maxGuests;

        public Hut(string name, int maxGuests, int avgStay) 
        {
            this.name = name;
            this.maxGuests = maxGuests;
            this.avgStay = avgStay;
            guests = 0;
        }

        public void AddGuests(int numGuests = 1) =>
            guests += numGuests;
    }
}
