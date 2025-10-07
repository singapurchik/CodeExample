using UnityEngine.Animations.Rigging;
using UnityEngine;
using Zenject;

namespace FAS.Actors.Companion
{
	public class CompanionAnimationRigging : MonoBehaviour
	{
		[Inject] private ICompanionCostumeProxy _costumeProxy;
		[Inject] private RigBuilder _rigBuilder;

		private readonly MultiRotationConstraintWrapper _spineRig = new ();
		private readonly MultiRotationConstraintWrapper _headRig = new ();

		private void Awake()
		{
			_costumeProxy.OnCostumeChanged += ChangeData;
		}

		private void OnDestroy()
		{
			_costumeProxy.OnCostumeChanged -= ChangeData;
		}

		private void ChangeData(ICompanionCostumeData data)
		{
			_spineRig.SetData(data.SpineRigData);
			_headRig.SetData(data.HeadRigData);
			
			_rigBuilder.layers.Clear();
			_rigBuilder.layers.Add(new RigLayer(data.Rig));
			_rigBuilder.Clear();
			_rigBuilder.Build();

			_spineRig.ForceUpdateWeight();
			_headRig.ForceUpdateWeight();
		}

		public void RequestEnabledSpineRig(float angle) => _spineRig.RequestEnable(angle);
		
		public void RequestEnabledHeadRig(float angle) => _headRig.RequestEnable(angle);

		private void Update()
		{
			_spineRig.Update();
			_headRig.Update();
		}
	}
}