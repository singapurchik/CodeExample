using System.Collections;
using UnityEngine;

namespace FAS
{
	[RequireComponent(typeof(CapsuleCollider))]
	public class Flies : MonoBehaviour, IInteractableVisitable
	{
		[SerializeField] private AudioSource _audioSource;
		[SerializeField] private AudioClip _aggressiveSound;
		[SerializeField] private AudioClip _calmSound;
		[SerializeField] private ParticleSystem _effect;
		[SerializeField] private float _minEffectRadius = 0.5f;
		[SerializeField] private float _maxEffectRadius = 3f;
		[SerializeField] private float _setMaxRadiusDelay = 2f;
    
		private ParticleSystem.ForceOverLifetimeModule _force;
		private ParticleSystem.ShapeModule _shape;
		private CapsuleCollider _collider;

		private Coroutine _currentSetCalmStateRoutine;

		private void Awake()
		{
			_collider = GetComponent<CapsuleCollider>();
		
			_force = _effect.forceOverLifetime;
			_shape = _effect.shape;
		}
		
		public void ChangeSpatialBlend(float blend) => _audioSource.spatialBlend = blend;

		private void SetAggressiveState()
		{
			if (_currentSetCalmStateRoutine != null)
				StopCoroutine(_currentSetCalmStateRoutine);
		
			_audioSource.clip = _aggressiveSound;
			_audioSource.Play();
			_collider.enabled = true;
			_shape.radius = _minEffectRadius;
			_force.enabled = false;
		}

		public void SetCalmState()
		{
			if (_currentSetCalmStateRoutine != null)
				StopCoroutine(_currentSetCalmStateRoutine);

			_audioSource.clip = _calmSound;
			_audioSource.Play();
			_currentSetCalmStateRoutine = StartCoroutine(SetCalmStateRoutine());
		}
	
		private IEnumerator SetCalmStateRoutine()
		{
			_collider.enabled = false;
			_force.enabled = true;
			yield return new WaitForSeconds(_setMaxRadiusDelay);
			_shape.radius = _maxEffectRadius;
		}
		
		public void Accept(IInteractableVisitor visitor) => visitor.Apply(this);
	}
}