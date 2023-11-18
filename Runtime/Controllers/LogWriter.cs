using System;
using System.IO;
using UnityEngine;

namespace TOMICZ.Debugger.Controllers
{
    public class LogWriter
    {
        public static Action<LogMessage> OnLogWrittenAction;

        private static string logsPath = Application.persistentDataPath + "/logs.txt";

        public static void Write(LogMessage logMessage)
        {
            using (StreamWriter writer = new StreamWriter(logsPath, true))
            {
                writer.WriteLine($"[{logMessage.type}] {logMessage.log}");
            }

            OnLogWrittenAction?.Invoke(logMessage);
        }
    }
}