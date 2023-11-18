using System;

namespace TOMICZ.Debugger
{
    public class RuntimeConsole
    {
        public static Action OnUnityRunEvent;

        public static void Log(object log)
        {

        }

        public static void Warning(object log)
        {

        }

        public static void Error(object log)
        {

        }

        public static void Loop(string message)
        {

        }

        public static void RegisterEvent(Action action) => OnUnityRunEvent += action;

        public static void RemoveEvent(Action action) => OnUnityRunEvent -= action;
    }
}