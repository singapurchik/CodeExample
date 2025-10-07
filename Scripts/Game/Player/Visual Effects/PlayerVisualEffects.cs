using UnityEngine;
using Zenject;
using FAS.UI;

namespace FAS.Players
{
	public class PlayerVisualEffects : MonoBehaviour
	{
		[SerializeField] private ParticleSystem _neckSlashBloodDecal;
		[SerializeField] private ParticleSystem _leftLagSlashBloodDecal;

		[Inject] private IPlayerCostumeProxy _costumeProxy;
		[Inject] private ParticleSystem _cameraBloodEffect;
		[Inject] private IUIFadingFrame _uiFadingFrame;
		
		private PlayerVisualEffectsData VisualEffectsData => _costumeProxy.Data.VisualEffects;
		
		public void PlayEaseBloodFrameCameraEffect() => _uiFadingFrame.PlayEasyBlood();

		public void PlayHealEffect()
		{
			_uiFadingFrame.PlayHeal();
		}

		public void PlayTakeDamageBloodEffect()
		{
			_uiFadingFrame.PlayEasyBlood();
			_cameraBloodEffect.Play();
			VisualEffectsData.StabStomachBloodEffect.Play();
		}
		
		public void PlayStabStomachBloodEffect()
		{
			_cameraBloodEffect.Play();
			VisualEffectsData.StabStomachBloodEffect.Play();
			_uiFadingFrame.PlayEasyBlood();
		}
		
		public void PlayKnifeOutOfStomachBloodEffect()
		{
			_cameraBloodEffect.Play();
			VisualEffectsData.KnifeOutOfStomachBloodEffect.Play();
			_neckSlashBloodDecal.Play();
			_uiFadingFrame.PlayHardBlood();
		}

		public void PlayLeftLagSlashBloodEffect()
		{
			_cameraBloodEffect.Play();
			VisualEffectsData.LeftLagSlashBloodEffect.Play();
			_leftLagSlashBloodDecal.Play();
			_uiFadingFrame.PlayEasyBlood();
		}

		public void PlayNeckSlashBloodEffect()
		{
			_cameraBloodEffect.Play();
			VisualEffectsData.NeckSlashBloodEffect.Play();
			_neckSlashBloodDecal.Play();
			_uiFadingFrame.PlayHardBlood();
		}
	}
}