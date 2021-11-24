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
        private readonly StringBuilder logBuilder;
        private StringBuilder logEntryBuilder;

        internal Logger()
        {
            logBuilder = new StringBuilder("time, timeFormatted, number, status, timeToNextStep, task");
            logEntryBuilder = new StringBuilder();
        }
        internal void Log(int time, Skier skier) 
        {
            var timeFormat = TimeSpan.FromMinutes(time).ToString(@"hh\:mm");
            logBuilder.AppendLine().AppendFormat("{0}, {1}, {2}, {3}, {4},", time, timeFormat, skier.GetNumber(), skier.GetStatus(), skier.GetTimeToNextStep()).Append(logEntryBuilder);
            logEntryBuilder = new StringBuilder();
        }

        internal Logger AppendTask(string task) 
        {
            logEntryBuilder.Append(' ').Append(task);
            return this;
        }

        internal void WriteToFile(string logFile) => 
            File.WriteAllText(logFile, logBuilder.ToString());
    }
}
