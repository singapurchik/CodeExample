using UnityEngine;
using System;
using Core;

namespace Location
{
	[CreateAssetMenu(fileName = "Location Info", menuName = "Location/Location Info")]
	public class LocationInfo : PlayerDataValueInfo
	{
		[SerializeField] private PlayerDataKey _currentLocationKey;
		
		public PlayerDataKey CurrentLocationKey => _currentLocationKey;

		public LocationType GetCurrentLocation(IPlayerDataInfo data) =>
			Enum.TryParse<LocationType>(data.GetString(_currentLocationKey), 
				true, out var value) ? value : default;

		public override string ReadCurrentValueAsString(IPlayerDataInfo data) =>
			data.GetString(_currentLocationKey);
	}
}