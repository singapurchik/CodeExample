using System.Collections.Generic;
using UnityEngine;
using System;

namespace FAS
{
	public abstract class ScenarioPlayer<TEnum> : MonoBehaviour where TEnum : struct, Enum
	{
		[SerializeField] protected List<Scenario<TEnum>> ScenariosList = new ();

		private readonly Dictionary<TEnum, Scenario<TEnum>> _scenarios = new (10);
		
		private Scenario<TEnum> _currentScenario;

		public TEnum CurrentScenarioType => _currentScenario.Type;
		
		public bool IsScenarioPlaying => _currentScenario != null;

		private void Start()
		{
			foreach (var scenario in ScenariosList)
			{
				_scenarios.Add(scenario.Type, scenario);
				scenario.TryDisassemble();
			}
		}

		public void TryChangeScenario(TEnum type)
		{
			if (_scenarios.TryGetValue(type, out var nextScenario))
			{
				if (_currentScenario == null)
				{
					_currentScenario = nextScenario;
				}
				else if (_currentScenario != nextScenario)
				{
					_currentScenario.TryDisassemble();
					_currentScenario = nextScenario;
				}
			}
		}

		public void TryAssembleScenario()
		{
			if (_currentScenario != null && !_currentScenario.IsActive)
				_currentScenario.TryAssemble();
		}

		public void TryDisassembleScenario()
		{
			if (_currentScenario != null && _currentScenario.IsActive)
			{
				_currentScenario.TryDisassemble();
				_currentScenario = null;
			}
		}
	}
}