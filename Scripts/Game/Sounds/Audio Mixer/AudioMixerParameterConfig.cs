using UnityEngine.Audio;
using System;

namespace FAS.Sounds.AudioMixerParametrs
{
	[Serializable]
	public abstract class AudioMixerParameterConfig
	{
		public abstract AudioMixerParameter Type { get; }

		public abstract void Apply(AudioMixer mixer, string prefix);
	}
}