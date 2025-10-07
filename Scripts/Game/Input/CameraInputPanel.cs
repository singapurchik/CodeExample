using UnityEngine.EventSystems;
using UnityEngine;

namespace FAS.Players
{
	public class CameraInputPanel : MonoBehaviour, IReadOnlyCameraInputPanel, IPointerDownHandler, IPointerUpHandler
	{
		[SerializeField] private float _maxInputDistance = 50f;
        
		private PointerEventData _currentEventData;
        
		private Vector2 _currentPosition;
		private Vector2 _startPosition;
        
		private bool _isInputProcess;

		public Vector2 CurrentInputVector { get; private set; }
        
		public bool IsInputProcess => _isInputProcess;

		public void OnPointerDown(PointerEventData eventData)
		{
			_currentEventData = eventData;
            
			_startPosition = _currentEventData.position;
			UpdateInputVector(_currentEventData);
			_isInputProcess = true;
		}
        
		public void UpdateInputVector(PointerEventData eventData)
		{
			_currentPosition = eventData.position;
			var deltaPosition = _currentPosition - _startPosition;

			var distance = deltaPosition.magnitude;
			var normalizedDistance = Mathf.Clamp01(distance / _maxInputDistance);

			CurrentInputVector = deltaPosition * normalizedDistance;

			_startPosition = _currentPosition;
		}
        
		public void OnPointerUp(PointerEventData eventData)
		{
			_isInputProcess = false;
		}

		private void Update()
		{
			if (_isInputProcess)
				UpdateInputVector(_currentEventData);
		}
	}
}