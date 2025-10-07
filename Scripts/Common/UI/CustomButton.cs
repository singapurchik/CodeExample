using System;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
using VInspector;

namespace FAS.UI
{
	[RequireComponent(typeof(Image))]
    public class CustomButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private AudioClip _clickSound;
        [SerializeField] private bool _isInteractable = true;
        [SerializeField] private bool _isTransparentOnNonInteractable = true;
        [SerializeField] protected bool IsAnimated;
        [ShowIf(nameof(IsAnimated))]
        [SerializeField] private float _clickAnimOutDuration = 0.06f;
        [SerializeField] private float _clickAnimInDuration = 0.03f;
        [SerializeField] private float _animScaleMultiplier = 0.75f;
        [SerializeField] private Ease _animEase = Ease.OutBounce;
        [EndIf]

        protected Image Image;
        
        private Vector3 _originalScale;
        private Vector3 _targetScale;
        
        protected bool IsEventsShowInEditor;
        private bool _isProcessClick;
        
        public bool IsHold { get; private set; }
        
        private const float DISABLED_ALPHA_VALUE = 0.6f;
        private const float ENABLED_ALPHA_VALUE = 1f;
        
        [ShowIf(nameof(IsEventsShowInEditor))]
        public UnityEvent OnEmptyClick;
        public UnityEvent OnButtonDown;
        public UnityEvent OnButtonUp;
        public UnityEvent OnClick;
        [EndIf]

        public bool IsInteractable
        {
            get => _isInteractable;
            set
            {
	            _isInteractable = value;
	            
	            if (_isTransparentOnNonInteractable)
	            {
		            var targetColor = Image.color;
		            targetColor.a = _isInteractable ? ENABLED_ALPHA_VALUE : DISABLED_ALPHA_VALUE;
		            Image.color = targetColor;   
	            }
            }
        }

        protected virtual void Awake()
        {
            Image = GetComponent<Image>();
            _originalScale = transform.localScale;
            _targetScale = _originalScale * _animScaleMultiplier;
        }

        private void OnDisable()
        {
	        IsHold = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            TryClick();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
	        if (_isInteractable)
		        ButtonDown(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
	        if (_isInteractable)
		        ButtonUp(eventData);
        }
        
        protected virtual void ButtonDown(PointerEventData eventData)
        {
	        IsHold = true;
	        OnButtonDown?.Invoke();
        }

        protected virtual void ButtonUp(PointerEventData eventData)
        {
	        IsHold = false;
	        OnButtonUp?.Invoke();
        }

        public void TryClick()
        {
            if (!_isProcessClick)
            {
                _isProcessClick = true;

                if (_isInteractable)
                    Click();
                else
                    EmptyClick();
            }
        }

        protected virtual void EmptyClick()
        {
            _isProcessClick = false;
            OnEmptyClick?.Invoke();
        }

        private void Click()
        {
            if (IsAnimated)
            {
                transform.DOScale(_targetScale, _clickAnimInDuration)
                    .OnComplete(() =>
                    {
                        transform.DOScale(_originalScale, _clickAnimOutDuration)
                            .SetEase(_animEase)
                            .OnComplete(OnClickComplete);
                    });
            }
            else
            {
                OnClickComplete();
            } 
        }

        protected virtual void OnClickComplete()
        {
            _isProcessClick = false;
            OnClick?.Invoke();
        }

#if UNITY_EDITOR
        [Button]
        private void EventsShowInEditor()
        {
            IsEventsShowInEditor = !IsEventsShowInEditor;
        }
        
        [Button]
        private void TestClickAnim()
        {
            transform.DOKill();
            transform.DOScale(_targetScale, _clickAnimInDuration)
                .OnComplete(() =>
                {
                    transform.DOScale(_originalScale, _clickAnimOutDuration)
                        .SetEase(_animEase);
                });
        }  
#endif
    }
}