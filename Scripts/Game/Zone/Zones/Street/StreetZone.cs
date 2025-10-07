using UnityEngine;
using FAS.Sounds;
using Zenject;

namespace FAS
{
	[RequireComponent(typeof(ZoneInside))]
	public class StreetZone : Zone<ZoneInside>
	{
		[SerializeField] private Transform _enterPoint;

		[Inject] private IStreetAmbience _streetAmbience;
		[Inject] private IMusicChanger _musicChanger;

		public override Transform EnterPoint => _enterPoint;
		
		public override ZoneType Type => ZoneType.Street;
		
		public override bool IsReturnable { get; } = true;

		public override void Enter()
		{
			base.Enter();
			_musicChanger.PlayStreetBackgroundMusic();
			_streetAmbience.Unmute();
		}
	}
}