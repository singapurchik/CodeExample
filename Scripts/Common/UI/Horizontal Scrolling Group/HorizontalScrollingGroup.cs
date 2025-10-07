using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VInspector;

namespace FAS.UI
{
    public class HorizontalScrollingGroup : MonoBehaviour
    {
        [SerializeField] private List<ScrollingPoint> _scrollingPoints;
        [SerializeField] private List<ScrollingObject> _scrollingObjects;
        [SerializeField] private CustomButton _leftButton;
        [SerializeField] private CustomButton _rightButton;

        private int _currentOffset;

        private void OnEnable()
        {
            _rightButton.OnClick.AddListener(ScrollRight);
            _leftButton.OnClick.AddListener(ScrollLeft);
        }

        private void OnDisable()
        {
            _rightButton.OnClick.RemoveListener(ScrollRight);
            _leftButton.OnClick.RemoveListener(ScrollLeft);
        }

        private void Start()
        {
            UpdatePositions();
        }

        private void ScrollLeft()
        {
	        _currentOffset = (_currentOffset - 1 + _scrollingObjects.Count) % _scrollingObjects.Count;
            UpdatePositions();
        }

        private void ScrollRight()
        {
            _currentOffset = (_currentOffset + 1) % _scrollingObjects.Count;
            UpdatePositions();
        }

        private void UpdatePositions()
        {
            for (int i = 0; i < _scrollingObjects.Count; i++)
            {
                int pointIndex = (i + _currentOffset) % _scrollingObjects.Count;

                if (i < _scrollingPoints.Count)
                {
                    _scrollingObjects[pointIndex].gameObject.SetActive(true);
                    _scrollingObjects[pointIndex].ChangePoint(_scrollingPoints[i]);
                }
                else
                {
                    _scrollingObjects[pointIndex].gameObject.SetActive(false);
                }
            }
        }
        
        public ScrollingObject GetPrioritizedScrollingObject() => _scrollingObjects.First(scrollingObject =>
	        scrollingObject.CurrentPriorityType == ScrollingPoint.PriorityType.High);

#if UNITY_EDITOR
        [Button]
        private void FindDependencies()
        {
            _scrollingObjects.Clear();
            _scrollingPoints.Clear();

            _scrollingObjects.AddRange(GetComponentsInChildren<ScrollingObject>(true));
            _scrollingPoints.AddRange(GetComponentsInChildren<ScrollingPoint>(true));
        }
#endif
    }
}
