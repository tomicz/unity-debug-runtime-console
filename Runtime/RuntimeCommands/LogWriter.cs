using System.IO;
using UnityEngine;

namespace TOMICZ.Debugger
{
    public class LogWriter
    {
        private static string logsPath = Application.persistentDataPath + "/logs.txt";

        public static void Write(LogMessage logMessage)
        {
            using (StreamWriter writer = new StreamWriter(logsPath, true))
            {
                writer.WriteLine($"[{logMessage.type}] {logMessage.log}");
            }
        }
    }
}