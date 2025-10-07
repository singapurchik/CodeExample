using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

namespace FAS.UI
{
	public class ScrollingObject : MonoBehaviour
	{
		[SerializeField] private Image _image;
		[SerializeField] private Sprite _highPrioritySprite;
		[SerializeField] private Sprite _midPrioritySprite;
		[SerializeField] private Sprite _lowPrioritySprite;

		public ScrollingPoint.PriorityType CurrentPriorityType { get; private set; } = ScrollingPoint.PriorityType.Low;
		
		public UnityEvent OnEnterHighPriorityPoint;
		public UnityEvent OnExitHighPriorityPoint;
		
		public void ChangePoint(ScrollingPoint point)
		{
			ChangePriority(point.Priority);
			ChangeTransforms(point.transform);
		}

		private void ChangePriority(ScrollingPoint.PriorityType priority)
		{
			CurrentPriorityType = priority;
			
			switch (CurrentPriorityType)
			{
				case ScrollingPoint.PriorityType.High:
				default:
					_image.sprite = _highPrioritySprite;
					OnEnterHighPriorityPoint?.Invoke();
					break;
				case ScrollingPoint.PriorityType.Middle:
					_image.sprite = _midPrioritySprite;
					OnExitHighPriorityPoint?.Invoke();
					break;
				case ScrollingPoint.PriorityType.Low:
					_image.sprite = _lowPrioritySprite;
					OnExitHighPriorityPoint?.Invoke();
					break;
			}
		}

		private void ChangeTransforms(Transform target)
		{
			transform.position = target.position;
			transform.rotation = target.rotation;
			transform.localScale = target.localScale;
		}
	}
}