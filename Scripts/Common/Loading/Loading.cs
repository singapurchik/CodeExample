using System.Collections;
using UnityEngine;
using Zenject;
using System;

namespace FAS
{
	public class Loading : MonoBehaviour, IReadOnlyLoading
	{
		[Inject] private LoadingScreen _loadingScreen;

		private bool _isAutoLoading;

		public bool IsLoading { get; private set; }

		public event Action OnLoadComplete;

		private void OnEnable()
		{
			_loadingScreen.OnFillHasMaxValue += OnFillHasMaxValue;
		}

		private void OnDisable()
		{
			_loadingScreen.OnFillHasMaxValue -= OnFillHasMaxValue;
		}

		public void PlayAutoLoading()
		{
			_isAutoLoading = true;
			StartLoading();
		}

		public void StartLoading()
		{
			IsLoading = true;
			_loadingScreen.Show();
			_loadingScreen.StartAutoMove();
		}

		private void OnFillHasMaxValue()
		{
			IsLoading = false;

			if (_isAutoLoading)
				FinishLoading();
		}

		public void FinishLoading() => StartCoroutine(HideLoadingScreen());

		private IEnumerator HideLoadingScreen()
		{
			yield return new WaitUntil(() => true);
			_loadingScreen.Hide();
			OnLoadComplete?.Invoke();
		}
	}
}