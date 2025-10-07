namespace FAS.Sounds
{
	public class SoundEffects : AudioSourceWrapper, ISoundEffectsPlayer
	{
		protected override string MixerParameterPrefix => "SFX";
		
		public override void Play()
		{
		}
	}
}