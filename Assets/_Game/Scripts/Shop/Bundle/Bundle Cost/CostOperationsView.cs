using System.Collections.Generic;
using System.Collections;
using Core;

namespace Shop
{
	public readonly struct CostOperationsView : IReadOnlyList<IPlayerDataOperationInfo>
	{
		private readonly IReadOnlyList<CostEntry> _src;
		public CostOperationsView(IReadOnlyList<CostEntry> src) => _src = src;
		public int Count => _src?.Count ?? 0;

		public IPlayerDataOperationInfo this[int index]
			=> _src[index]?.Operation as IPlayerDataOperationInfo;

		public Enumerator GetEnumerator() => new Enumerator(_src);

		IEnumerator<IPlayerDataOperationInfo> IEnumerable<IPlayerDataOperationInfo>.GetEnumerator()
			=> new Enumerator(_src);
		IEnumerator IEnumerable.GetEnumerator() => new Enumerator(_src);

		public struct Enumerator : IEnumerator<IPlayerDataOperationInfo>
		{
			private readonly IReadOnlyList<CostEntry> _list;
			private int _index;
			private IPlayerDataOperationInfo _current;

			public Enumerator(IReadOnlyList<CostEntry> list)
			{ _list = list; _index = -1; _current = null; }

			public bool MoveNext()
			{
				int next = _index + 1;
				if (_list != null && next < _list.Count)
				{
					_index = next;
					_current = _list[next]?.Operation as IPlayerDataOperationInfo;
					return true;
				}
				_current = null;
				return false;
			}

			public IPlayerDataOperationInfo Current => _current;
			object IEnumerator.Current => _current;
			public void Reset() { _index = -1; _current = null; }
			public void Dispose() { }
		}
	}
}