using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUSkiSim.Lib
{
    public class Logger
    {
        private readonly StringBuilder logBuilder;
        private StringBuilder logEntryBuilder;

        //nur für UnitTest
        public List<string> Lines { get; }

        public Logger(List<string> linesList = null)
        {
            logBuilder = new StringBuilder("time, timeFormatted, number, status, timeToNextStep, task");
            logEntryBuilder = new StringBuilder();
            Lines = linesList;
        }

        internal void Log(int time, Skier skier) 
        {
            var timeFormat = TimeSpan.FromMinutes(time).ToString(@"hh\:mm");
            logBuilder.AppendLine().AppendFormat("{0}, {1}, {2}, {3}, {4},", time, timeFormat, skier.Number, (int)skier.Status, skier.TimeToNextStep).Append(logEntryBuilder);
            Lines?.Add($"{time}, {timeFormat}, {skier.Number}, {(int)skier.Status}, {skier.TimeToNextStep},{logEntryBuilder}");
            logEntryBuilder = new StringBuilder();
        }

        internal Logger AppendTask(string task) 
        {
            logEntryBuilder.Append(' ').Append(task);
            return this;
        }

        public void WriteToFile(string logFile) => 
            File.WriteAllText(logFile, logBuilder.ToString());
    }
}
