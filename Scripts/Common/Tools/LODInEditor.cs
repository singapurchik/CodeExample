#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace FAS.EditorTools
{
	[InitializeOnLoad]
	static class LODInEditor
	{
		const string EnabledKey = "FAS.LODInEditor.Enabled";
		const string BiasKey = "FAS.LODInEditor.Bias";
		const string OriginalBiasKey = "FAS.LODInEditor.OriginalBias";
		const float DefaultBias = 50f;

		static bool Enabled
		{
			get => EditorPrefs.GetBool(EnabledKey, false);
			set => EditorPrefs.SetBool(EnabledKey, value);
		}

		static float Bias
		{
			get => EditorPrefs.GetFloat(BiasKey, DefaultBias);
			set => EditorPrefs.SetFloat(BiasKey, Mathf.Max(0.01f, value));
		}

		static bool HasOriginal => EditorPrefs.HasKey(OriginalBiasKey);
		static float OriginalBias
		{
			get => EditorPrefs.GetFloat(OriginalBiasKey, 1f);
			set => EditorPrefs.SetFloat(OriginalBiasKey, value);
		}

		static void ClearOriginal()
		{
			if (HasOriginal) EditorPrefs.DeleteKey(OriginalBiasKey);
		}

		static LODInEditor()
		{
			EditorApplication.playModeStateChanged += OnPlayModeChanged;
			EditorApplication.quitting += OnQuitting;
			EditorApplication.delayCall += Apply;
		}

		static void OnQuitting()
		{
			if (Enabled) RestoreOriginal();
		}

		static void OnPlayModeChanged(PlayModeStateChange s)
		{
			if (s == PlayModeStateChange.EnteredPlayMode) RestoreOriginal();
			if (s == PlayModeStateChange.EnteredEditMode) Apply();
		}

		static void SaveOriginalIfNeeded()
		{
			if (!HasOriginal) OriginalBias = QualitySettings.lodBias;
		}

		static void RestoreOriginal()
		{
			if (!HasOriginal) return;
			QualitySettings.lodBias = OriginalBias;
			ClearOriginal();
		}

		static void Apply()
		{
			if (!Enabled) { RestoreOriginal(); return; }
			if (Application.isPlaying) return;
			SaveOriginalIfNeeded();
			QualitySettings.lodBias = Bias;
		}

		[MenuItem("Tools/FAS/LOD In Editor/Enable", true)]
		static bool ValidateEnable() => !Enabled;

		[MenuItem("Tools/FAS/LOD In Editor/Enable")]
		static void MenuEnable()
		{
			Enabled = true;
			Apply();
		}

		[MenuItem("Tools/FAS/LOD In Editor/Disable", true)]
		static bool ValidateDisable() => Enabled;

		[MenuItem("Tools/FAS/LOD In Editor/Disable")]
		static void MenuDisable()
		{
			Enabled = false;
			Apply();
		}

		[MenuItem("Tools/FAS/LOD In Editor/Toggle")]
		static void MenuToggle()
		{
			Enabled = !Enabled;
			Apply();
		}

		[MenuItem("Tools/FAS/LOD In Editor/Window")]
		static void OpenWindow()
		{
			LODInEditorWindow.ShowWindow();
		}

		public static bool GetEnabled() => Enabled;
		public static void SetEnabled(bool value) { Enabled = value; Apply(); }
		public static float GetBias() => Bias;
		public static void SetBias(float value) { Bias = value; Apply(); }
	}

	sealed class LODInEditorWindow : EditorWindow
	{
		bool _enabled;
		float _bias;

		public static void ShowWindow()
		{
			var w = GetWindow<LODInEditorWindow>("LOD In Editor");
			w.minSize = new Vector2(260, 100);
			w.Show();
		}

		void OnEnable()
		{
			_enabled = LODInEditor.GetEnabled();
			_bias = LODInEditor.GetBias();
		}

		void OnGUI()
		{
			EditorGUI.BeginChangeCheck();
			_enabled = EditorGUILayout.Toggle("Enabled", _enabled);
			_bias = EditorGUILayout.FloatField("Bias", _bias);
			if (EditorGUI.EndChangeCheck())
			{
				LODInEditor.SetBias(_bias);
				LODInEditor.SetEnabled(_enabled);
			}

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Current lodBias", QualitySettings.lodBias.ToString("0.###"));
			if (GUILayout.Button("Apply Now")) LODInEditor.SetEnabled(_enabled);
		}
	}
}
#endif