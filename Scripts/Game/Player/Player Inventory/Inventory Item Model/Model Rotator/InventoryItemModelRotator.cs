using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FAS.Players
{
	public class InventoryItemModelRotator : MonoBehaviour
	{
		[SerializeField] private float _rotationSpeed = 0.2f;
		
		[Inject] private List<InventoryItemModelRotationInputPanel> _inputPanels;
		
		private InventoryItemModelRotationInputPanel _currentInputPanel;
		private IInventoryModelRotatable _currentRotatable;

		private void OnEnable()
		{
			foreach (var inputPanel in _inputPanels)
				inputPanel.OnStartDrag += SetCurrentInputPanel;
		}

		private void OnDisable()
		{
			foreach (var inputPanel in _inputPanels)
				inputPanel.OnStartDrag -= SetCurrentInputPanel;
		}

		private void SetCurrentInputPanel(InventoryItemModelRotationInputPanel inputPanel)
			=> _currentInputPanel = inputPanel;

		public void SetCurrentRotatable(IInventoryModelRotatable rotatable)
			=> _currentRotatable = rotatable;
		
		public void StopRotating()
		{
			_currentRotatable = null;
		}

		private void Update()
		{
			if (_currentRotatable != null
			    && _currentInputPanel != null
			    && _currentInputPanel.CurrentDelta.sqrMagnitude > 0.001f)
			{
				var delta = _currentInputPanel.CurrentDelta;
				var yAngle = -delta.x * _rotationSpeed;
				var xAngle = delta.y * _rotationSpeed;
				_currentRotatable.Rotate(new Vector2(xAngle, yAngle));
			}
		}
	}
}