using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TOMICZ.Debugger
{
    public enum MessageType
    {
        Error,
        Log,
        Loop,
        Header
    }

    public class ConsoleWindow : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private TMP_Text _consoleText;
        [SerializeField] private TMP_Text _loopText;
        [SerializeField] private Image[] _raycastImages;

        private RectTransform _consoleRect;
        private ConsoleWindowProperties _consoleWindowProperties;
        private ScrollRect _scrollRect;
        private bool _isConsoleTransparent = false;
        private bool _isRaycastingEnabled = true;

        private void Awake()
        {
            SetupDependencies();
        }

        private void SetupDependencies()
        {
            RuntimeConsole.SetupConsoleWindow(this);

            _consoleRect = GetComponent<RectTransform>();
            _consoleWindowProperties = new ConsoleWindowProperties();
            _scrollRect = GetComponentInChildren<ScrollRect>();

            _isConsoleTransparent = _consoleWindowProperties.GetTransperancyState();
            _isRaycastingEnabled = _consoleWindowProperties.GetClickThroughState();

            SetUIElementsTransparent();
            EnableClickThrough();
            SetRectSize(_consoleRect, new Vector2(_consoleRect.sizeDelta.x, _consoleWindowProperties.GetWindowHeight()));
        }

        public void PrintMessage(MessageType messageType, string message)
        {
            if (_consoleText.text != null)
            {
                switch (messageType)
                {
                    case MessageType.Error:
                        _consoleText.text += GetMessageType(MessageType.Error) + message + "\n";
                        UpdateScrollOnNewInput();
                        break;
                    case MessageType.Log:
                        _consoleText.text += GetMessageType(MessageType.Log) + message + "\n";
                        UpdateScrollOnNewInput();
                        break;
                    case MessageType.Loop:
                        _loopText.text = GetMessageType(MessageType.Loop) + message;
                        break;
                    case MessageType.Header:
                        _consoleText.text += GetMessageType(MessageType.Header) + message + "\n";
                        UpdateScrollOnNewInput();
                        break;
                }
            }
        }

        public void DragToExpandConsole()
        {
            SetRectSize(_consoleRect, new Vector2(0, _consoleRect.position.y - Input.mousePosition.y));

        }

        public void SetUIElementsTransparent()
        {
            if (!_isConsoleTransparent)
            {
                foreach (var element in RuntimeConsole.WindowElementList)
                {
                    element.EnableTransperancy();
                }

                _isConsoleTransparent = true;
                _consoleWindowProperties.CacheTransparencyValue(true);
                PrintMessage(MessageType.Header, "Transperancy mode enabled.");
            }
            else
            {
                foreach (var element in RuntimeConsole.WindowElementList)
                {
                    element.DisableTransperancy();
                }

                _isConsoleTransparent = false;
                _consoleWindowProperties.CacheTransparencyValue(false);
                PrintMessage(MessageType.Header, "Transperancy mode disabled.");
            }
        }

        public void EnableClickThrough()
        {
            if (_isRaycastingEnabled)
            {
                PrintMessage(MessageType.Header, "Click through UI mode enabled.");
                EnableRaycasting(false);
                _isRaycastingEnabled = false;
            }
            else
            {
                PrintMessage(MessageType.Header, "Click through UI mode disabled.");
                EnableRaycasting(true);
                _isRaycastingEnabled = true;
            }
        }

        private void EnableRaycasting(bool value)
        {
            foreach (var image in _raycastImages)
            {
                image.raycastTarget = value;
            }

            _consoleWindowProperties.CacheClickThroughValue(value);
            _consoleText.raycastTarget = value;
        }

        private void UpdateScrollOnNewInput() => _scrollRect.verticalNormalizedPosition = 0;

        private void SetRectSize(RectTransform rect, Vector2 newSize)
        {
            rect.sizeDelta = newSize;
            _consoleWindowProperties.ChacheWindowHeight(newSize.y);
        }

        private string GetMessageType(MessageType messageType)
        {
            switch (messageType)
            {
                case MessageType.Error:
                    return " * <color=red>[Error]</color> ";
                case MessageType.Log:
                    return " * <color=white>[Log]</color> ";
                case MessageType.Loop:
                    return " * <color=yellow>[Loop0]</color> ";
                case MessageType.Header:
                    return " * <color=yellow>[Header]</color> ";
            }

            return "message-empty";
        }
    }
}