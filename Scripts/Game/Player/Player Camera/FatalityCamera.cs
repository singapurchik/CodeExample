using Cinemachine;
using UnityEngine;
using Zenject;

namespace FAS.Players
{
	public class FatalityCamera : MonoBehaviour
	{
		[Inject] private CinemachineTargetGroup _targetGroup;

		public void AddTarget(Transform newTarget, float weight = 1f, float radius = 2f)
		{
			var oldTargets = _targetGroup.m_Targets;
			var oldCount = oldTargets.Length;

			if (oldCount == 0)
			{
				_targetGroup.m_Targets = new CinemachineTargetGroup.Target[1];
				_targetGroup.m_Targets[0] = CreateTarget(newTarget, weight, radius);
			}
			else if (oldCount == 1)
			{
				var newTargets = new CinemachineTargetGroup.Target[2];
				newTargets[0] = oldTargets[0];
				newTargets[1] = CreateTarget(newTarget, weight, radius);
				_targetGroup.m_Targets = newTargets;
			}
			else
			{
				oldTargets[1] = CreateTarget(newTarget, weight, radius);
				_targetGroup.m_Targets = oldTargets;
			}
		}

		private CinemachineTargetGroup.Target CreateTarget(Transform target, float weight, float radius)
		{
			var newTarget = new CinemachineTargetGroup.Target
			{
				target = target,
				weight = weight,
				radius = radius
			};
			return newTarget;
		}
	}
}