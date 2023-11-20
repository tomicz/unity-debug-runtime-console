using TOMICZ.Debugger.Views;
using UnityEngine;

namespace TOMICZ.Debugger.Controllers
{
    public class ConsoleDependencyInstallerController : MonoBehaviour
    {
        private ConsoleWindow _consoleWindow;
        private LogController _logController;

        private void Awake()
        {
            InjectDependencies();
        }

        private void InjectDependencies()
        {
            _consoleWindow = Resources.Load<ConsoleWindow>("ConsoleWindow");
            _logController = new LogController(_consoleWindow);
        }
    }
}