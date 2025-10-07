using UnityEngine;
using Zenject;

namespace FAS.Players
{
	public class PlayerCharacterController : MonoBehaviour
	{
		[SerializeField] private float _moveCenterToDefaultSpeed = 1f;
		
		[Inject] private CharacterController _characterController;
		
		private Vector3 _requestedCenterOffset;
		private Vector3 _defaultCenter;
		
		private float _requestedAddCenterOffsetSpeed;
		
		private bool _isAddCenterOffsetRequested;

		private void Awake()
		{
			_defaultCenter = _characterController.center;
		}

		public void RequestAddCenterOffset(Vector3 centerOffset, float speed = 1)
		{
			_isAddCenterOffsetRequested = true;
			_requestedCenterOffset = centerOffset;
			_requestedAddCenterOffsetSpeed = speed;
		}

		private void Update()
		{
			if (_isAddCenterOffsetRequested)
			{
				var targetCenter = _defaultCenter + _requestedCenterOffset;
				
				if (_characterController.center != targetCenter)
					_characterController.center =
						Vector3.MoveTowards(_characterController.center, targetCenter,
							_requestedAddCenterOffsetSpeed * Time.deltaTime);

				_isAddCenterOffsetRequested = false;
			}
			else if (_characterController.center != _defaultCenter)
			{
				_characterController.center =
					Vector3.MoveTowards(_characterController.center, _defaultCenter,
						_moveCenterToDefaultSpeed * Time.deltaTime);
			}
		}
	}
}