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

    public enum AnchorPosition
    {
        Top,
        Max
    }

    public class ConsoleWindow : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private TMP_Text _consoleText;
        [SerializeField] private TMP_Text _loopText;
        [SerializeField] private TMP_Text _headerDescription; 
        [SerializeField] private TMP_Text _fpsCounterText;
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

        private float pollingTime = 1f;
        private float time = 0;
        private int frameCount = 0;

        private void Awake()
        {
            SetupDependencies();

            PrintConsoleMessage("Copyright @ TOMICZ & Darko Tomic.");
            PrintConsoleMessage("Console initilised.");

            LoadPersistantData();
        }

        private void Update()
        {
            ShowFPS();
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
                        _consoleText.text += GetMessageType(MessageType.Header) + message + "</color>" + "\n";
                        UpdateScrollOnNewInput();
                        break;
                }

                if (_header.gameObject.activeInHierarchy)
                {
                    _headerOutputText.text = _consoleText.text;
                }
            }
        }

        public void DragToExpandConsole()
        {
            SetRectSize(_consoleRect, new Vector2(0, _consoleRect.position.y - Input.mousePosition.y));

            if (_isConsoleMaximized)
            {
                SetWindowMaximized(false, AnchorPosition.Top, _consoleWindowProperties.GetWindowHeight());
            }

            if (_isConsoleMinimized)
            {
                SetWindowMinimized(false, _consoleWindowProperties.GetWindowHeight());
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
                PrintConsoleMessage("Transperancy mode enabled.");
            }
            else
            {
                foreach (var element in RuntimeConsole.WindowElementList)
                {
                    element.DisableTransperancy();
                }

                _isConsoleTransparent = false;
                _consoleWindowProperties.CacheTransparencyValue(false);
                PrintConsoleMessage("Transperancy mode disabled.");
            }
        }

        public void EnableClickThrough()
        {
            if (_isRaycastingEnabled)
            {
                PrintConsoleMessage("Click through UI mode enabled.");
                EnableRaycasting(false);
                _isRaycastingEnabled = false;
            }
            else
            {
                PrintConsoleMessage("Click through UI mode disabled.");
                EnableRaycasting(true);
                _isRaycastingEnabled = true;
            }
        }

        private void ShowFPS()
        {
            if(_fpsCounterText == null)
            {
                PrintConsoleMessage("Missing TMP_Text component '_fpsCounterText'. Please assign it in the inspector in order to display the FPS.");
                return;
            }

            time += Time.deltaTime;

            frameCount++;

            if(time > pollingTime)
            {
                int frameRate = Mathf.RoundToInt(frameCount / time);
                _fpsCounterText.text = frameRate.ToString() + " FPS";
                time -= pollingTime;
                frameCount = 0;
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


        private void PrintConsoleMessage(string message)
        {
            _consoleText.text += " ~ <color=orange>[Console]</color> " + message + "\n";
            UpdateScrollOnNewInput();
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
                    return " ~ <color=red>[Error]</color> ";
                case MessageType.Log:
                    return " ~ <color=white>[Log]</color> ";
                case MessageType.Loop:
                    return " ~ <color=yellow>[Loop0]</color> ";
                case MessageType.Header:
                    return " ~ <color=yellow>";
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
                if (_isConsoleMaximized)
                {
                    SetWindowMaximized(false, AnchorPosition.Top, _consoleWindowProperties.GetWindowHeight());
                }

                SetWindowMinimized(true, _header.sizeDelta.y);
            }
            else
            {
                SetWindowMinimized(false, _consoleWindowProperties.GetWindowHeight());
            }
        }

        private void SetWindowMinimized(bool enabled, float windowHeight)
        {
            foreach (var element in _visibleElements)
            {
                if (element != _header)
                {
                    element.gameObject.SetActive(!enabled);
                }
            }

            SetConsoleWindowHeight(windowHeight);
            _headerDescription.transform.gameObject.SetActive(!enabled);
            _headerOutputText.gameObject.SetActive(enabled);
            _isConsoleMinimized = enabled;
            _consoleWindowProperties.SetBoolean(CONSOLE_MINIMIZED_KEY, enabled);
            PrintConsoleMessage("Window is minimized: " + enabled);
        }

        private void SetConsoleWindowHeight(float height) => _consoleRect.sizeDelta = new Vector2(_consoleRect.sizeDelta.x, height);

        public void MaximizeConsole()
        {
            if (!_isConsoleMaximized)
            {
                if (_isConsoleMinimized)
                {
                    SetWindowMinimized(false, _consoleWindowProperties.GetWindowHeight());
                }

                SetWindowMaximized(true, AnchorPosition.Max, 0);
            }
            else
            {
                SetWindowMaximized(false, AnchorPosition.Top, _consoleWindowProperties.GetWindowHeight());
            }
        }

        private void SetWindowMaximized(bool enabled, AnchorPosition anchorPosition, float windowHeight)
        {
            SetAnchorPosition(anchorPosition);
            SetConsoleWindowHeight(windowHeight);
            _isConsoleMaximized = enabled;
            _consoleWindowProperties.SetBoolean(CONSOLE_MAXIMIZED_KEY, enabled);
            PrintConsoleMessage("Window is maximized: " + enabled);
        }

        private void SetAnchorPosition(AnchorPosition position)
        {
            switch (position)
            {
                case AnchorPosition.Top:
                    _consoleRect.anchorMin = new Vector2(0, 1);
                    break;
                case AnchorPosition.Max:
                    _consoleRect.anchorMin = new Vector2(0, 0);
                    break;
            }

        }
    }
}