namespace FAS.Sounds
{
	public class Voice : AudioSourceWrapper, IVoicePlayer
	{
		protected override string MixerParameterPrefix => "Voice";

		public override void Play()
		{
			
		}
	}	
}