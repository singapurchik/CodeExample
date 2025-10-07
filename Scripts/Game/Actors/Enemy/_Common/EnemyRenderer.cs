using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FAS.Actors.Emenies
{
	public class EnemyRenderer : MonoBehaviour
	{
		[Inject] private IReadOnlyList<Renderer> _renderers;
		
		private int _defaultLayer;

		private void Awake()
		{
			_defaultLayer = _renderers[0].gameObject.layer;
		}

		public void EnableOutline()
		{
			for (int i = 0; i < _renderers.Count; i++)
				_renderers[i].gameObject.layer = GameObjectLayer.OUTLINE;
		}

		public void DisableOutline()
		{
			for (int i = 0; i < _renderers.Count; i++)
				_renderers[i].gameObject.layer = _defaultLayer;
		}
	}
}