using System.Collections.Generic;
using UnityEngine;
using FAS.Sounds;
using System;

namespace FAS
{
	[Serializable]
	public struct MonologueString
	{
		public string Text;
		public SoundEvent Voice;
		public Sounds.AudioType AudioType;
	}
	
	[CreateAssetMenu(fileName = "Monologue Data", menuName = "FAS/Monologue Data", order = 0)]
	public class MonologueData : ScriptableObject
	{
		[SerializeField] private List<MonologueString> _monologueStrings = new(0);

		public List<MonologueString> MonologueStrings => _monologueStrings;
	}
}