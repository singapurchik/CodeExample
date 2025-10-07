using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VInspector;

namespace FAS
{
	public abstract class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
	{
		[SerializeField] private List<T> _objects = new();
		[SerializeField] private T _prefab;

		private readonly Queue<T> _pool = new();

		protected virtual void Awake()
		{
			foreach (var obj in _objects)
			{
				_pool.Enqueue(obj);
				InitializeObject(obj);
			}
		}

		protected virtual void OnDisable()
		{
			if (_pool.Any())
				foreach (var obj in _objects)
					CleanupObject(obj);
		}

		public virtual T Get()
		{
			T obj;

			if (_pool.Any())
			{
				obj = _pool.Dequeue();
			}
			else
			{
				obj = Instantiate(_prefab, transform);
				_objects.Add(obj);
				InitializeObject(obj);
			}

			obj.gameObject.SetActive(true);
			return obj;
		}

		protected virtual void ReturnToPool(T obj)
		{
			obj.gameObject.SetActive(false);
			_pool.Enqueue(obj);
		}

		protected abstract void InitializeObject(T text);
		protected abstract void CleanupObject(T obj);
		
#if UNITY_EDITOR
		[Button]
		private void FindObjects()
		{
			_objects.Clear();
			_objects.AddRange(GetComponentsInChildren<T>());
		}
#endif
	}
}