using UnityEngine.Audio;
using UnityEngine;
using System;

namespace FAS.Sounds.AudioMixerParametrs
{
	[Serializable]
	public class LowpassParameterConfig : AudioMixerParameterConfig
	{
		[SerializeField] private float _cutoff = 22000f;
		[SerializeField] private float _resonance = 1f;
		
		public override AudioMixerParameter Type => AudioMixerParameter.Lowpass;
		
		private const string CUTOFF_FREQ = "Cutoff Freq";
		private const string RESONANCE = "Resonance";
		
		public override void Apply(AudioMixer mixer, string prefix)
		{
			mixer.SetFloat($"{prefix} {CUTOFF_FREQ}", _cutoff);
			mixer.SetFloat($"{prefix} {RESONANCE}", _resonance);
		}
	}
}