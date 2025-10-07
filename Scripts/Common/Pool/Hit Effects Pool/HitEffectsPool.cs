namespace FAS
{
	public class HitEffectsPool : ObjectPool<HitEffects>
	{
		protected override void InitializeObject(HitEffects effects)
		{
			effects.Initialize();
			effects.OnComplete += ReturnToPool;
		}

		protected override void CleanupObject(HitEffects effects)
		{
			effects.OnComplete -= ReturnToPool;
		}
	}	
}