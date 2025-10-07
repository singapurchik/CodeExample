using System.Collections.Generic;
using UnityEngine;
using VInspector;
using FAS.Items;
using Zenject;

namespace FAS.Players
{
	public class UsableItemEffectsInstaller : MonoInstaller
	{
		[SerializeField] private InjectorPenEffect _injectorPenEffect;

		public override void InstallBindings()
		{
			var effects = new List<UsableItemEffect> { _injectorPenEffect };
			Container.BindInstance(effects).WhenInjectedInto<PlayerInventoryItemsUser>();
		}

#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_injectorPenEffect = GetComponentInChildren<InjectorPenEffect>(true);
		}
#endif
	}
}