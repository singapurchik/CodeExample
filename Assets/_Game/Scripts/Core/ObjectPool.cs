using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Core
{
	public abstract class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
	{
		[SerializeField] private List<T> _objects = new();
		[SerializeField] private T _prefab;

		[Inject] private DiContainer _diContainer;
		
		private readonly Dictionary<T, float> _lastUseTime = new ();
		private readonly Queue<T> _pool = new();

		protected virtual void Awake()
		{
			foreach (var obj in _objects)
			{
				_pool.Enqueue(obj);
				InitializeObject(obj);
				obj.gameObject.SetActive(false);
				_lastUseTime[obj] = -1f;
			}
		}

		protected virtual void OnDisable()
		{
			foreach (var obj in _objects)
				CleanupObject(obj);
		}

		public virtual T Get(bool isInstantiateIfNeeded = true)
		{
			T obj;

			if (_pool.Any())
			{
				obj = _pool.Dequeue();
			}
			else if (isInstantiateIfNeeded)
			{
				obj = _diContainer.InstantiatePrefabForComponent<T>(_prefab, transform);
				_objects.Add(obj);
				InitializeObject(obj);
			}
			else
			{
				obj = GetOldestActiveObject();
			}

			obj.gameObject.SetActive(true);
			_lastUseTime[obj] = Time.time;
			return obj;
		}

		protected virtual void ReturnToPool(T obj)
		{
			obj.transform.SetParent(transform, false);
			obj.gameObject.SetActive(false);
			_pool.Enqueue(obj);
		}

		private T GetOldestActiveObject()
		{
			T oldest = null;
			float oldestTime = float.MaxValue;

			for (var i = 0; i < _objects.Count; i++)
			{
				var obj = _objects[i];

				if (obj.gameObject.activeSelf)
				{
					if (_lastUseTime.TryGetValue(obj, out var usedAt))
					{
						if (usedAt < oldestTime)
						{
							oldestTime = usedAt;
							oldest = obj;
						}
					}	
				}
			}

			return oldest;
		}

		protected abstract void InitializeObject(T obj);
		protected abstract void CleanupObject(T obj);
	}
}