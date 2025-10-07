using System.Collections.Generic;
using UnityEngine;
using VInspector;

namespace FAS
{
	public class Damageable : MonoBehaviour, IRangeWeaponTarget
	{
		[SerializeField] private ZoneType _zoneType;
		[SerializeField] private DamageableHealth _health;
		[SerializeField] private List<Renderer> _renderers = new ();

		public ZoneType ZoneType => _zoneType;
		
		private int _defaultLayer;
		
		public Vector3 AimingPosition => transform.position;
		public Vector3 Position => transform.position;

		private void Awake()
		{
			_defaultLayer = _renderers[0].gameObject.layer;
		}

		public void Deselect()
		{
			foreach (var renderer in _renderers)
				renderer.gameObject.layer = _defaultLayer;
			
			_health.PredictHealth(0);
			_health.HideView();
		}

		public void Select()
		{
			foreach (var renderer in _renderers)
				renderer.gameObject.layer = GameObjectLayer.OUTLINE;
			
			_health.ShowView();
			//_health.PredictHealth();
		}

		public void PredictHealth(float damage)
		{
			_health.PredictHealth(damage);
		}
		
#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_renderers.Clear();
			_renderers.AddRange(GetComponentsInChildren<Renderer>(true));
		}
#endif
	}
}