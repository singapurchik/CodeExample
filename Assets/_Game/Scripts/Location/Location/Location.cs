using UnityEngine;
using Core;

namespace Location
{
	[CreateAssetMenu(fileName = "Location", menuName = "Location/Location")]
	public class Location : PlayerDataValue
	{
		[SerializeField] private LocationInfo _info;
		[SerializeField] private LocationType _startLocation = LocationType.Lobby;
		
		public override PlayerDataValueInfo Info => _info;

		public override void Initialize(PlayerData data) => SetNew(data, _startLocation);
		
		public void SetNew(PlayerData data, LocationType locationType)
			=> data.SetString(_info.CurrentLocationKey, locationType.ToString());

		public LocationType GetCurrent(IPlayerDataInfo data) => _info.GetCurrentLocation(data);
	}
}