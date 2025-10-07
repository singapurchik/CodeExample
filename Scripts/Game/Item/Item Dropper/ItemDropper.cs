using UnityEngine;
using System;

namespace FAS.Items
{
	public sealed class ItemDropper : MonoBehaviour, IItemDropBonus
	{
		[Serializable]
		struct DropEntry
		{
			public ItemData Item;
			[Range(0, 1f)] public float BaseChance;
		}

		[SerializeField] private bool EnableVerboseLogs = false;
		[SerializeField] private DropEntry[] Entries = Array.Empty<DropEntry>();

		private ItemData[] Items = Array.Empty<ItemData>();
		
		private float[] BaseChance = Array.Empty<float>();
		private float[] CurrentChance = Array.Empty<float>();
		private float[] FirstAttemptChance = Array.Empty<float>();
		private double[] IncrementPerFail = Array.Empty<double>();
		private int[] FailedAttempts = Array.Empty<int>();
		private float[] AdditiveBonus = Array.Empty<float>();
		private float[] MultiplicativeBonus = Array.Empty<float>();
		private int[] ItemTypeToIndex = Array.Empty<int>();
		private int RoundRobinCursor;

		private void Awake()
		{
			int count = Entries.Length;

			Items = new ItemData[count];
			BaseChance = new float[count];
			CurrentChance = new float[count];
			FirstAttemptChance = new float[count];
			IncrementPerFail = new double[count];
			FailedAttempts = new int[count];
			AdditiveBonus = new float[count];
			MultiplicativeBonus = new float[count];

			int maxTypeValue = 0;
			for (int i = 0; i < count; i++)
			{
				int value = (int)Entries[i].Item.Type;
				if (value > maxTypeValue) maxTypeValue = value;
			}
			ItemTypeToIndex = new int[maxTypeValue + 1];
			for (int i = 0; i <= maxTypeValue; i++) ItemTypeToIndex[i] = -1;

			for (int i = 0; i < count; i++)
			{
				ItemData item = Entries[i].Item;
				ItemType itemType = Entries[i].Item.Type;
				float baseProbability = Mathf.Clamp01(Entries[i].BaseChance);

				Items[i] = item;
				BaseChance[i] = baseProbability;
				FailedAttempts[i] = 0;
				AdditiveBonus[i] = 0f;
				MultiplicativeBonus[i] = 0f;

				if (ItemTypeToIndex[(int)itemType] == -1)
					ItemTypeToIndex[(int)itemType] = i;

				double coefficient = PseudoRandomDistribution.GetValveCoefficientCached(baseProbability);
				IncrementPerFail[i] = coefficient;
				FirstAttemptChance[i] = Clamp01((float)coefficient);
				CurrentChance[i] = FirstAttemptChance[i];
			}
		}

		public bool TryDropByType(ItemType itemType, out ItemData droppedItem)
		{
			int index = ((int)itemType < ItemTypeToIndex.Length) ? ItemTypeToIndex[(int)itemType] : -1;
			if (index < 0) { droppedItem = null; return false; }

			float baseChanceThisAttempt = CurrentChance[index];
			float effectiveChance = baseChanceThisAttempt * (1f + MultiplicativeBonus[index]) + AdditiveBonus[index];
			if (effectiveChance > 1f) effectiveChance = 1f;
			if (effectiveChance < 0f) effectiveChance = 0f;

			bool success = UnityEngine.Random.value < effectiveChance;

			if (success)
			{
				FailedAttempts[index] = 0;
				CurrentChance[index] = FirstAttemptChance[index];
				droppedItem = Items[index];

				if (EnableVerboseLogs)
					Debug.Log($"[Drop] {itemType} | base={(BaseChance[index]*100f):F2}%" +
					          $"| fails=0 | used={(effectiveChance*100f):F2}%" +
					          $"| next={(CurrentChance[index]*100f):F2}%" +
					          $"| result=SUCCESS");

				return true;
			}
			else
			{
				FailedAttempts[index] += 1;
				float nextChance = baseChanceThisAttempt + (float)IncrementPerFail[index];
				CurrentChance[index] = nextChance >= 1f ? 1f : nextChance;
				droppedItem = null;

				if (EnableVerboseLogs)
					Debug.Log($"[Drop] {itemType} | base={(BaseChance[index]*100f):F2}%" +
					          $"| fails={FailedAttempts[index]} | used={(effectiveChance*100f):F2}%" +
					          $"| next={(CurrentChance[index]*100f):F2}% | result=FAIL");

				return false;
			}
		}

		public bool TryDropFromTypes(ItemType[] allowedTypes, out ItemData droppedItem)
		{
			int length = allowedTypes.Length;
			int startIndex = RoundRobinCursor;
			RoundRobinCursor = (startIndex + 1 == length) ? 0 : startIndex + 1;

			for (int offset = 0; offset < length; offset++)
			{
				int cursor = startIndex + offset;
				if (cursor >= length) cursor -= length;

				int itemTypeIndexValue = (int)allowedTypes[cursor];
				int entryIndex = (itemTypeIndexValue < ItemTypeToIndex.Length) ? ItemTypeToIndex[itemTypeIndexValue] : -1;
				if (entryIndex < 0) continue;

				float baseChanceThisAttempt = CurrentChance[entryIndex];
				float effectiveChance = baseChanceThisAttempt * (1f + MultiplicativeBonus[entryIndex]) + AdditiveBonus[entryIndex];
				if (effectiveChance > 1f) effectiveChance = 1f;
				if (effectiveChance < 0f) effectiveChance = 0f;

				bool success = UnityEngine.Random.value < effectiveChance;

				if (success)
				{
					FailedAttempts[entryIndex] = 0;
					CurrentChance[entryIndex] = FirstAttemptChance[entryIndex];
					droppedItem = Items[entryIndex];

					if (EnableVerboseLogs)
						Debug.Log($"[Drop*] {allowedTypes[cursor]}" +
						          $"| base={(BaseChance[entryIndex]*100f):F2}% | fails=0" +
						          $"| used={(effectiveChance*100f):F2}%| next={(CurrentChance[entryIndex]*100f):F2}%" +
						          $"| result=SUCCESS");

					return true;
				}
				else
				{
					FailedAttempts[entryIndex] += 1;
					float nextChance = baseChanceThisAttempt + (float)IncrementPerFail[entryIndex];
					CurrentChance[entryIndex] = nextChance >= 1f ? 1f : nextChance;

					if (EnableVerboseLogs)
						Debug.Log($"[Drop*] {allowedTypes[cursor]}" +
						          $"| base={(BaseChance[entryIndex]*100f):F2}%" +
						          $"| fails={FailedAttempts[entryIndex]}" +
						          $"| used={(effectiveChance*100f):F2}%" +
						          $"| next={(CurrentChance[entryIndex]*100f):F2}%" +
						          $"| result=FAIL");
				}
			}

			droppedItem = null;
			return false;
		}

		public void SetAdditiveBonus(ItemType itemType, float bonus)
		{
			int index = ((int)itemType < ItemTypeToIndex.Length) ? ItemTypeToIndex[(int)itemType] : -1;
			if (index < 0) return;
			AdditiveBonus[index] = bonus;
		}

		public void SetMultiplicativeBonus(ItemType itemType, float multiplier)
		{
			int index = ((int)itemType < ItemTypeToIndex.Length) ? ItemTypeToIndex[(int)itemType] : -1;
			if (index < 0) return;
			MultiplicativeBonus[index] = multiplier;
		}

		public void ClearBonuses(ItemType itemType)
		{
			int index = ((int)itemType < ItemTypeToIndex.Length) ? ItemTypeToIndex[(int)itemType] : -1;
			if (index < 0) return;
			AdditiveBonus[index] = 0f;
			MultiplicativeBonus[index] = 0f;
		}

		public float GetCurrentChance(ItemType itemType)
		{
			int index = ((int)itemType < ItemTypeToIndex.Length) ? ItemTypeToIndex[(int)itemType] : -1;
			return (index < 0) ? 0f : CurrentChance[index];
		}

		public int GetFailedAttempts(ItemType itemType)
		{
			int index = ((int)itemType < ItemTypeToIndex.Length) ? ItemTypeToIndex[(int)itemType] : -1;
			return (index < 0) ? 0 : FailedAttempts[index];
		}

		public void ResetState(ItemType itemType)
		{
			int index = ((int)itemType < ItemTypeToIndex.Length) ? ItemTypeToIndex[(int)itemType] : -1;
			if (index < 0) return;
			FailedAttempts[index] = 0;
			CurrentChance[index] = FirstAttemptChance[index];
		}

		static float Clamp01(float value) => value <= 0f ? 0f : (value >= 1f ? 1f : value);
	}
}