using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System;
using System.IO;

namespace TOMICZ.Debugger
{
    public class LogsEditorController
    {
        private static string _logsPath = Application.persistentDataPath + "/logs.txt";

        [MenuItem("Tomicz/Debugger/Open Logs", false)]
        private static void OpenLogs()
        {
            try
            {
                using(Process process = new Process())
                {
                    process.StartInfo.FileName = _logsPath;
                    process.Start();
                }
            }
            catch (Exception exception)
            {
                UnityEngine.Debug.LogWarning($"{exception.Message}");
            }
        }

        [MenuItem("Tomicz/Debugger/Clear Logs", false)]
        private static void ClearLogs()
        {
            try
            {
                if (File.Exists(_logsPath))
                {
                    File.WriteAllText(_logsPath, string.Empty);
                }
                else
                {
                    UnityEngine.Debug.LogWarning("Log file not found. No logs cleared.");
                }
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError($"Error clearing logs: {e.Message}");
            }
        }
    }
}