using Zenject;

namespace FAS
{
	public class CharacterShadowByZone : CharacterFakeShadow
	{
		[Inject] private IZonesHolderInfo _zonesInfo;

		private bool _isForceShadowZone;

		private void OnEnable()
		{
			_zonesInfo.OnZoneChanged += OnZoneChanged;
		}

		private void OnDisable()
		{
			_zonesInfo.OnZoneChanged -= OnZoneChanged;
		}

		private void OnZoneChanged(ZoneType zoneType)
		{
			switch (zoneType)
			{
				case ZoneType.Apartment:
				case ZoneType.Corridor:
					_isForceShadowZone = true;
					break;
				case ZoneType.Street:
				default:
					_isForceShadowZone = false;
					break;
			}
		}

		protected override void Update()
		{
			if (_isForceShadowZone)
				RequestEnable();
			
			base.Update();
		}
	}
}