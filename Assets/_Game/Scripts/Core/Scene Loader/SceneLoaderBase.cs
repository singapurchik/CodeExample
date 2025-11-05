using UnityEngine.SceneManagement;

namespace Core
{
	public abstract class SceneLoaderBase
	{
		protected void LoadScene(SceneLoadingData data) => SceneManager.LoadScene(data.SceneName);
	}
}