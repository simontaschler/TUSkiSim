using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUSkiSim.Lib
{
    public class Hut
    {
        //Auto-Properties ersetzen Get-Methoden und private Felder
        public int AverageStay { get; }
        public int MaxGuests { get; }
        public string Name { get; }
        public int Guests { get; private set; }

        public Hut(string name, int maxGuests, int avgStay) 
        {
            Name = name;
            MaxGuests = maxGuests;
            AverageStay = avgStay;
            Guests = 0;
        }

        public void AddGuest() =>
            Guests++;
    }
}
