using System;
using TMPro;
using UnityEngine;

namespace TOMICZ.Debugger
{
    public class Header : MonoBehaviour
    {
        public Action<bool> OnConsoleCollapsedEvent;
        public Action<bool> OnConsoleFullscreenEvent;
        public Action<bool> OnConsoleTransparentEvent;
        public Action<bool> OnConsoleClickThroughEvent;
        public Action<bool> OnConsoleAutoScrollEvent;

        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private TMP_Text _fpsCounterText;

        private bool _isConsoleWindowCollapsed = true;
        private bool _isConsoleFullscreen = true;
        private bool _isConsoleTransparent = false;
        private bool _isClickThroughEnabled = false;
        private bool _isAutoScrollingEnabled = true;

        private float pollingTime = 1f;
        private float time = 0;
        private int frameCount = 0;

        private void Update()
        {
            ShowFPS();
        }

        public void CollapseConsole() => OnConsoleCollapsedEvent?.Invoke(_isConsoleWindowCollapsed = !_isConsoleWindowCollapsed);

        public void EnableFullscreen() => OnConsoleFullscreenEvent?.Invoke(_isConsoleFullscreen = !_isConsoleFullscreen);

        public void SetConsoleTransparent() => OnConsoleTransparentEvent?.Invoke(_isConsoleTransparent = !_isConsoleTransparent);

        public void EnableClickThrough() => OnConsoleClickThroughEvent?.Invoke(_isClickThroughEnabled = !_isClickThroughEnabled);

        public void EnableAutoscrolling() => OnConsoleAutoScrollEvent?.Invoke(_isAutoScrollingEnabled = !_isAutoScrollingEnabled);

        private void ShowFPS()
        {
            time += Time.deltaTime;

            frameCount++;

            if (time > pollingTime)
            {
                int frameRate = Mathf.RoundToInt(frameCount / time);
                _fpsCounterText.text = frameRate.ToString() + " FPS";
                time -= pollingTime;
                frameCount = 0;
            }
        }
    }
}