using UnityEngine.UI;
using UnityEngine;
using FAS.UI;

namespace FAS.Players
{
	public class InventoryQuickBarScreen : UIInteractableScreen
	{
		[SerializeField] private GridLayoutGroup _slotsHolder;
		
		private RectTransform _slotsHolderTransform;
		
		private bool _isUpdateSlotsViewRequested;

		protected override void Awake()
		{
			base.Awake();
			_slotsHolderTransform = _slotsHolder.GetComponent<RectTransform>();
		}

		public void AddSlot(PlayerInventorySlotView slotView)
		{
			slotView.transform.SetParent(_slotsHolder.transform);
			slotView.transform.localScale = Vector3.one * 1.5f;
		}

		public void RequestUpdateSlotsView()
		{
			_isUpdateSlotsViewRequested = true;
		}
		
		private void Update()
		{
			if (_isUpdateSlotsViewRequested)
			{
				_slotsHolder.enabled = true;
				_isUpdateSlotsViewRequested = false;
			}
			else if (_slotsHolder.enabled && _slotsHolder.gameObject.activeInHierarchy)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(_slotsHolderTransform);
				_slotsHolder.enabled = false;
			}
		}
	}
}