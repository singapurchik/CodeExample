using UnityEngine.Animations.Rigging;
using FAS.Players.Animations;
using UnityEngine;
using Zenject;

namespace FAS.Players.AnimRig
{
	public class PlayerAnimationRig : MonoBehaviour
	{
		[Space(5)]
		[SerializeField] private Rig _hands;

		[Space(10)]
		[Header("Left Hand Rig")]
		[SerializeField] private ChainIKConstraint _leftHand;
		[SerializeField] private Transform _leftHandTarget;

		[Space(10)]
		[Header("Right Hand Rig")]
		[SerializeField] private ChainIKConstraint _rightHand;
		[SerializeField] private Transform _rightHandTarget;

		[Space(10)]
		[Header("Torso Rig")]
		[SerializeField] private Rig _spine;
		
		[Space(10)]
		[Header("Head Rig")]
		[SerializeField] private Rig _head;
		[SerializeField] private Transform _headTarget;

		private readonly WeightChanger _rightHandWeightData = new ();
		private readonly WeightChanger _leftHandWeightData = new ();
		private readonly WeightChanger _handsWeightData = new ();
		private readonly WeightChanger _spineWeightData = new ();
		private readonly WeightChanger _headWeightData = new ();
		
		private Transform _currentRightHandTarget;
		private Transform _currentLeftHandTarget;
		
		public void SetHeadTargetRotation(Vector3 rotation) => _headTarget.localRotation = Quaternion.Euler(rotation);

		public Quaternion GetHeadTargetRotation() => _headTarget.localRotation;

		public void RequestEnableSpine() => _spineWeightData.RequestEnable();

		public void RequestEnableHands() => _handsWeightData.RequestEnable();

		public void RequestEnableHead() => _headWeightData.RequestEnable();
		
		public void RequestEnableRightHand(float weight = 1, Transform target = null)
		{
			_currentRightHandTarget = target;
			_rightHandWeightData.RequestEnable();
		}

		public void RequestEnableLeftHand(float weight = 1, Transform target = null)
		{
			_currentLeftHandTarget = target;
			_leftHandWeightData.RequestEnable();
		}

		private void Update()
		{
			if (_spineWeightData.IsWeightChanged(out var spineWeight))
				_spine.weight = spineWeight;
			
			if (_handsWeightData.IsWeightChanged(out var handsWeight))
				_hands.weight = handsWeight;
			
			if (_headWeightData.IsWeightChanged(out var headWeight))
				_head.weight = headWeight;
			
			if (_rightHandWeightData.IsWeightChanged(out var rightHandWeight))
				_rightHand.weight = rightHandWeight;

			if (_leftHand.weight > 0 && _currentLeftHandTarget != null)
				_leftHandTarget.SetPositionAndRotation(_currentLeftHandTarget.position, _currentLeftHandTarget.rotation);

			if (_rightHand.weight > 0 && _currentRightHandTarget != null)
				_rightHandTarget.SetPositionAndRotation(_currentRightHandTarget.position, _currentRightHandTarget.rotation);
		}
	}
}
