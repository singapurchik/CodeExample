using System.Collections.Generic;
using UnityEngine;
using Zenject;

#if UNITY_EDITOR
using Core.Editor;
#endif

namespace Core
{
	public class ProjectInstaller : MonoInstaller
	{
		[SerializeField] private CoroutineRunner _coroutineRunner;
		[SerializeField] private List<PlayerDataValue> _playerDataValues = new ();
		
		private readonly PlayerData _playerData = new ();
		
		public override void InstallBindings()
		{
			Container.Bind<ICoroutineRunner>().FromInstance(_coroutineRunner).AsSingle();
			Container.Bind<IPlayerDataInfo>().FromInstance(_playerData).AsSingle();
			Container.BindInstance(_playerData).AsSingle();
		}

		private void Awake()
		{
			foreach (var value in _playerDataValues)
				value.Initialize(_playerData);
		}
		
#if UNITY_EDITOR
		private void OnValidate()
		{
			_playerDataValues.MakeSlotsEmptyAndPreventDuplicates();
		}
#endif
	}
}