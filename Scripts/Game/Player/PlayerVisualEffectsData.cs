using UnityEngine;
using System;

namespace FAS.Players
{
	[Serializable]
	public struct PlayerVisualEffectsData
	{
		public ParticleSystem LeftLagSlashBloodEffect;
		public ParticleSystem NeckSlashBloodEffect;
		public ParticleSystem StabStomachBloodEffect;
		public ParticleSystem KnifeOutOfStomachBloodEffect;
	}
}