using System.IO;
using UnityEngine;

namespace TOMICZ.Debugger
{
    public static class LogWriter
    {
        private static string path = Application.persistentDataPath + "/logs.txt";

        /// <summary>
        /// Logs a message to a persistent data path on your device.
        /// </summary>
        /// <param name="message"></param>
        public static void LogMessage(string message)
        {
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message);
            }
        }

        /// <summary>
        /// Clears existing logs.
        /// </summary>
        public static void ClearLogs()
        {
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.Write("");
            }
        }
    }
}