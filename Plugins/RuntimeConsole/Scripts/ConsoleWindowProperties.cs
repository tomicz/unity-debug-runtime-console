using UnityEngine;

namespace TOMICZ.Debugger
{
    public class ConsoleWindowProperties
    {
        private string _transperancyEnabled = "transperancyEnabled";

        public void SetTransperancyValue(bool value)
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
    }
}