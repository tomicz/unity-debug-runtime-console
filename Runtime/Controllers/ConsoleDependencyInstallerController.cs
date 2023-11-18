using TOMICZ.Debugger.Views;
using UnityEngine;

namespace TOMICZ.Debugger.Controllers
{
    public class ConsoleDependencyInstallerController : MonoBehaviour
    {
        private ConsoleWindow _consoleWindow;
        private LogController _logController;
        private WaitForSeconds _tickRate = new WaitForSeconds(1f);

        private void Awake()
        {
            InjectDependencies();
        }

        private void InjectDependencies()
        {
            LogReader.ClearLogs();

            _consoleWindow = Resources.Load<ConsoleWindow>("ConsoleWindow");
            _logController = new LogController(_consoleWindow);
        }
    }
}