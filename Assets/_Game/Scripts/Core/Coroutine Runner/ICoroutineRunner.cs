using System.Collections;

namespace Core
{
	public interface ICoroutineRunner
	{
		public void Run(IEnumerator routine);
	}
}