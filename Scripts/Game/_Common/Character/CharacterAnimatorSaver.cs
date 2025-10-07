using System.Collections.Generic;
using UnityEngine;

namespace FAS
{
	public class CharacterAnimatorSaver
	{
		private readonly Dictionary<int, bool> _savedBools = new (8);
		private readonly List<int> _boolsHashes = new (8);
		private readonly Animator _animator;

		public CharacterAnimatorSaver(Animator animator)
		{
			_animator =  animator;
			BuildBools();
		}
		
		private void BuildBools()
		{
			_boolsHashes.Clear();

			var animatorParameters = _animator.parameters;
			
			for (int i = 0; i < animatorParameters.Length; i++)
				if (animatorParameters[i].type == AnimatorControllerParameterType.Bool)
					_boolsHashes.Add(animatorParameters[i].nameHash);
		}

		public void SaveData(IReadOnlyList<Layer> layers)
		{
			SaveBools();
			
			foreach (var layer in layers)
				layer.SaveData();
		}
		
		public void LoadData(IReadOnlyList<Layer> layers)
		{
			LoadBools();
			
			foreach (var layer in layers)
				if (layer.IsHasSavedData)
					layer.RestoreData();
		}
		
		private void SaveBools()
		{
			_savedBools.Clear();
			for (int i = 0; i < _boolsHashes.Count; i++)
			{
				int currentHash = _boolsHashes[i];
				_savedBools[currentHash] = _animator.GetBool(currentHash);
			}
		}
		
		private void LoadBools()
		{
			if (_savedBools.Count > 0)
				foreach (var kv in _savedBools)
					_animator.SetBool(kv.Key, kv.Value);
		}
	}
}