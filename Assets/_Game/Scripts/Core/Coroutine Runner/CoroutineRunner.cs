using System.Collections;
using UnityEngine;

namespace Core
{
	public sealed class CoroutineRunner : MonoBehaviour, ICoroutineRunner
	{
		public void Run(IEnumerator routine) => StartCoroutine(routine);
	}
}