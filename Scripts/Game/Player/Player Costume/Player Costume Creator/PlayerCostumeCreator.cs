#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace FAS.Players
{
	public class PlayerCostumeCreator : CharacterCostumeCreator
	{
		protected override void CreateCostume()
		{
			CreateLOD0();
		}

		public void CreateLOD0()
		{
			var lodGroup = GetComponent<LODGroup>();
			var lods = lodGroup.GetLODs();

			var list = new List<SkinnedMeshRenderer>(32);
			var all = GetComponentsInChildren<SkinnedMeshRenderer>(true);
			for (int i = 0; i < all.Length; i++)
				if (all[i] != null && all[i].GetComponentInParent<LODGroup>() == lodGroup)
					list.Add(all[i]);

			if (lods == null || lods.Length == 0)
			{
				lods = new[]
				{
					new LOD(0.6f, list.ToArray()),
					new LOD(0.3f, System.Array.Empty<Renderer>()),
					new LOD(0.1f, System.Array.Empty<Renderer>())
				};
			}
			else
			{
				lods[0].renderers = list.ToArray();
				NormalizeThresholds(ref lods);
			}

			lodGroup.SetLODs(lods);
			lodGroup.RecalculateBounds();
		}

		private static void NormalizeThresholds(ref LOD[] lods)
		{
			const float eps = 0.001f;
			if (lods.Length == 0) return;

			lods[0].screenRelativeTransitionHeight = Mathf.Clamp01(Mathf.Max(lods[0].screenRelativeTransitionHeight, eps));

			for (int i = 1; i < lods.Length; i++)
			{
				float maxAllowed = lods[i - 1].screenRelativeTransitionHeight - eps;
				float h = lods[i].screenRelativeTransitionHeight;
				h = Mathf.Clamp(h, 0f, Mathf.Max(0f, maxAllowed));
				lods[i].screenRelativeTransitionHeight = h;
			}
		}
	}
}
#endif