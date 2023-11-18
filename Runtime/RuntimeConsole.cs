using System;
using TOMICZ.Debugger.Controllers;
using TOMICZ.Debugger.Data;

namespace TOMICZ.Debugger
{
    public class RuntimeConsole
    {
        public static Action OnUnityRunEvent;

        public static void Log(object log)
        {
            LogWriter.Write(new LogMessage(LogMessageType.Log, log));
        }

        public static void Warning(object log)
        {
            LogWriter.Write(new LogMessage(LogMessageType.Warrning, log));
        }

        public static void Error(object log)
        {
            LogWriter.Write(new LogMessage(LogMessageType.Error, log));
        }

        public static void RegisterEvent(Action action) => OnUnityRunEvent += action;

        public static void RemoveEvent(Action action) => OnUnityRunEvent -= action;
    }
}