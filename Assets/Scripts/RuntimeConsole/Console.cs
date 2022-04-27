using UnityEngine;

namespace TOMICZ.RuntimeConsole
{
    public class Console : MonoBehaviour
    {
        private RectTransform _consoleRect;

        private void Awake()
        {
            SetupDependencies();
        }

        private void SetupDependencies()
        {
            _consoleRect = GetComponent<RectTransform>();
        }

        public void DragToExpandConsole()
        {
            SetRectSize(_consoleRect, new Vector2(0, Input.mousePosition.y));
            Debug.Log("Mouse position in UI: " + Input.mousePosition.y);
        }

        private void SetRectSize(RectTransform rect, Vector2 newSize) => rect.sizeDelta = newSize;
    }
}