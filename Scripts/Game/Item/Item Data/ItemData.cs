using System.Collections.Generic;
using FAS.Players;
using UnityEngine;
using FAS.Sounds;
using VInspector;

namespace FAS.Items
{
	[CreateAssetMenu(fileName = "Item", menuName = "FAS/Item Data/Item", order = 0)]
	public class ItemData : BaseItemData
	{
		[SerializeField] private ItemType _type;
		[SerializeField] private bool _isStackable;
		[SerializeField] private bool _isCanEquipFromInventory;
		[SerializeField] private bool _isCanUseFromInventory;
		[SerializeField] private bool _isUsingInInteraction;
		[ShowIf(nameof(_isUsingInInteraction))]
		[SerializeField] private SoundEvent _interactionUseSound;
		[SerializeField] private List<TMPCustomText> _interactionUseText;
		[ReadOnly, SerializeField] private string _buildedInteractionUseText;
		[EndIf]
		
		public override SoundEvent InteractionUseSound => _interactionUseSound;
		public override ItemType Type => _type;

		public override bool IsCanEquipFromInventory => _isCanEquipFromInventory;
		public override bool IsCanUseFromInventory => _isCanUseFromInventory;
		public override bool IsUsingInInteraction => _isUsingInInteraction;
		public override bool IsStackable => _isStackable;

		public override string BuildedInteractionUseText
		{
			get
			{
				if (string.IsNullOrEmpty(_buildedInteractionUseText))
					RebuildUsedScreenRichText();
				
				return _buildedInteractionUseText;
			}
		}
		
		public override bool TryResolveModel(ItemInventoryModels items, RangeWeaponInventoryModels range,
			MeleeWeaponInventoryModels melee, AmmoInventoryModels ammo, out InventoryItemModel model)
			=> items.TryGetModel(_type, out model);

		public override bool TryAddTo(PlayerInventoryWeapons weapons, PlayerInventoryItems items,
			PlayerInventoryAmmo ammo, out PlayerInventorySlot slot)
			=> items.TryAdd(this, out slot);

		public override bool TryRemoveFrom(PlayerInventoryWeapons weapons, PlayerInventoryItems items,
			PlayerInventoryAmmo ammo)
			=> items.TryRemove(_type);

		private void RebuildUsedScreenRichText()
		{
			if (_interactionUseText == null || _interactionUseText.Count == 0)
				_buildedInteractionUseText = string.Empty;
			else
				_buildedInteractionUseText = TMPCustomTextBuilder.Build(_interactionUseText);

#if UNITY_EDITOR
			UnityEditor.EditorUtility.SetDirty(this);
#endif
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			RebuildUsedScreenRichText();
		}

		[Button("Rebuild Used Text")]
		private void RebuildUsedScreenRichTextEditor() => RebuildUsedScreenRichText();
#endif
	}
}
