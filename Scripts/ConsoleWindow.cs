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
        [SerializeField] private TMP_Text _headerDescription; 
        [SerializeField] private Button _expandButton;
        [SerializeField] private RectTransform _header;
        [SerializeField] private TMP_Text _headerOutputText;
        [SerializeField] private Image[] _raycastImages;
        [SerializeField] private Transform[] _visibleElements;

        private RectTransform _consoleRect;
        private ConsoleWindowProperties _consoleWindowProperties;
        private ScrollRect _scrollRect;

        private bool _isConsoleTransparent = false;
        private bool _isRaycastingEnabled = true;
        private bool _isConsoleExpanded = true;
        private bool _isConsoleMinimized = false;
        private bool _isConsoleMaximized = false;

        private const string CONSOLE_EXPANDED_KEY = "console-expanded-key";
        private const string CONSOLE_MINIMIZED_KEY = "console-minimized-key";
        private const string CONSOLE_MAXIMIZED_KEY = "console-maximized-key";

        private void Awake()
        {
            SetupDependencies();
            LoadPersistantData();
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

                _headerOutputText.text = _consoleText.text;
            }
        }

        public void DragToExpandConsole()
        {
            SetRectSize(_consoleRect, new Vector2(0, _consoleRect.position.y - Input.mousePosition.y));

            if (_isConsoleMaximized)
            {
                _consoleRect.anchorMin = new Vector2(0, 1);
                _isConsoleMaximized = false;
                _consoleWindowProperties.SetBoolean(CONSOLE_MAXIMIZED_KEY, false);
            }

            if (_isConsoleMinimized)
            {
                foreach (var element in _visibleElements)
                {
                    if (element != _header)
                    {
                        element.gameObject.SetActive(true);
                    }
                }

                _headerDescription.transform.gameObject.SetActive(true);
                _headerOutputText.gameObject.SetActive(false);
                _isConsoleMinimized = false;
                _consoleWindowProperties.SetBoolean(CONSOLE_MINIMIZED_KEY, false);
            }
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

        private void SetupDependencies()
        {
            RuntimeConsole.SetupConsoleWindow(this);

            _consoleRect = GetComponent<RectTransform>();
            _consoleWindowProperties = new ConsoleWindowProperties();
            _scrollRect = GetComponentInChildren<ScrollRect>();
        }

        private void LoadPersistantData()
        {
            _isConsoleTransparent = _consoleWindowProperties.GetTransperancyState();
            _isRaycastingEnabled = _consoleWindowProperties.GetClickThroughState();
            _isConsoleExpanded = _consoleWindowProperties.GetBoolean(CONSOLE_EXPANDED_KEY);
            _isConsoleMinimized = _consoleWindowProperties.GetBoolean(CONSOLE_MINIMIZED_KEY);
            _isConsoleMaximized = _consoleWindowProperties.GetBoolean(CONSOLE_MAXIMIZED_KEY);

            SetUIElementsTransparent();
            EnableClickThrough();
            SetRectSize(_consoleRect, new Vector2(_consoleRect.sizeDelta.x, _consoleWindowProperties.GetWindowHeight()));

            CheckConsoleExpandStateOnInitilisation();
            MinimizeConsole();
            MaximizeConsole();
        }

        private void CheckConsoleExpandStateOnInitilisation()
        {
            if (_isConsoleExpanded)
            {
                HideConsole();
            }
            else
            {
                ExpandConsole();
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

        private void UpdateScrollOnNewInput()
        {
            if (!_isConsoleMinimized)
            {
                _scrollRect.verticalNormalizedPosition = 0;
            }
        }

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

        public void ExpandConsole()
        {
            foreach(var element in _visibleElements)
            {
                element.gameObject.SetActive(true);
            }
            _expandButton.gameObject.SetActive(false);

            _consoleWindowProperties.SetBoolean(CONSOLE_EXPANDED_KEY, true);
        }

        public void HideConsole()
        {
            foreach (var element in _visibleElements)
            {
                element.gameObject.SetActive(false);
            }

            _expandButton.gameObject.SetActive(true);
            _consoleWindowProperties.SetBoolean(CONSOLE_EXPANDED_KEY, false);
        }

        public void MinimizeConsole()
        {
            if (!_isConsoleMinimized)
            {
                foreach (var element in _visibleElements)
                {
                    if (element != _header)
                    {
                        element.gameObject.SetActive(false);
                    }
                }

                SetConsoleWindowHeight(_header.sizeDelta.y);
                _headerDescription.transform.gameObject.SetActive(false);
                _headerOutputText.gameObject.SetActive(true);
                _isConsoleMinimized = true;
                _consoleWindowProperties.SetBoolean(CONSOLE_MINIMIZED_KEY, true);
            }
            else
            {
                foreach (var element in _visibleElements)
                {
                    if (element != _header)
                    {
                        element.gameObject.SetActive(true);
                    }
                }

                SetConsoleWindowHeight(_consoleWindowProperties.GetWindowHeight());
                _headerDescription.transform.gameObject.SetActive(true);
                _headerOutputText.gameObject.SetActive(false);
                _isConsoleMinimized = false;
                _consoleWindowProperties.SetBoolean(CONSOLE_MINIMIZED_KEY, false);
            }
        }

        private void SetConsoleWindowHeight(float height) => _consoleRect.sizeDelta = new Vector2(_consoleRect.sizeDelta.x, height);

        public void MaximizeConsole()
        {
            if (!_isConsoleMaximized)
            {
                _consoleRect.anchorMin = new Vector2(0, 0);
                SetConsoleWindowHeight(0);
                _isConsoleMaximized = true;
                _consoleWindowProperties.SetBoolean(CONSOLE_MAXIMIZED_KEY, true);
            }
            else
            {
                _consoleRect.anchorMin = new Vector2(0, 1);
                SetConsoleWindowHeight(_consoleWindowProperties.GetWindowHeight());
                _isConsoleMaximized = false;
                _consoleWindowProperties.SetBoolean(CONSOLE_MAXIMIZED_KEY, false);
            }
        }
    }
}