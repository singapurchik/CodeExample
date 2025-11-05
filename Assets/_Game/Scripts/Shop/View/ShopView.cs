using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Zenject;
using System;
using Core;

namespace Shop
{
	public sealed class ShopView : MonoBehaviour, IShopView
	{
		[SerializeField] private RectTransform _statsViewContainer;
		[SerializeField] private RectTransform _bundlesContainer;
		[SerializeField] private CanvasGroup _canvasGroup;
		[SerializeField] private Button _closeInfoButton;

		[Inject] private IReadOnlyList<PlayerDataValueInfo> _dataValueInfos;
		[Inject] private IPlayerDataInfo _dataInfo;
		[Inject] private StatsViewsPool _statsPool;
		[Inject] private BundlesPool _bundlesPool;
		
		private readonly Dictionary<StatsView, PlayerDataValueInfo> _statsViewInfoPair = new (10);
		private readonly HashSet<Bundle> _bundles = new (10);
		
		private Coroutine _currentUpdateStatsViewLayoutCoroutine;
		private HorizontalLayoutGroup _statsViewLayoutGroup;
		
		public event Action<PlayerDataValueInfo> OnPlusButtonClicked;
		public event Action OnCloseInfoButtonClicked;

		private void Awake()
		{
			_statsViewLayoutGroup = _statsViewContainer.GetComponent<HorizontalLayoutGroup>();
			_statsViewLayoutGroup.enabled = false;
		}

		private void OnEnable()
		{
			_closeInfoButton.onClick.AddListener(InvokeOnCloseInfoButtonClicked);
		}

		private void OnDisable()
		{
			_closeInfoButton.onClick.RemoveListener(InvokeOnCloseInfoButtonClicked);
		}

		public void DisableInput()
		{
			_canvasGroup.blocksRaycasts = false;
			_canvasGroup.interactable = false;
		}

		public void EnableInput()
		{
			_canvasGroup.blocksRaycasts = true;
			_canvasGroup.interactable = true;
		}

		public void CreateStatsViews()
		{
			foreach (var valueInfo in _dataValueInfos)
				if (valueInfo != null)
					CreateStatsView(valueInfo);
		}
		
		private void CreateStatsView(PlayerDataValueInfo info)
		{
			var view = _statsPool.Get();
			view.transform.SetParent(_statsViewContainer, false);
			view.SetLabel(info.DisplayName);
			view.SetValue(info.ReadCurrentValueAsString(_dataInfo));
			_statsViewInfoPair.Add(view, info);
			view.OnPlusButtonClicked += InvokeOnStatPlusClicked;
		}

		public Bundle CreateBundle(BundleData data)
		{
			var bundle = _bundlesPool.Get();
			bundle.transform.SetParent(_bundlesContainer, false);
			bundle.Initialize(data);
			_bundles.Add(bundle);
			return bundle;
		}
		
		public void UpdateView()
		{
			foreach (var bundle in _bundles)
				bundle.UpdateButtonState();

			foreach (var statsView in _statsViewInfoPair)
				statsView.Key.SetValue(statsView.Value.ReadCurrentValueAsString(_dataInfo));

			if (_currentUpdateStatsViewLayoutCoroutine != null)
				StopCoroutine(_currentUpdateStatsViewLayoutCoroutine);
			
			_currentUpdateStatsViewLayoutCoroutine = StartCoroutine(UpdateStatsViewLayout());
		}

		private IEnumerator UpdateStatsViewLayout()
		{
			_statsViewLayoutGroup.enabled = false;
			yield return null;
			_statsViewLayoutGroup.enabled = true;
			yield return null;
			_statsViewLayoutGroup.enabled = false;
			_currentUpdateStatsViewLayoutCoroutine = null;
		}

		private void InvokeOnStatPlusClicked(StatsView view) => OnPlusButtonClicked?.Invoke(_statsViewInfoPair[view]);

		private void InvokeOnCloseInfoButtonClicked() => OnCloseInfoButtonClicked?.Invoke();
	}
}