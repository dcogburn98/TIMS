using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using Microsoft.Data.Sqlite;

namespace TIMSServer
{
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error
    }

    public static class Logger
    {
        public static void LogEOD(this string message, LogLevel logLevel)
        {
            WriteToLog("EOD", message, logLevel);
        }

        public static void LogClient(this string message, LogLevel logLevel)
        {
            WriteToLog("Client", message, logLevel);
        }

        public static void LogServer(this string message, LogLevel logLevel)
        {
            WriteToLog("Server", message, logLevel);
        }

        private static void WriteToLog(string origin, string message, LogLevel logLevel)
        {
            string logMessage = $"{DateTime.Now} [{logLevel}] [{origin}] {message}";

            Console.WriteLine(logMessage);

            using (StreamWriter writer = new StreamWriter("log.txt", true))
            {
                writer.WriteLine(logMessage);
            }
        }
    }
}
