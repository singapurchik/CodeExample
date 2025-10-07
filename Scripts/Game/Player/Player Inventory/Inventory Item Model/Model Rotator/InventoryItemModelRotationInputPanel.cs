using UnityEngine.EventSystems;
using UnityEngine;
using System;

namespace FAS.Players
{
	public class InventoryItemModelRotationInputPanel : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
	{
		[SerializeField] private float _damping = 5f;

		public Vector2 InertiaVelocity { get; private set; } = Vector2.zero;
		public Vector2 CurrentDelta { get; private set; } = Vector2.zero;
		
		public bool IsDragging { get; private set; } = false;

		private Vector2 _previousPosition;

		public event Action<InventoryItemModelRotationInputPanel> OnStartDrag;

		private void OnDisable()
		{
			InertiaVelocity = Vector2.zero;
			CurrentDelta = Vector2.zero;
			IsDragging = false;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			IsDragging = true;
			_previousPosition = eventData.position;
			InertiaVelocity = Vector2.zero;
			CurrentDelta = Vector2.zero;
			OnStartDrag?.Invoke(this);
		}

		public void OnDrag(PointerEventData eventData)
		{
			var current = eventData.position;
			var delta = current - _previousPosition;
			_previousPosition = current;

			CurrentDelta = delta;
			InertiaVelocity = delta / Time.deltaTime;
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			IsDragging = false;
		}
		
		private void Update()
		{
			if (!IsDragging && InertiaVelocity.sqrMagnitude > 0.001f)
			{
				CurrentDelta = InertiaVelocity * Time.deltaTime;
				InertiaVelocity = Vector2.Lerp(InertiaVelocity, Vector2.zero, Time.deltaTime * _damping);
			}
			else if (!IsDragging)
			{
				CurrentDelta = Vector2.zero;
			}
		}
	}
}