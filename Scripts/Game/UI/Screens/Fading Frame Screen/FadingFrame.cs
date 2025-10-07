using UnityEngine;

namespace FAS.UI
{
	[RequireComponent(typeof(Animator))]
	public class FadingFrame : MonoBehaviour
	{
		private Animator _animator;
		
		private readonly int _showAnimHash = Animator.StringToHash("Show");

		private void Awake() => _animator = GetComponent<Animator>();

		public void Play() => _animator.Play(_showAnimHash, 0, 0f);
	}
}