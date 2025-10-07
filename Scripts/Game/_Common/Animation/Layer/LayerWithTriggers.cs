using System.Collections.Generic;
using UnityEngine;

namespace FAS
{
	public abstract class LayerWithTriggers : Layer, ILayerWithTriggers
	{
		protected LayerWithTriggers(Animator animator, int index, List<Layer> list, List<ILayerWithTriggers> triggersLit)
			: base(animator, index, list)
		{
			triggersLit.Add(this);
		}

		public abstract void ResetTriggers();
	}
}