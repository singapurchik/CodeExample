using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using VInspector;

namespace FAS.Players
{
	public class PlayerRenderer : MonoBehaviour
	{
		[SerializeField] private List<Renderer> _renderers = new ();
		[SerializeField] private List<LODGroup> _lods = new ();
		
		private Coroutine _lodCrossFadeRoutine;
		
		private const float CROSS_FADE_DURATION = 0.5f;
		
		private void Awake()
		{
			foreach (var lod in _lods)
				lod.enabled = false;
		}

		public void Hide()
		{
			foreach (var renderer in _renderers)
				renderer.enabled = false;
			
			if (_lodCrossFadeRoutine != null)
				StopCoroutine(_lodCrossFadeRoutine);
		}

		public void Show()
		{
			foreach (var renderer in _renderers)
				renderer.enabled = true;

			if (Settings.CurrentGraphicsQuality != GraphicsQualityType.Low)
			{
				if (_lodCrossFadeRoutine != null)
					StopCoroutine(_lodCrossFadeRoutine);
				
				_lodCrossFadeRoutine = StartCoroutine(LODCrossFade());
			}
		}

		private IEnumerator LODCrossFade()
		{
			foreach (var lod in _lods)
			{
				lod.enabled = true;
				lod.ForceLOD(1);
			}
			
			yield return null;
			foreach (var lod in _lods)
				lod.ForceLOD(-1);
			
			yield return new WaitForSeconds(CROSS_FADE_DURATION);
			foreach (var lod in _lods)
				lod.enabled = false;
		}

#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_lods.Clear();
			_lods.AddRange(GetComponentsInChildren<LODGroup>(true));
			
			_renderers.Clear();
			
			foreach (var lodGroup in _lods)
				AddRenderersFromLODGroup(lodGroup);
		}

		private void AddRenderersFromLODGroup(LODGroup lodGroup)
		{
			var lods = lodGroup.GetLODs();
			if (lods.Length == 0) return;

			var lod0Renderers = lods[0].renderers;

			foreach (var renderer in lod0Renderers)
				_renderers.Add(renderer);
		}
#endif
	}
}