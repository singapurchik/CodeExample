using UnityEngine;
using Core;

namespace VIP
{
	[CreateAssetMenu(fileName = "Add VIP Time", menuName = "VIP/Operations/Add VIP Time")]
	public sealed class AddVipTime : ProvideOperation, IOperationWithParameter
	{
		[SerializeField] private VIP _vip;
		[Min(1)] [SerializeField] private int _defaultSeconds = 30;

		public override PlayerDataValueInfo Info => _vip.Info;

		public IOperationParameter CreateDefaultParam() => new IntAmountParameter { Amount = _defaultSeconds };

		public bool IsSupports(IOperationParameter parameter) => parameter is IntAmountParameter;

		public bool IsCanApply(IPlayerDataInfo data, IOperationParameter parameter) => true;

		public override void Apply(PlayerData data) => AddSeconds(data, _defaultSeconds);

		public void Apply(PlayerData data, IOperationParameter parameter)
		{
			var intParam = parameter as IntAmountParameter;
			AddSeconds(data, intParam?.Amount ?? _defaultSeconds);
		}

		private void AddSeconds(PlayerData data, int seconds)
		{
			seconds = Mathf.Max(1, seconds);

			var now = VipTime.NowTicks();
			var until = _vip.GetUntilTicks(data);
			
			if (until < now)
				until = now;

			var addTicks = VipTime.SecondsToTicks(seconds);
			var maxAdd = long.MaxValue - until;
			var newUntil = addTicks > maxAdd ? long.MaxValue : until + addTicks;

			_vip.SetUntilTicks(data, newUntil);
		}

#if UNITY_EDITOR
		private void OnValidate() => _defaultSeconds = Mathf.Max(1, _defaultSeconds);
#endif
	}
}