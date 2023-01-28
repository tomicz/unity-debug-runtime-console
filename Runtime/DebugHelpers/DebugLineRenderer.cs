using UnityEngine;

namespace TOMICZ.Debugger.DebugVisualisers
{
    public class DebugLineRenderer
    {
        private GameObject _primitive;
        private Renderer _renderer;

        private float _lineWidth = 0;
        private float _lineLength = 0;

        public DebugLineRenderer(Vector3 startPosition, Vector3 endPosition, float lineWidth, Color color)
        {
            CreateLine(startPosition, endPosition, lineWidth, color);
        }

        public void UpdatePosition(Vector3 startPosition, Vector3 endPosition)
        {
            _lineLength = Vector3.Distance(startPosition, endPosition);

            _primitive.transform.position = (startPosition + endPosition) / 2;
            _primitive.transform.LookAt(endPosition);
            _primitive.transform.localScale = new Vector3(_lineWidth, _lineWidth, Mathf.Abs(_lineLength));
        }

        private void CreateLine(Vector3 startPosition, Vector3 endPosition, float lineWidth, Color color)
        {
            _lineLength = Vector3.Distance(startPosition, endPosition);

            _lineWidth = lineWidth;
            _primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _primitive.transform.position = (startPosition + endPosition) / 2;
            _primitive.transform.LookAt(endPosition);
            _primitive.transform.localScale = new Vector3(lineWidth, lineWidth, _lineLength);

            _renderer = _primitive.GetComponent<Renderer>();
            _renderer.material.color = color;
        }
    }
}
