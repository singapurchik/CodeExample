using UnityEngine;

namespace Core
{
	[CreateAssetMenu(fileName = "Scene Loading Data", menuName = "Core/Scene Loading/Data", order = 0)]
	public class SceneLoadingData : ScriptableObject
	{
		[SerializeField] private string _sceneName;
		
		public string SceneName => _sceneName;
	}
}