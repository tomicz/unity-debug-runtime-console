using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System;

namespace TOMICZ.Debugger
{
    public class LogsFileManager
    {
        private static string path = Application.persistentDataPath + "/logs.txt";

        [MenuItem("Tomicz/Debugger/Open Logs", false)]
        private static void OpenLogs()
        {
            try
            {
                using(Process process = new Process())
                {
                    process.StartInfo.FileName = path;
                    process.Start();
                }
            }
            catch (Exception exception)
            {
                UnityEngine.Debug.LogWarning($"{exception.Message}");
            }
        }
    }
}