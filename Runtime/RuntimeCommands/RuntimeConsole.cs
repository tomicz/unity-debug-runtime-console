using System;
using System.Collections.Generic;
using TOMICZ.Debugger.DebugVisualisers;
using UnityEngine;

namespace TOMICZ.Debugger
{
    public class RuntimeConsole
    {
        public static Action OnUnityRunEvent;

        public static List<WindowElement> WindowElementList = new List<WindowElement>();
        public static Stack<DebugLineRenderer> DebugLineRenderers => _debugLineRenderes;

        private static RuntimeCommands _runtimeCommands;
        private static ConsoleWindow _consoleWindow;
        private static LogWriter _logWriter;

        private static List<ITick> _tickables = new List<ITick>();
        private static Stack<DebugLineRenderer> _debugLineRenderes = new Stack<DebugLineRenderer>();

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
            _consoleWindow.Loop(message);
            Tick();
        }

        /// <summary>
        /// Draw a debug line from pointA to pointB.
        /// Make sure you call this method only once a frame (Awake(), Start(), OnEnable(), etc). Do not call it in an Update(), unless it is what you want.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="lineWidth"></param>
        /// <param name="color"></param>
        public static void DrawLine(Transform start, Transform end, float lineWidth, Color color)
        {
            DebugLineRenderer debugLineRenderer = new DebugLineRenderer(start, end, lineWidth, color);
            _debugLineRenderes.Push(debugLineRenderer);
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