using UnityEngine;

namespace TOMICZ.Debugger.Controllers
{
    public class LogController
    {
        private ConsoleWindow _consoleWindow;

        public LogController(ConsoleWindow consoleWindow)
        {
            _consoleWindow = consoleWindow;

            CreateConsoleWindow(consoleWindow);

            LogWriter.OnLogWrittenAction += TickMessage;
        }

        private void CreateConsoleWindow(ConsoleWindow consoleWindow)
        {
            _consoleWindow = GameObject.Instantiate(consoleWindow);
        }

        private void TickMessage(LogMessage logMessage)
        {
            _consoleWindow.UpdateLog(logMessage.log.ToString());
        }
    }
}