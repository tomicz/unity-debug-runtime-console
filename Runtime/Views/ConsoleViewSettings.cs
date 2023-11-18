using UnityEngine;

namespace TOMICZ.Debugger.Views
{
    [CreateAssetMenu(fileName = "New console view settings", menuName = "Tomicz/Runtime Console/New Console Settings")]
    public class ConsoleViewSettings : ScriptableObject
    {
        public Color LogColor => _logColor;
        public Color WarrningColor => _warrningColor;
        public Color ErrorCoor => _errorColor;

        [SerializeField] private Color _logColor;
        [SerializeField] private Color _warrningColor;
        [SerializeField] private Color _errorColor;
    }
}