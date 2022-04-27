using UnityEngine;
using TMPro;

public enum MessageType
{
    Error,
    Log
}
namespace TOMICZ.RuntimeConsole
{
    public class Console : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private TMP_Text _consoleText;

        private RectTransform _consoleRect;
        private MessageType _messageType;

        private void Awake()
        {
            SetupDependencies();
        }

        private void SetupDependencies()
        {
            _consoleRect = GetComponent<RectTransform>();
        }

        public void PrintMessage(MessageType messageType, string text)
        {
            if(_consoleText.text != null)
            {
                _consoleText.text += GetMessageType(messageType) + text + "\n";
            }
        }

        public void DragToExpandConsole() => SetRectSize(_consoleRect, new Vector2(0, Input.mousePosition.y));

        private void SetRectSize(RectTransform rect, Vector2 newSize) => rect.sizeDelta = newSize;

        private string GetMessageType(MessageType messageType)
        {
            switch (messageType)
            {
                case MessageType.Error:
                    return " * <color=red>[Error]</color> ";
                case MessageType.Log:
                    return " * <color=green>[Log]</color> ";
            }

            return "message-empty";
        }
    }
}