using UnityEngine;

namespace TOMICZ.Debugger.DebugVisualisers
{
    public class DebugLineRenderer
    {
        private GameObject _primitive;
        private Renderer _renderer;

        private Transform _startPosition;
        private Transform _endPosition;

        private float _lineWidth = 0;
        private float _lineLength = 0;

        public DebugLineRenderer(Transform startPosition, Transform endPosition, float lineWidth, Color color)
        {
            _startPosition = startPosition;
            _endPosition = endPosition;
            CreateLine(startPosition, endPosition, lineWidth, color);
        }

        public void UpdatePosition()
        {
            _lineLength = Vector3.Distance(_startPosition.position, _endPosition.position);

            _primitive.transform.position = (_startPosition.position + _endPosition.position) / 2;
            _primitive.transform.LookAt(_endPosition);
            _primitive.transform.localScale = new Vector3(_lineWidth, _lineWidth, Mathf.Abs(_lineLength));
        }

        private void CreateLine(Transform startPosition, Transform endPosition, float lineWidth, Color color)
        {
            _lineLength = Vector3.Distance(startPosition.position, endPosition.position);
            _lineWidth = lineWidth;

            _primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _primitive.transform.position = (startPosition.position + endPosition.position) / 2;
            _primitive.transform.LookAt(endPosition);
            _primitive.transform.localScale = new Vector3(lineWidth, lineWidth, _lineLength);

            _renderer = _primitive.GetComponent<Renderer>();
            _renderer.material.color = color;
        }
    }
}
