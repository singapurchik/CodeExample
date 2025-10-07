using FAS.Apartments.Inside;
using FAS.Actors.Emenies;
using UnityEngine;
using Zenject;

namespace FAS.Actors.Enemies.Geisha
{
	public class GeishaShowingFace : ShowingFace
	{
		[SerializeField] private Transform _screamerPoint;
		[SerializeField] private SkinnedMeshRenderer _body;
		[SerializeField] private float _playAnimDelay = 0.5f;
		
		[Inject] private Jumpscare _jumpscare;
		
		private bool _isWaitingForPlayAnim;

		private float _nextTimePlayAnim;

		public override void Enter()
		{
			base.Enter();
			IsWaitingForCameraBlend = true;
			_isWaitingForPlayAnim = false;
		}

		protected override void OnCameraBlendCompleted()
		{
			base.OnCameraBlendCompleted();
			_body.enabled = false;
			_jumpscare.SetParent(_screamerPoint, true);
			_jumpscare.ReadyToPlay();
			IsWaitingForCameraBlend = false;
			_isWaitingForPlayAnim = true;
			_nextTimePlayAnim = Time.timeSinceLevelLoad + _playAnimDelay;
		}
		
		public override void Perform()
		{
			base.Perform();
			if (_isWaitingForPlayAnim && Time.timeSinceLevelLoad > _nextTimePlayAnim)
			{
				_jumpscare.Play();
				_isWaitingForPlayAnim = false;
			}
		}
	}
}