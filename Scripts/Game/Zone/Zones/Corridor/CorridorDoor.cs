using UnityEngine;
using System;
using TMPro;

namespace FAS.Corridor
{
	public class CorridorDoor : MonoBehaviour
	{
		[SerializeField] private int _number;
		[SerializeField] private TextMeshPro _numberText;
		
		public int Number => _number;
		
		public event Action<int> OnEnter;
		
		public void Enter() => OnEnter?.Invoke(_number);
		
		public void SetNumber(int number)
		{
			_number = number;
			_numberText.text = _number.ToString();
		}
	}
}