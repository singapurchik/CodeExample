namespace FAS.Players
{
	public class PlayerInventorySlotViewPool : ObjectPool<PlayerInventorySlotView>
	{
		protected override void InitializeObject(PlayerInventorySlotView view)
		{
			view.gameObject.SetActive(false);
			view.OnReturn += ReturnToPool;
		}

		protected override void CleanupObject(PlayerInventorySlotView view)
		{
			view.OnReturn -= ReturnToPool;
		}
	}
}