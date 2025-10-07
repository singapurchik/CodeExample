using UnityEngine;

namespace FAS.Apartments.Inside
{
	[RequireComponent(typeof(AudioSource))]
	public class CeilingFan : MonoBehaviour
	{
		private AudioSource _audioSource;

		private void Awake()
		{
			_audioSource = GetComponent<AudioSource>();
		}
		
		public void ChangeSpatialBlend(float blend) => _audioSource.spatialBlend = blend;
	}	
}