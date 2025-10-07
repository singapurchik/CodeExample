using System.Collections.Generic;

namespace FAS
{
	internal sealed class ResettableQueue<T>
	{
		private readonly Queue<T> _queue;
		
		private bool _resetOnNextEnqueue;

		public int Count => _queue.Count;
		
		public ResettableQueue(int capacity = 10)
		{
			_queue = new Queue<T>(capacity);
		}

		public void Enqueue(T item, bool resetAfterThis = false)
		{
			if (_resetOnNextEnqueue)
				_queue.Clear();

			_queue.Enqueue(item);
			_resetOnNextEnqueue = resetAfterThis;
		}

		public bool TryDequeue(out T item)
		{
			if (_queue.Count == 0)
			{
				item = default;
				return false;
			}
			
			item = _queue.Dequeue();
			return true;
		}

		public void Clear() => _queue.Clear();
	}
}