using UnityEngine;
using UnityEngine.UI;

namespace TOMICZ.Debugger
{
    public class WindowElement : MonoBehaviour
    {
        [Header("Element properties")]
        [SerializeField] private float _elementAlphaTransperancy = 1f;

        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();

            RuntimeConsole.AddWindowElement(this);
        }

        public void EnableTransperancy() => SetBackgroundAlpha(_elementAlphaTransperancy);

        public void DisableTransperancy() => SetBackgroundAlpha(1);

        private void SetBackgroundAlpha(float alphaAmount)
        {
            Color tempColor = _image.color;
            tempColor.a = alphaAmount;
            _image.color = tempColor;
        }
    }
}