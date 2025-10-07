using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using Zenject;

namespace FAS
{
    public class Loader : MonoBehaviour
    {
	    [Inject] private GlobalInput _input;
	    [Inject] private Loading _loading;

	    private void OnEnable()
	    {
		    _loading.OnLoadComplete += OnLoadingComplete;
	    }

	    private void OnDisable()
	    {
		    _loading.OnLoadComplete -= OnLoadingComplete;
	    }

	    public IEnumerator LoadSceneAsync(string newScene)
        {
	        _input.Disable();
            var loadSceneAsync = SceneManager.LoadSceneAsync(newScene);
	        _loading.StartLoading();

	        loadSceneAsync.allowSceneActivation = false;
	        yield return new WaitUntil(() => loadSceneAsync.progress >= 0.9f);
	        loadSceneAsync.allowSceneActivation = true;
	        yield return new WaitUntil(() => !_loading.IsLoading);
	        _loading.FinishLoading();
        }
        
        public void PlayFakeLoading()
        {
	        _input.Disable();
	        _loading.PlayAutoLoading();
        }
        
        private void OnLoadingComplete() => _input.Enable();
    }
}
