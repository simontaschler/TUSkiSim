using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUSkiSim.Lib
{
    internal class Logger
    {
        private StringBuilder logBuilder;

        internal Logger() => 
            logBuilder = new StringBuilder().AppendLine("time, timeFormatted, number, status, timeToNextStep, task");

        internal void Log(int time, Skier skier, string task) 
        {
            var timeFormat = TimeSpan.FromMinutes(time).ToString(@"hh\:mm");
            logBuilder.AppendFormat("{0}, {1}, {2}, {3}, {4}, {5}", time, timeFormat, skier.GetNumber(), skier.GetStatus(), skier.GetTimeToNextStep(), task);
        }

        internal void WriteToFile(string logFile) => 
            File.WriteAllText(logFile, logBuilder.ToString());
    }
}
