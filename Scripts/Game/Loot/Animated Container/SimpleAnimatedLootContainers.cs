using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using VInspector;
using FAS;

public class SimpleAnimatedLootContainers : MonoBehaviour
{
	[SerializeField] private List<SimpleAnimatedLootContainer> _containers = new(3);

	private SimpleAnimatedLootContainer _currentContainer;
	
	private int _currentContainerIndex;

	public UnityEvent OnAllContainersLooted;
	public UnityEvent OnContainerChanged;

	private void Awake()
	{
		_currentContainerIndex = _containers.Count - 1;
	}
	
	private void OnEnable()
	{
		foreach (var container in _containers)
		{
			container.OnLootedBefore.AddListener(ForceChangeContainer);
			container.OnAnimComplete.AddListener(TryChangeContainer);
			container.OnRestored.AddListener(Restore);
		}
	}

	private void OnDisable()
	{
		foreach (var container in _containers)
		{
			container.OnLootedBefore.RemoveListener(ForceChangeContainer);
			container.OnAnimComplete.RemoveListener(TryChangeContainer);
			container.OnRestored.RemoveListener(Restore);
		}
	}

	private void TryChangeContainer()
	{
		if (_currentContainerIndex >= 0)
			OnContainerChanged?.Invoke();
	}

	public void Accept(IInteractableVisitor visitor)
	{
		_currentContainer = _containers[_currentContainerIndex];
		visitor.Apply(_currentContainer);
		ForceChangeContainer();
	}

	public void ForceChangeContainer()
	{
		_currentContainerIndex--;

		if (_currentContainerIndex < 0)
			OnAllContainersLooted?.Invoke();
	}

	private void Restore()
	{
		_currentContainer = null;
		_currentContainerIndex = _containers.Count - 1;
	}

#if UNITY_EDITOR
	[Button]
	private void FindContainers()
	{
		_containers.Clear();
		_containers.AddRange(GetComponentsInChildren<SimpleAnimatedLootContainer>(true));
	}
#endif
}
