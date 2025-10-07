#if UNITY_EDITOR
using System.Collections.Generic;
using FAS.Apartments.Outside;
using FAS.Apartments;
using UnityEngine;
using VInspector;

public class ApartmentOutsideDataAssigner : MonoBehaviour
{
	[SerializeField] private List<ApartmentInsideData> _dataPool = new ();
	
	[Button]
	private void SetData()
	{
		List<ApartmentOutside> _apartmentOutsides = new();
		_apartmentOutsides.AddRange(FindObjectsOfType<ApartmentOutside>(true));

		if (_apartmentOutsides.Count != _dataPool.Count)
		{
			Debug.LogError($"Count mismatch: {_apartmentOutsides.Count} ApartmentOutside vs {_dataPool.Count} data items.");
			return;
		}

		var shuffledData = new List<ApartmentInsideData>(_dataPool);
		for (int i = 0; i < shuffledData.Count; i++)
		{
			int j = Random.Range(i, shuffledData.Count);
			(shuffledData[i], shuffledData[j]) = (shuffledData[j], shuffledData[i]);
		}

		var field = typeof(ApartmentOutside)
			.GetField("_insideData", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

		if (field == null)
		{
			Debug.LogError("Field '_insideData' not found on ApartmentOutside.");
			return;
		}

		for (int i = 0; i < _apartmentOutsides.Count; i++)
		{
			var outside = _apartmentOutsides[i];
			var data = shuffledData[i];

			UnityEditor.Undo.RecordObject(outside, "Assign ApartmentInsideData");
			field.SetValue(outside, data);
			UnityEditor.EditorUtility.SetDirty(outside);
		}

		Debug.Log("Successfully assigned unique ApartmentInsideData to all ApartmentOutside objects.");
	}

}
#endif
