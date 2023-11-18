using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TOMICZ.Debugger
{
    public class LogWriter
    {
        public static Stack<LogMessage> Messages = new Stack<LogMessage>();

        public string path = Application.persistentDataPath + "/logs.txt";

        public void Write(LogMessage logMessage)
        {
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine($"[{logMessage.type}] {logMessage.message}");
            }

            Messages.Push(logMessage);
        }

        public void ClearLogs()
        {
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.Write("");
            }
        }
    }
}