using System;
using System.Threading.Tasks;

namespace SmisBot.Logging
{
    public enum LogType
    {
        Error,
        Log,
        Warning,
        Command
    }

    public class Log
    {
        //Not beautifull but works and easy..
        public static Task WriteLog(string log, LogType logType = LogType.Log)
        {
            switch(logType)
            {
                case LogType.Log:
                    Console.WriteLine($"[{getTime()} - Log] - {log}.");
                    break;
                case LogType.Command:
                    Console.WriteLine($"[{getTime()} - Command] - {log}.");
                    break;
                case LogType.Error:
                    Console.WriteLine($"[{getTime()} - Error] - {log}.");
                    break;
                case LogType.Warning:
                    Console.WriteLine($"[{getTime()} - Warning] - {log}.");
                    break;
            }

            return Task.CompletedTask;
        }

        private static string getTime()
        {
            return DateTime.Now.ToString("hh:mm:ss");
        }
    }


}
