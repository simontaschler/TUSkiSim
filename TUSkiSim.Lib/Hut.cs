using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUSkiSim.Lib
{
    public class Hut
    {
        private readonly int avgStay;
        private readonly int maxGuests;
        private readonly string name;
        private int guests;

        public Hut(string name, int maxGuests, int avgStay) 
        {
            this.name = name;
            this.maxGuests = maxGuests;
            this.avgStay = avgStay;
            guests = 0;
        }

        //Properties ersetzen Get-Methoden
        public int AverageStay => avgStay;
        public int Guests => guests;
        public int MaxGuests => maxGuests;

        public void AddGuest() =>
            guests++;
    }
}
