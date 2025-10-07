using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
#endif

namespace FAS
{
	public class ClothesWetness : MonoBehaviour, IClothesWetness
	{
		[System.Serializable]
		private class RendererWetSettings
		{
			[HideInInspector] public string Name;
			public Renderer Renderer;

			[Header("Wet override for this renderer")]
			[Range(0f, 1f)] public float WetMetallic = 0.1f;
			[Range(0f, 1f)] public float WetSmoothness = 0.5f;

			[HideInInspector] public List<float> DryMetallic = new();
			[HideInInspector] public List<float> DrySmoothness = new();

			public List<MaterialPropertyBlock> Blocks = new();
		}

		[SerializeField] private RendererWetSettings[] _renderers = System.Array.Empty<RendererWetSettings>();
		[SerializeField] private float _defaultDisableWetnessSpeed = 2f;
		[SerializeField] private float _defaultEnableWetnessSpeed = 0.25f;

		private readonly int _smoothnessId = Shader.PropertyToID("_Smoothness");
		private readonly int _metallicId = Shader.PropertyToID("_Metallic");

		private float _requestedDisableWetnessSpeed;
		private float _requestedEnableWetnessSpeed;
		private float _currentDisableWetnessSpeed;
		private float _currentEnableWetnessSpeed;

		private bool _isForceEnableWetnessRequested;
		private bool _isEnableWetnessRequested;
		
		public float CurrentWetAmount { get; private set; }

		private void Awake()
		{
			foreach (var wetSettings in _renderers)
			{
				var sharedMats = wetSettings.Renderer.sharedMaterials;
				wetSettings.DryMetallic.Clear();
				wetSettings.DrySmoothness.Clear();
				wetSettings.Blocks.Clear();

				for (int i = 0; i < sharedMats.Length; i++)
				{
					var mat = sharedMats[i];
					if (mat == null)
					{
						wetSettings.DryMetallic.Add(0f);
						wetSettings.DrySmoothness.Add(0.5f);
						wetSettings.Blocks.Add(new MaterialPropertyBlock());
						continue;
					}

					float dryMetallic = mat.HasProperty(_metallicId) ? mat.GetFloat(_metallicId) : 0f;
					float drySmooth = mat.HasProperty(_smoothnessId) ? mat.GetFloat(_smoothnessId) : 0.5f;

					wetSettings.DryMetallic.Add(dryMetallic);
					wetSettings.DrySmoothness.Add(drySmooth);

					var block = new MaterialPropertyBlock();
					wetSettings.Renderer.GetPropertyBlock(block, i);
					wetSettings.Blocks.Add(block);
				}
			}
		}
		
		public void ForceChangeWetAmount(float amount)
		{
			if (CurrentWetAmount != amount)
			{
				CurrentWetAmount = amount;
				ApplyWetness(CurrentWetAmount);
			}
		}

		public void RequestChangeDisableWetnessSpeed(float speed) => _requestedDisableWetnessSpeed = speed;

		public void RequestEnableWetnessSmooth(float speed = 0)
		{
			_requestedEnableWetnessSpeed = speed;
			_isEnableWetnessRequested = true;
		}

		public void RequestForceEnableWetness() => _isForceEnableWetnessRequested = true;
		
		private void ApplyWetness(float time)
		{
			foreach (var rendererWetSettings in _renderers)
			{
				for (int i = 0; i < rendererWetSettings.Blocks.Count; i++)
				{
					var block = rendererWetSettings.Blocks[i];

					float metallic = Mathf.Lerp(rendererWetSettings.DryMetallic[i],
						rendererWetSettings.WetMetallic, time);
					float smoothness = Mathf.Lerp(rendererWetSettings.DrySmoothness[i],
						rendererWetSettings.WetSmoothness, time);

					block.SetFloat(_metallicId, metallic);
					block.SetFloat(_smoothnessId, smoothness);

					rendererWetSettings.Renderer.SetPropertyBlock(block, i);
				}
			}
		}

		private void ChangeWetnessSmooth(float requestedSpeed, float defaultSpeed, float targetValue)
		{
			var speed = requestedSpeed;

			if (speed <= 0)
				speed = defaultSpeed;

			CurrentWetAmount = Mathf.MoveTowards(CurrentWetAmount, targetValue, speed * Time.deltaTime);
			ApplyWetness(CurrentWetAmount);
		}

		private void Update()
		{
#if UNITY_EDITOR
			if (_isForceWetRequest)
				_isEnableWetnessRequested = true;
#endif
			if (_isEnableWetnessRequested)
			{
				if (CurrentWetAmount < 1)
					ChangeWetnessSmooth(_requestedEnableWetnessSpeed, _defaultEnableWetnessSpeed, 1);

				_isEnableWetnessRequested = false;
			}
			else if (CurrentWetAmount > 0)
			{
				ChangeWetnessSmooth(_requestedDisableWetnessSpeed, _defaultDisableWetnessSpeed, 0);
			}

			if (_isForceEnableWetnessRequested)
			{
				if (CurrentWetAmount < 1)
				{
					CurrentWetAmount = 1;
					ApplyWetness(CurrentWetAmount);
				}

				_isForceEnableWetnessRequested = false;
			}
		}

#if UNITY_EDITOR
		[SerializeField] private bool _isForceWetRequest;

		private void OnValidate()
		{
			if (_renderers == null) return;
			foreach (var s in _renderers)
				if (s != null && s.Renderer != null)
					s.Name = s.Renderer.name;
		}

		private void FindRenderers(Transform root)
		{
			var list = new List<RendererWetSettings>();

			var rendererField = typeof(RendererWetSettings).GetField("Renderer",
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

			var nameField = typeof(RendererWetSettings).GetField("Name",
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

			foreach (var t in root.GetComponentsInChildren<Transform>(true))
			{
				if (t.TryGetComponent(out Renderer r))
				{
					var settings = new RendererWetSettings();
					rendererField.SetValue(settings, r);
					nameField.SetValue(settings, r.name);
					list.Add(settings);
				}
			}

			_renderers = list.ToArray();
			EditorUtility.SetDirty(this);
		}
#endif
	}
}