using UnityEngine;
using Core;

namespace Location
{
	[CreateAssetMenu(fileName = "Reset Location To Default", menuName = "Location/Operations/Reset Location To Default")]
	public sealed class ResetLocationToDefault : ConsumeOperation
	{
		[SerializeField] private Location _location;

		public override PlayerDataValueInfo Info => _location.Info;

		public override bool IsCanApply(IPlayerDataInfo data) => _location.GetCurrent(data) != default;

		public override void Apply(PlayerData data) => _location.SetNew(data, default);
	}
}