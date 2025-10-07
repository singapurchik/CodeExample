using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine;
using System;
using FAS.UI;

namespace FAS.Players
{
	[RequireComponent(typeof(VideoPlayer))]
	public class PlayerHealthScreen : UIScreen, IPlayerHealthView
	{
		[SerializeField] private RawImage _videoPlayerImage;

		private VideoPlayer _videoPlayer;

		public event Action OnVideoLoopPointReached;
		
		protected override void Awake()
		{
			base.Awake();
			_videoPlayer =  GetComponent<VideoPlayer>();
		}

		private void OnEnable()
		{
			_videoPlayer.loopPointReached += OnVideoLoopPoint;
		}

		private void OnDisable()
		{
			_videoPlayer.loopPointReached -= OnVideoLoopPoint;
		}
		
		private void OnVideoLoopPoint(VideoPlayer videoPlayer) => OnVideoLoopPointReached?.Invoke();

		public override void Hide()
		{
			_videoPlayer.Pause();
			_videoPlayer.time = 0;
			base.Hide();
		}

		public override void Show()
		{
			_videoPlayer.Play();
			base.Show();
		}

		public void ChangeVideoParameters(Color color, float speed)
		{
			_videoPlayer.playbackSpeed = speed;
			_videoPlayerImage.color = color;
		}
	}
}