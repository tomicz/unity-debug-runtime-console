using System;
using System.Collections.Generic;
using UnityEngine;

namespace TOMICZ.Debugger
{
    public static class RuntimeConsole
    {
        public static Action OnUnityRunEvent;

        public static List<WindowElement> WindowElementList = new List<WindowElement>();

        private static RuntimeCommands _runtimeCommands;
        private static ConsoleWindow _consoleWindow;
        private static LogWriter _logWriter;

        private static List<ITick> _tickables = new List<ITick>();

        public static void Initilise(ConsoleWindow consoleWindow)
        {
            _consoleWindow = consoleWindow;
            _runtimeCommands = new RuntimeCommands();
            _logWriter = new LogWriter();
            _tickables.Add(_consoleWindow);
            _tickables.Add(_logWriter);
            _logWriter.ClearLogs();
        }

        public static void AddWindowElement(WindowElement windowElement) => WindowElementList.Add(windowElement);

        public static void Log(string message)
        {
            _logWriter.Write(new LogMessage(LogMessageType.Log, message));
            Tick();
        }

        public static void Header(string message)
        {
            _logWriter.Write(new LogMessage(LogMessageType.Header, message));
            Tick();
        }

        public static void Error(string message)
        {
            _logWriter.Write(new LogMessage(LogMessageType.Error, message));
            Tick();
        }

        public static void Loop(string message)
        {
            _logWriter.Write(new LogMessage(LogMessageType.Loop, message));
            Tick();
        }

        public static void PrintMessage(LogMessageType messageType, string message)
        {
            if (HasConsole())
            {
                WriteMessage(messageType, message);
            }
        }

        public static void PrintList(List<object> list) => _runtimeCommands.PrintList(list);

        public static void PrintList(List<object> list, bool isSerialized) => _runtimeCommands.PrintList(list, isSerialized);

        public static void RegisterEvent(Action action) => OnUnityRunEvent += action;

        public static void RemoveEvent(Action action) => OnUnityRunEvent -= action;

        private static bool HasConsole()
        {
            if (_consoleWindow == null)
            {
                Debug.LogWarning("Runtime Console is missing console window. Create one inside Canvas with right-click > Tomicz > New Console Window.");
                return false;
            }
            return true;
        }

        private static void WriteMessage(LogMessageType messageType, string message) => _consoleWindow.PrintMessage(new LogMessage(messageType, message));

        private static void Tick()
        {
            foreach (var tick in _tickables)
            {
                tick.Tick();
            }
        }
    }
}