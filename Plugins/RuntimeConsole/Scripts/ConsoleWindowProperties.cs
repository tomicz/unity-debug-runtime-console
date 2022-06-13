using UnityEngine;

namespace TOMICZ.Debugger
{
    public class ConsoleWindowProperties
    {
        private string _transperancyEnabled = "transperancyEnabled";
        private string _clickThroughEnabled = "clickThroughEnabled";
        private string _windowHeight = "windowHeight";

        public void CacheTransparencyValue(bool value)
        {
            if(value)
            {
                PlayerPrefs.SetInt(_transperancyEnabled, 1);
            }
            else
            {
                PlayerPrefs.SetInt(_transperancyEnabled, 0);
            }
        }

        public bool GetTransperancyState()
        {
            if (PlayerPrefs.HasKey(_transperancyEnabled))
            {
                var value = PlayerPrefs.GetInt(_transperancyEnabled);

                if(value == 1)
                {
                    return false;
                }
            }

            return true;
        }

        public void ChacheWindowHeight(float height) => PlayerPrefs.SetFloat(_windowHeight, height);

        public float GetWindowHeight()
        {
            if (PlayerPrefs.HasKey(_windowHeight))
            {
                return PlayerPrefs.GetFloat(_windowHeight);
            }

            return 480;
        }

        public void CacheClickThroughValue(bool value)
        {
            if (value)
            {
                PlayerPrefs.SetInt(_clickThroughEnabled, 1);
            }
            else
            {
                PlayerPrefs.SetInt(_clickThroughEnabled, 0);
            }
        }

        public bool GetClickThroughState()
        {
            if (PlayerPrefs.HasKey(_clickThroughEnabled))
            {
                var value = PlayerPrefs.GetInt(_clickThroughEnabled);

                if (value == 1)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Persist data state by using key, value properties.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetFloat(string key, float value) => PlayerPrefs.SetFloat(key, value);

        /// <summary>
        /// Get persist data by key. If returned value is -1 it means that it's null.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public float GetFloat(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetFloat(key);
            }

            return -1;
        }
    }
}