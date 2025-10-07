using UnityEngine.Events;
using UnityEngine;
using FAS.Sounds;
using FAS.Items;
using Zenject;
using System;
using VInspector;

namespace FAS
{
	public class LootContainer : LootHolder
	{
		[SerializeField] protected SoundEvent OpenSound;
		[SerializeField] private bool _isCanDropItem;
		[ShowIf(nameof(_isCanDropItem))]
		[SerializeField] private ItemType[] _randomDropItemTypes;
		[EndIf]

		[Inject] protected ISoundEffectsPlayer SoundEffects;
		[Inject] protected ItemDropper ItemDropper;

		public bool IsLooted { get; private set; }
		public bool IsCanDropItem => _isCanDropItem;

		public event Action<string> OnLooted;

		public UnityEvent OnLootedBefore;
		public UnityEvent OnRestored;
		
		public new void Accept(IInteractableVisitor visitor) => visitor.Apply(this);

		public bool TryDropItem()
		{
			if (ItemDropper.TryDropFromTypes(_randomDropItemTypes, out var item))
			{
				SetLoot(item);
				return true;
			}
			return false;
		}

		public virtual void SetLooted()
		{
			IsLooted = true;
			OnLootedBefore?.Invoke();
		}
		
		public virtual void Restore()
		{
			RemoveItem();
			OnRestored?.Invoke();
		}
		
		public virtual void Open()
		{
			if (OpenSound != null)
				SoundEffects.PlayOneShot(OpenSound);

			IsLooted = true;
		}
		
		protected void InvokeOnLooted() => OnLooted?.Invoke(UniqueId);
	}
}