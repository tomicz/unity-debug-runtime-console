using UnityEngine;
using UnityEditor;
using System.Diagnostics;

namespace TOMICZ.Debugger
{
    public class LogsFileManager
    {
        private static string path = Application.persistentDataPath + "/logs.txt";

        [MenuItem("Tomicz/Debugger/Open Logs", false)]
        private static void OpenLogs()
        {
            Process.Start(path);
        }
    }
}