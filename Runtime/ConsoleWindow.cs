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
        Header,
        Unity
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
        [SerializeField] private TMP_Text _unityText;
        [SerializeField] private TMP_Text _loopText;
        [SerializeField] private TMP_Text _headerDescription; 
        [SerializeField] private TMP_Text _fpsCounterText;
        [SerializeField] private Transform _mainContainer;
        [SerializeField] private Button _expandButton;
        [SerializeField] private Header _header;
        [SerializeField] private TMP_Text _headerOutputText;
        [SerializeField] private Image[] _raycastImages;
        [SerializeField] private Transform[] _visibleElements;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private TMP_InputField _commandInputField;

        [Header("Properties")]
        [SerializeField] private bool _isPersistant;

        private RectTransform _consoleRect;
        private ConsoleWindowProperties _consoleWindowProperties;
        private ScrollRect _scrollRect;

        private bool _isAutoScrollingEnabled = true;
        private bool _isEnabled = false;

        private void Awake()
        {
            SetupDependencies();
            PersistOnSceneChange();
        }

        private void OnEnable()
        {
            RegisterHeaderEvents();
            Application.logMessageReceived += OnUnityLogMessageReceived;
        }

        private void OnDisable()
        {
            UnregisterHeaderEvents();
            Application.logMessageReceived -= OnUnityLogMessageReceived;
        }

        public void Log(string message)
        {
            _consoleText.text += GetMessageType(MessageType.Log) + message + "\n";
            UpdateScrollOnNewInput();
            UpdateHeaderOutput(message);
        }

        public void Header(string message)
        {
            _consoleText.text += GetMessageType(MessageType.Header) + message.ToUpper() + "</color>" + "\n";
            UpdateScrollOnNewInput();
            UpdateHeaderOutput(message);
        }

        public void Error(string message)
        {
            _consoleText.text += GetMessageType(MessageType.Error) + message + "\n";
            UpdateScrollOnNewInput();
            UpdateHeaderOutput(message);
        }

        public void Loop(string message)
        {
            _loopText.text = GetMessageType(MessageType.Loop) + message;
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
                        _consoleText.text += GetMessageType(MessageType.Header) + message.ToUpper() + "</color>" + "\n";
                        UpdateScrollOnNewInput();
                        break;
                    case MessageType.Unity:
                        _unityText.text += GetMessageType(MessageType.Unity) + message + "\n";
                        UpdateScrollOnNewInput();
                        break;
                }

                if (messageType != MessageType.Loop && _header.gameObject.activeInHierarchy)
                {
                    _headerOutputText.text = "<color=orange>[Main]</color> " + message;
                }
            }
        }

        public void SetConsoleWindowHeight(float height) => _consoleRect.sizeDelta = new Vector2(_consoleRect.sizeDelta.x, height);

        public void SetAnchorPosition(AnchorPosition position)
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

        public void EnableConsole() => gameObject.SetActive(_isEnabled = !_isEnabled);

        public void SelectInputField()
        {
            _commandInputField.Select();
        }

        private void RegisterHeaderEvents()
        {
            _header.OnConsoleCollapsedEvent += HandleOnConsoleCollapsed;
            _header.OnConsoleFullscreenEvent += HandleOnFullscreenEnabled;
            _header.OnConsoleTransparentEvent += HandleOnTransparentWindow;
            _header.OnConsoleClickThroughEvent += HandleOnClickThroughEnabled;
            _header.OnConsoleAutoScrollEvent += HandleOnAutoscrollEnabled;
        }

        private void UnregisterHeaderEvents()
        {
            _header.OnConsoleCollapsedEvent -= HandleOnConsoleCollapsed;
            _header.OnConsoleFullscreenEvent -= HandleOnFullscreenEnabled;
            _header.OnConsoleTransparentEvent -= HandleOnTransparentWindow;
            _header.OnConsoleClickThroughEvent -= HandleOnClickThroughEnabled;
            _header.OnConsoleAutoScrollEvent -= HandleOnAutoscrollEnabled;
        }

        private void HandleOnConsoleCollapsed(bool collapsed)
        {
            CollapseConsole(collapsed);
            PrintConsoleMessage($"Console collapsed: {collapsed}");
        }

        private void HandleOnFullscreenEnabled(bool fullscreen)
        {
            SetWindowMinimized(fullscreen);
            PrintConsoleMessage($"Fullscreen enabled: {fullscreen}");
        }

        private void HandleOnTransparentWindow(bool transparent)
        {
            SetWindowTransparent(transparent);
            PrintConsoleMessage($"Transparency enabled: {transparent}");
        }

        private void HandleOnClickThroughEnabled(bool enabled)
        {
            EnableRaycasting(enabled);
            PrintConsoleMessage($"Click through enabled: {enabled}");
        }

        private void HandleOnAutoscrollEnabled(bool enabled)
        {
            _isAutoScrollingEnabled = enabled;
            PrintConsoleMessage($"Autoscroll enabled: {enabled}");
        }

        private void SetupDependencies()
        {
            _consoleWindowProperties = new ConsoleWindowProperties();
            RuntimeConsole.SetupConsoleWindow(this);

            _consoleRect = GetComponent<RectTransform>();
            _scrollRect = GetComponentInChildren<ScrollRect>();
        }

        private void PrintConsoleMessage(string message)
        {
            _consoleText.text += "~ <color=orange>[Console]</color> " + message + "\n";
            UpdateHeaderOutput(message);
            UpdateScrollOnNewInput();
        }

        private void EnableRaycasting(bool value)
        {
            foreach (var image in _raycastImages)
            {
                image.raycastTarget = !value;
            }

            _consoleText.raycastTarget = !value;
            _consoleWindowProperties.CacheClickThroughValue(!value);
        }

        private void UpdateScrollOnNewInput()
        {
            if (_isAutoScrollingEnabled)
            {
                _scrollRect.verticalNormalizedPosition = 0;
            }
        }

        private string GetMessageType(MessageType messageType)
        {
            switch (messageType)
            {
                case MessageType.Error:
                    return "~ <color=red>[Error]</color> ";
                case MessageType.Log:
                    return "~ <color=white>[Log]</color> ";
                case MessageType.Loop:
                    return "<color=yellow>[Loop0]</color> ";
                case MessageType.Header:
                    return "~ <color=yellow>";
                case MessageType.Unity:
                    return "~ <color=#70afe9>[Unity]</color>";
            }

            return "message-empty";
        }

        private void CollapseConsole(bool value)
        {
            _mainContainer.gameObject.SetActive(value);
            _expandButton.gameObject.SetActive(!value);
        }

        private void SetWindowMinimized(bool value)
        {
            foreach (var element in _visibleElements)
            {
                element.gameObject.SetActive(value);
            }

            _backgroundImage.enabled = value;
            _headerDescription.transform.gameObject.SetActive(value);
            _headerOutputText.gameObject.SetActive(!value);
        }

        private void SetWindowTransparent(bool value)
        {
            foreach (var element in RuntimeConsole.WindowElementList)
            {
                element.EnableTransperancy(value);
            }
        }

        private void UpdateHeaderOutput(string message)
        {
            if (_header.gameObject.activeInHierarchy)
            {
                _headerOutputText.text = "<color=orange>[Main]</color> " + message;
            }
        }

        private void OnUnityLogMessageReceived(string logString, string stackTrace, LogType type)
        {
            if(_unityText == null)
            {
                Error("Couln't print Unity message because TMP_Text _unityText component referecne is not added in the inspector. Inspect Console Window prefab and drag _unityText reference.");
                return;
            }

            switch (type)
            {
                case LogType.Assert:
                    PrintMessage(MessageType.Unity, "<color=red>[Assert]</color>" + logString);
                    break;
                case LogType.Error:
                    PrintMessage(MessageType.Unity, "<color=red>[Error]</color>" + logString);
                    break;
                case LogType.Exception:
                    PrintMessage(MessageType.Unity, "<color=red>[Exception]</color>" + logString);
                    break;
                case LogType.Log:
                    PrintMessage(MessageType.Unity, "<color=white>[Log]</color>" + logString);
                    break;
                case LogType.Warning:
                    PrintMessage(MessageType.Unity, "<color=yellow>[Warning]</color>" + logString);
                    break;
            }
        }

        private void PersistOnSceneChange()
        {
            if (_isPersistant)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}