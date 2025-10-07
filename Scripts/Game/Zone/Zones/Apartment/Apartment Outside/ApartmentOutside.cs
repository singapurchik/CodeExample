using FAS.Transitions;
using UnityEngine;
using System;
using TMPro;

namespace FAS.Apartments.Outside
{
	public class ApartmentOutside : MonoBehaviour
	{
		[SerializeField] private ApartmentInsideData _insideData;
		[SerializeField] private TextureSelector _windowTextureSelector;
		[SerializeField] private TransitionZone _windowTransitionZone;
		[SerializeField] private TransitionZone _doorTransitionZone;
		[SerializeField] private int _apartmentNumber;
		[SerializeField] private TextMeshPro _numberText;
		
		private ApartmentInsideData _insideDataInstance;
		private ITransitionZoneCamera _windowCamera;
		private ITransitionZoneCamera _doorCamera;
		private IApartmentWindow _window;

		public ApartmentInsideData InsideData
		{
			get
			{
				if (_insideDataInstance == null)
					_insideDataInstance = Instantiate(_insideData);
				
				return _insideDataInstance;
			}
		}
		
		public Texture WindowTexture => _windowTextureSelector.GetSelectedTexture();
		
		public int ApartmentNumber => _apartmentNumber;

		public event Action<ApartmentOutside> OnEnterApartment;
		public event Action<ApartmentOutside> OnEnterWindow;

		private void Awake()
		{
			_windowCamera = _windowTransitionZone.Camera;
			_doorCamera = _doorTransitionZone.Camera;
		}

		private void OnEnable()
		{
			_doorCamera.OnEnterFinished.AddListener(InvokeOnEnterApartment);
			_windowCamera.OnEnterFinished.AddListener(InvokeOnEnterWindow);
		}

		private void OnDisable()
		{
			_doorCamera.OnEnterFinished.RemoveListener(InvokeOnEnterApartment);
			_windowCamera.OnEnterFinished.RemoveListener(InvokeOnEnterWindow);
		}

		public void Initialize(IApartmentWindow window)
		{
			_window = window;
		}

		private void InvokeOnEnterApartment() => OnEnterApartment?.Invoke(this);
		
		private void InvokeOnEnterWindow() => OnEnterWindow?.Invoke(this);


#if UNITY_EDITOR
		private void OnValidate()
		{
			_numberText.text = _apartmentNumber.ToString();
		}
#endif
	}
}