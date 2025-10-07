using FAS.Actors.Companion.Animations;
using UnityEngine.Animations.Rigging;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using VInspector;
using Zenject;

namespace FAS.Actors.Companion
{
	[RequireComponent(typeof(Companion))]
	public class CompanionInstaller : MonoInstaller
	{
		[SerializeField] private List<ClothesWetness> _clothesWetnesses = new();
		[SerializeField] private CompanionCostumeChanger _companionCostumeChanger;
		[SerializeField] private CompanionAnimationRigging _animationRigging;
		[SerializeField] private CompanionClothesWetness _clothesWetness;
		[SerializeField] private Animator _animatorController;
		[SerializeField] private CompanionAnimator _animator;
		[SerializeField] private NavMeshAgent _navMeshAgent;
		[SerializeField] private CompanionRotator _rotator;
		[SerializeField] private RigBuilder _rigBuilder;
		[SerializeField] private CompanionMover _mover;
		[SerializeField] private Companion _companion;
		[SerializeField] private Pausable _pausable;
		
		public override void InstallBindings()
		{
			Container.BindInstance(_clothesWetnesses).WhenInjectedIntoInstance(_clothesWetness);
			Container.BindInstance(_animatorController).WhenInjectedInto<CompanionAnimator>();
			Container.BindInstance(_rigBuilder).WhenInjectedInto<CompanionAnimationRigging>();
			Container.BindInstance(_navMeshAgent).WhenInjectedInto<CompanionRotator>();
			Container.BindInstance(_navMeshAgent).WhenInjectedInto<CompanionMover>();
			Container.BindInstance(_animationRigging);
			Container.BindInstance(_rotator);
			Container.BindInstance(_mover);

			Container.BindInstance(_companionCostumeChanger).WhenInjectedIntoInstance(_companion);
			Container.Bind<ICompanionCostumeProxy>().FromInstance(_companionCostumeChanger).AsSingle();
			
			Container.Bind<ActorAnimator>().FromInstance(_animator).WhenInjectedInto<CompanionMover>();
			Container.BindInstance(_animator);
			
			CreateAndBindAnimationLayers();
			InjectPausables();
		}
		
		private void CreateAndBindAnimationLayers()
		{
			Container.Bind<IAnimatorDataChanger>().FromInstance(_animator)
				.WhenInjectedInto<CompanionCostumeChanger>();

			Container.Bind<IAnimatorDataSaver>().FromInstance(_animator).AsSingle();
			
			var triggersList = new List<ILayerWithTriggers>(10);
			var layersList = new List<Layer>(10);

			BindInstanceToGirlAnimator(new BaseLayer(_animatorController, 0, layersList, triggersList));
			BindInstanceToGirlAnimator(new LowerBodyLayer(_animatorController, 1, layersList, triggersList));
			BindInstanceToGirlAnimator(new UniqueStateLayer(_animatorController, 2, layersList, triggersList));
			BindInstanceToGirlAnimator(triggersList);
			BindInstanceToGirlAnimator(layersList);
		}
		
		private void BindInstanceToGirlAnimator<T>(T instance)
			=> Container.BindInstance(instance).WhenInjectedInto<CompanionAnimator>();
		
		private void InjectPausables()
		{
			var list = new List<IPausable>(10)
			{
				_animator,
			};
			Container.BindInstance(list).WhenInjectedIntoInstance(_pausable);
			Container.Bind<IPausableInfo>().FromInstance(_pausable).AsSingle();
		}
		
#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_clothesWetnesses.Clear();
			_clothesWetnesses.AddRange(GetComponentsInChildren<ClothesWetness>(true));
			
			_companionCostumeChanger = GetComponentInChildren<CompanionCostumeChanger>(true);
			_animationRigging = GetComponentInChildren<CompanionAnimationRigging>(true);
			_clothesWetness = GetComponentInChildren<CompanionClothesWetness>(true);
			_animatorController = GetComponentInChildren<Animator>(true);
			_navMeshAgent = GetComponentInChildren<NavMeshAgent>(true);
			_animator = GetComponentInChildren<CompanionAnimator>(true);
			_rotator = GetComponentInChildren<CompanionRotator>(true);
			_rigBuilder = GetComponentInChildren<RigBuilder>(true);
			_mover = GetComponentInChildren<CompanionMover>(true);
			_companion = GetComponentInChildren<Companion>(true);
			_pausable = GetComponentInChildren<Pausable>(true);
		}
#endif
	}
}