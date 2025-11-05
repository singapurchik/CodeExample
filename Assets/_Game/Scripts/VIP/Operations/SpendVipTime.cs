using UnityEngine;
using Core;

namespace VIP
{
	[CreateAssetMenu(fileName = "Spend VIP Time", menuName = "VIP/Operations/Spend VIP Time")]
	public sealed class SpendVipTime : ConsumeOperation, IOperationWithParameter
	{
		[SerializeField] private VIP _vip;
		[Min(1)] [SerializeField] private int _defaultSeconds = 1;

		public override PlayerDataValueInfo Info => _vip.Info;

		public IOperationParameter CreateDefaultParam() => new IntAmountParameter { Amount = _defaultSeconds };

		public bool IsSupports(IOperationParameter parameter) => parameter is IntAmountParameter;

		public bool IsCanApply(IPlayerDataInfo data, IOperationParameter parameter)
		{
			var intParam = parameter as IntAmountParameter;
			return HasEnough(data, intParam?.Amount ?? _defaultSeconds);
		}

		public override bool IsCanApply(IPlayerDataInfo data) => HasEnough(data, _defaultSeconds);

		public override void Apply(PlayerData data) => SpendSeconds(data, _defaultSeconds);

		public void Apply(PlayerData data, IOperationParameter parameter)
		{
			var intParam = parameter as IntAmountParameter;
			SpendSeconds(data, intParam?.Amount ?? _defaultSeconds);
		}

		private bool HasEnough(IPlayerDataInfo data, int seconds)
		{
			seconds = Mathf.Max(1, seconds);
			var now = VipTime.NowTicks();
			var until = _vip.GetUntilTicks(data);
			var need = VipTime.SecondsToTicks(seconds);
			return until > now && (until - now) >= need;
		}

		private void SpendSeconds(PlayerData data, int seconds)
		{
			seconds = Mathf.Max(1, seconds);
			var now = VipTime.NowTicks();
			var until = _vip.GetUntilTicks(data);

			if (until > now)
			{
				var next = until - VipTime.SecondsToTicks(seconds);
				_vip.SetUntilTicks(data, next <= now ? 0L : next);
			}
			else
			{
				_vip.SetUntilTicks(data, 0L);
			}
		}

#if UNITY_EDITOR
		private void OnValidate() => _defaultSeconds = Mathf.Max(1, _defaultSeconds);
#endif
	}
}