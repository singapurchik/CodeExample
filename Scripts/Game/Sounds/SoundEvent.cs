using FAS.Sounds.AudioMixerParametrs;
using System.Collections.Generic;
using UnityEngine;
using FAS.Utils;

namespace FAS.Sounds
{
	[CreateAssetMenu(fileName = "Sound Event", menuName = "FAS/Sound Event", order = 0)]
	public class SoundEvent : ScriptableObject
	{
		[SerializeField] private List<AudioClip> _clips = new (0);
		[SerializeField] private float _volume = 1f;
		[Range(-80f, 20f)] [SerializeField] private float _volumeDB;
		[SerializeReference] private List<AudioMixerParameterConfig> _mixerConfigs;

		private readonly List<AudioClip> _clipsOrder = new (10);
		private AudioClip _lastClip;
		
		private int _clipIndex;

		public List<AudioMixerParameterConfig> MixerParameters => _mixerConfigs;
		public AudioClip CurrentClip { get; private set; }
		
		public float VolumeDB => _volumeDB;
		public float Volume => _volume;

		public AudioClip GetClip()
		{
#if UNITY_EDITOR
			CurrentClip = _clips[0];
#endif
			if (CurrentClip == null)
			{
				CurrentClip = _clips[0];
			}
			else if (_clips.Count > 1)
			{
				if (_clipIndex >= _clipsOrder.Count)
					RebuildClipsOrder();

				CurrentClip = _clipsOrder[_clipIndex++];
			}

			_lastClip = CurrentClip;
			return CurrentClip;
		}

		private void RebuildClipsOrder()
		{
			_clipsOrder.Clear();

			_clipsOrder.AddRange(_clips);
			_clipsOrder.Shuffle();

			if (_clipsOrder.Count > 1 && _clipsOrder[0] == _lastClip)
			{
				int swap = 1;
				(_clipsOrder[0], _clipsOrder[swap]) = (_clipsOrder[swap], _clipsOrder[0]);
			}

			_clipIndex = 0;
		}
	}
}