using System.Collections.Generic;
using FAS.Apartments;
using FAS.Players;
using UnityEngine;
using VInspector;
using Zenject;

namespace FAS.Sounds
{
	public class SoundsInstaller : MonoInstaller
	{
		[SerializeField] private StreetAmbience _streetAmbience;
		[SerializeField] private SoundEffects _soundEffects;
		[SerializeField] private MusicChanger _musicChanger;
		[SerializeField] private Pausable _pausable;
		[SerializeField] private Music _music;
		[SerializeField] private Voice _voice;
		
		public override void InstallBindings()
		{
			Container.Bind<IMusicChanger>().FromInstance(_musicChanger).WhenInjectedInto<PlayerHealthBehaviour>();
			Container.Bind<IStreetAmbience>().FromInstance(_streetAmbience).WhenInjectedInto<ZoneBase>();
			Container.Bind<IMusicChanger>().FromInstance(_musicChanger).WhenInjectedInto<ZoneBase>();
			Container.Bind<IMusicChanger>().FromInstance(_musicChanger).WhenInjectedInto<Player>();
			Container.BindInstance(_music).WhenInjectedInto<MusicChanger>();
			
			Container.Bind<ISoundEffectsPlayer>().FromInstance(_soundEffects).AsSingle();
			Container.Bind<IVoicePlayer>().FromInstance(_voice).AsSingle();
			
			var list = new List<IPausable>(10)
			{
				_streetAmbience,
				_soundEffects,
				_music,
				_voice,
			};
			Container.BindInstance(list).WhenInjectedInto<Pausable>();
		}

#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_streetAmbience = FindObjectOfType<StreetAmbience>(true);
			_soundEffects = FindObjectOfType<SoundEffects>(true);
			_musicChanger = FindObjectOfType<MusicChanger>(true);
			_music = FindObjectOfType<Music>(true);
			_voice = FindObjectOfType<Voice>(true);
		}
#endif
	}
}