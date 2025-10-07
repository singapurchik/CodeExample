using System.Collections.Generic;
using UnityEngine;

namespace FAS
{
	[RequireComponent(typeof(BoxCollider))]
	public abstract class ReliableTrigger<T> : MonoBehaviour where T : class
	{
		[SerializeField] private LayerMask _layerMask = ~0;

		private readonly HashSet<Collider> _collidersInside = new();
		private readonly Dictionary<Collider, T> _targets = new();
		private readonly HashSet<Collider> _currentOverlaps = new();

		private static readonly Collider[] _overlapBuffer = new Collider[128];
		private Collider _triggerCollider;

		protected Dictionary<Collider, T>.ValueCollection Targets => _targets.Values;
		protected int TargetCount => _targets.Count;

		private static class ListPool<TItem>
		{
			private static readonly Stack<List<TItem>> _pool = new();
			public static List<TItem> Get() => _pool.Count > 0 ? _pool.Pop() : new List<TItem>(16);
			public static void Release(List<TItem> list) { list.Clear(); _pool.Push(list); }
		}

		protected virtual void Awake()
		{
			_triggerCollider = GetComponent<Collider>();
			_triggerCollider.isTrigger = true;
		}

		private void OnDisable()
		{
			if (_collidersInside.Count == 0) return;

			foreach (var collider in _collidersInside)
				if (_targets.TryGetValue(collider, out var target))
					OnExit(target, collider);

			_collidersInside.Clear();
			_targets.Clear();
		}

		private void OnTriggerEnter(Collider other)
		{
			if (TryResolveTarget(other, out var target) && _collidersInside.Add(other))
			{
				_targets[other] = target;
				OnEnter(target, other);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (_collidersInside.Remove(other) && _targets.TryGetValue(other, out var target))
			{
				OnExit(target, other);
				_targets.Remove(other);
			}
		}

		private void SyncOverlaps()
		{
			int count = QueryOverlaps(_overlapBuffer);

			_currentOverlaps.Clear();
			for (int i = 0; i < count; i++)
			{
				var collider = _overlapBuffer[i];
				_currentOverlaps.Add(collider);

				if (!_collidersInside.Contains(collider) && TryResolveTarget(collider, out var target))
				{
					_collidersInside.Add(collider);
					_targets[collider] = target;
					OnEnter(target, collider);
				}
			}

			var toRemove = ListPool<Collider>.Get();
			foreach (var collider in _collidersInside)
				if (!_currentOverlaps.Contains(collider))
					toRemove.Add(collider);

			for (int i = 0; i < toRemove.Count; i++)
			{
				var collider = toRemove[i];
				if (_targets.TryGetValue(collider, out var target))
					OnExit(target, collider);

				_collidersInside.Remove(collider);
				_targets.Remove(collider);
			}

			ListPool<Collider>.Release(toRemove);
		}

		private int QueryOverlaps(Collider[] buffer)
		{
			if (_triggerCollider is BoxCollider box)
			{
				var center = box.bounds.center;
				var halfExtents = Vector3.Scale(box.size * 0.5f, transform.lossyScale);

				return Physics.OverlapBoxNonAlloc(
					center,
					halfExtents,
					buffer,
					transform.rotation,
					_layerMask,
					QueryTriggerInteraction.Collide
				);
			}

			var bounds = _triggerCollider.bounds;
			return Physics.OverlapBoxNonAlloc(
				bounds.center,
				bounds.extents,
				buffer,
				transform.rotation,
				_layerMask,
				QueryTriggerInteraction.Collide
			);
		}

		protected virtual bool TryResolveTarget(Collider other, out T target)
		{
#if UNITY_2020_1_OR_NEWER
			if (other.TryGetComponent(out target))
				return true;
#else
			target = other.GetComponent(typeof(T)) as T;
			if (target != null) return true;
#endif
			target = other.GetComponentInParent(typeof(T)) as T;
			return target != null;
		}

		protected virtual void OnEnter(T target, Collider other) { }
		protected virtual void OnExit(T target, Collider other) { }
		
		private void FixedUpdate()
		{
			SyncOverlaps();
		}
	}
}