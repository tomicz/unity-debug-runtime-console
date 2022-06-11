using UnityEngine;

namespace TOMICZ.Debugger
{
    public class ConsoleWindowProperties
    {
        private string _transperancyEnabled = "transperancyEnabled";
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
    }
}