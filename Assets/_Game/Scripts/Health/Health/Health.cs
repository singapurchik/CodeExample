using System;
using UnityEngine;
using Core;

namespace Health
{
	[CreateAssetMenu(fileName = "Health", menuName = "Health/Health")]
	public sealed class Health : PlayerDataValue
	{
		[SerializeField] private HealthInfo _info;
		[SerializeField] private int _startHealth = 10;
		[SerializeField] private int _maxHealth = 100;

		public override PlayerDataValueInfo Info => _info;

		public override void Initialize(PlayerData data)
			=> data.SetInt(_info.CurrentHealthKey, Mathf.Clamp(_startHealth, 0, _maxHealth));
		
		public void Increase(PlayerData data, int amount)
			=> data.SetInt(_info.CurrentHealthKey, GetCurrent(data) + amount);

		public void Decrease(PlayerData data, int amount)
			=> data.SetInt(_info.CurrentHealthKey, Mathf.Max(0, GetCurrent(data) - amount));
		
		public int GetCurrent(IPlayerDataInfo data) => _info.GetCurrentHealth(data);

#if UNITY_EDITOR
		private void OnValidate()
		{
			_maxHealth = Mathf.Max(1, _maxHealth);
			_startHealth = Mathf.Clamp(_startHealth, 0, _maxHealth);
		}
#endif
	}
}