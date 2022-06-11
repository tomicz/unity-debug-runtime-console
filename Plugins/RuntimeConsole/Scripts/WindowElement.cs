using UnityEngine;
using UnityEngine.UI;

namespace TOMICZ.Debugger
{
    public class WindowElement : MonoBehaviour
    {
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();

            RuntimeConsole.AddWindowElement(this);
        }

        public void SetBackgroundAlpha(float alphaAmount)
        {
            Color tempColor = _image.color;
            tempColor.a = alphaAmount;
            _image.color = tempColor;
        }
    }
}