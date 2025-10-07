using System.Collections.Generic;
using UnityEngine;
using System;

public class SignalBus
{
	private readonly Dictionary<Type, List<Delegate>> _subscribers = new();

	public void Subscribe<TSignal>(Action<TSignal> callback)
	{
		var type = typeof(TSignal);
		if (!_subscribers.ContainsKey(type))
		{
			_subscribers[type] = new List<Delegate>();
		}
		_subscribers[type].Add(callback);
	}

	public void Unsubscribe<TSignal>(Action<TSignal> callback)
	{
		var type = typeof(TSignal);
		if (_subscribers.TryGetValue(type, out var callbacks))
		{
			callbacks.Remove(callback);
			if (callbacks.Count == 0)
			{
				_subscribers.Remove(type);
			}
		}
	}

	public void Fire<TSignal>(TSignal signal)
	{
		if (_subscribers.TryGetValue(typeof(TSignal), out var callbacks))
		{
			var callbacksCopy = new List<Delegate>(callbacks);

			foreach (var callback in callbacksCopy)
			{
				try
				{
					(callback as Action<TSignal>)?.Invoke(signal);
				}
				catch (Exception ex)
				{
					Debug.LogError($"Error while executing signal callback: {ex.Message}");
				}
			}
		}
	}
}