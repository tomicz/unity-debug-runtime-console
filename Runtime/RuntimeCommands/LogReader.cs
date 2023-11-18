using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace TOMICZ.Debugger
{
    public class LogReader
    {
        private static string logsPath = Application.persistentDataPath + "/logs.txt";

        public static void ReadLine()
        {
            if (!File.Exists(logsPath))
            {
                Debug.LogError("LogReader cannot find logs.txt location.");
                return;
            }

            using(StreamReader streamReader = new StreamReader(logsPath))
            {
                string line;
                string lastLine = null;

                while ((line = streamReader.ReadLine()) != null)
                {
                    lastLine = line;
                }

                Debug.Log(lastLine);
            }
        }

        public static void ReadLines(int maxLines)
        {
            Queue<string> last20Lines = new Queue<string>(maxLines);
            string line;

            using (StreamReader streamReader = new StreamReader(logsPath))
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    last20Lines.Enqueue(line);

                    if (last20Lines.Count > 20)
                    {
                        last20Lines.Dequeue();
                    }
                }
            }

            // Now last20Lines contains the last 20 lines of the file
            Debug.Log("Last 20 lines:");

            foreach (string lastLine in last20Lines)
            {
                Debug.Log(lastLine);
            }
        }

        public static void ClearLogs()
        {
            using (StreamWriter writer = new StreamWriter(logsPath, false))
            {
                writer.Write("");
            }
        }
    }
}