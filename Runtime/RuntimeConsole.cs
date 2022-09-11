using System.Collections.Generic;
using UnityEngine;

namespace TOMICZ.Debugger
{
    public static class RuntimeConsole
    {
        private static ConsoleWindow _consoleWindow;

        public static List<WindowElement> WindowElementList = new List<WindowElement>();

        private static RuntimeCommands _runtimeCommands;

        public static void SetupConsoleWindow(ConsoleWindow consoleWindow)
        {
            _consoleWindow = consoleWindow;
            _runtimeCommands = new RuntimeCommands();
        }

        public static void AddWindowElement(WindowElement windowElement)
        {
            WindowElementList.Add(windowElement);
        }

        public static void Log(string message)
        {
            _consoleWindow.Log(message);
        }

        public static void Header(string message)
        {
            _consoleWindow.Header(message);
        }

        public static void Error(string message)
        {
            _consoleWindow.Error(message);
        }

        public static void Loop(string message)
        {
            _consoleWindow.Loop(message);
        }

        /// <summary>
        /// Prints message to Console Window.
        /// </summary>
        /// <param name="messageType">Selects type of a message.</param>
        /// <param name="message">Print message container.</param>
        public static void PrintMessage(MessageType messageType, string message)
        {
            if (HasConsole())
            {
                WriteMessage(messageType, message);
            }
        }

        public static void PrintList(List<object> list)
        {
            _runtimeCommands.PrintList(list);
        }

        public static void PrintList(List<object> list, bool isSerialized)
        {
            _runtimeCommands.PrintList(list, isSerialized);
        }

        /// <summary>
        /// Checks if Console Window is available. If not, throws a warning to add it. 
        /// </summary>
        /// <returns></returns>
        private static bool HasConsole()
        {
            if (_consoleWindow == null)
            {
                Debug.LogWarning("Runtime Console is missing console window. Create one inside Canvas with right-click > Tomicz > New Console Window.");
                return false;
            }
            return true;
        }

        private static void WriteMessage(MessageType messageType, string message) => _consoleWindow.PrintMessage(messageType, message);
    }
}