using UnityEngine;
using Zenject;

namespace FAS.Transitions
{
	public class TransitionZone : MonoBehaviour, ITransitionZone
	{
		[SerializeField] private TransitionZoneCamera _camera;
		[SerializeField] private TransitionZoneData _data;
		
		public ITransitionZoneCamera Camera => _camera;
		public TransitionZoneData Data => _data;

		[Inject]
		public void Construct(DiContainer container)
		{
			container.Inject(_camera);
		}

		protected virtual void Awake()
		{
			_camera.Initialize();
		}
		
		public virtual void Accept(IInteractableVisitor visitor) => visitor.Apply(this);
	}
}