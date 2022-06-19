using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TOMICZ
{
    public class ConsoleWindow : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private TMP_Text _consoleText;

        private RectTransform _consoleRect;
        private bool _isConsoleTransparent = false;

        private void Awake()
        {
            SetupDependencies();
        }

        private void SetupDependencies()
        {
            RuntimeConsole.SetupConsoleWindow(this);

            _consoleRect = GetComponent<RectTransform>();
        }

        public void PrintMessage(MessageType messageType, string text)
        {
            if (_consoleText.text != null)
            {
                _consoleText.text += GetMessageType(messageType) + text + "\n";
            }
        }

        public void DragToExpandConsole() => SetRectSize(_consoleRect, new Vector2(0, Input.mousePosition.y));

        public void SetUIElementTransparent(Image image)
        {
            if (!_isConsoleTransparent)
            {
                SetImageAlpha(image, 0);
                _isConsoleTransparent = true;
            }
            else
            {
                SetImageAlpha(image, 1);
                _isConsoleTransparent = false;
            }
        }

        private void SetRectSize(RectTransform rect, Vector2 newSize) => rect.sizeDelta = newSize;

        private string GetMessageType(MessageType messageType)
        {
            switch (messageType)
            {
                case MessageType.Error:
                    return " * <color=red>[Error]</color> ";
                case MessageType.Log:
                    return " * <color=white>[Log]</color> ";
                case MessageType.Null:
                    return " * ";
            }

            return "message-empty";
        }

        private void SetImageAlpha(Image image, float alphaAmount)
        {
            Color tempColor = image.color;
            tempColor.a = alphaAmount;
            image.color = tempColor;
        }
    }
}