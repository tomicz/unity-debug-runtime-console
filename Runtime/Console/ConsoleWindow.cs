using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TOMICZ.Debugger
{
    public enum AnchorPosition
    {
        Top,
        Max
    }

    public class ConsoleWindow : MonoBehaviour, ITick
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
        }

        private void OnDisable()
        {
            UnregisterHeaderEvents();
        }

        private void Update()
        {
            UpdateDebugVisualisers();
        }

        public void Tick()
        {
            UpdateScrollOnNewInput();
        }

        public void PrintMessage(LogMessage logMessage)
        {
            if (_consoleText.text != null)
            {
                _consoleText.text += logMessage;
                UpdateScrollOnNewInput();
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

        public void SelectInputField() => _commandInputField.Select();

        public void RunEvent() => RuntimeConsole.OnUnityRunEvent?.Invoke();

        public void Loop(string message) => _loopText.text = $"[ {message} ]";

        private void RegisterHeaderEvents()
        {
            _header.OnConsoleCollapsedEvent += HandleOnConsoleCollapsed;
            _header.OnConsoleFullscreenEvent += HandleOnFullscreenEnabled;
            _header.OnConsoleClickThroughEvent += HandleOnClickThroughEnabled;
            _header.OnConsoleAutoScrollEvent += HandleOnAutoscrollEnabled;
        }

        private void UnregisterHeaderEvents()
        {
            _header.OnConsoleCollapsedEvent -= HandleOnConsoleCollapsed;
            _header.OnConsoleFullscreenEvent -= HandleOnFullscreenEnabled;
            _header.OnConsoleClickThroughEvent -= HandleOnClickThroughEnabled;
            _header.OnConsoleAutoScrollEvent -= HandleOnAutoscrollEnabled;
        }

        private void HandleOnConsoleCollapsed(bool collapsed)
        {
            CollapseConsole(collapsed);
        }

        private void HandleOnFullscreenEnabled(bool fullscreen)
        {
            SetWindowMinimized(fullscreen);
        }

        private void HandleOnClickThroughEnabled(bool enabled)
        {
            EnableRaycasting(enabled);
        }

        private void HandleOnAutoscrollEnabled(bool enabled)
        {
            _isAutoScrollingEnabled = enabled;
        }

        private void SetupDependencies()
        {
            _consoleWindowProperties = new ConsoleWindowProperties();
            RuntimeConsole.Initilise(this);

            _consoleRect = GetComponent<RectTransform>();
            _scrollRect = GetComponentInChildren<ScrollRect>();
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
            if (LogWriter.Messages.Count < 1)
            {
                return;
            }

            var log = LogWriter.Messages.Pop();

            _consoleText.text += $"[{log.type}] {log.message}\n";
            if (_isAutoScrollingEnabled)
            {
                _scrollRect.verticalNormalizedPosition = 0;
            }
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

        private void UpdateHeaderOutput(string message)
        {
            if (_header.gameObject.activeInHierarchy)
            {
                _headerOutputText.text = "<color=orange>[Main]</color> " + message;
            }
        }

        private void PersistOnSceneChange()
        {
            if (_isPersistant)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        private void UpdateDebugVisualisers()
        {
            if(RuntimeConsole.DebugLineRenderers.Count > 0)
            {
                foreach (var visualiser in RuntimeConsole.DebugLineRenderers)
                {
                    visualiser.UpdatePosition();
                }
            }
        }
    }
}