using System;
using UnityEngine;

namespace TOMICZ.Debugger
{
    public class Header : MonoBehaviour
    {
        public Action<bool> OnConsoleCollapsedEvent;

        [SerializeField] RectTransform _rectTransform;

        private bool _isConsoleWindowCollapsed = true;

        public void CollapseConsole() => OnConsoleCollapsedEvent?.Invoke(_isConsoleWindowCollapsed = !_isConsoleWindowCollapsed);
    }
}