using UnityEngine;
using Zenject;

namespace FAS.Scene
{
    public class SceneCreator : MonoBehaviour
    {
	    [Inject] private Loader _loader;
        
        private const string MAIN_MENU_SCENE = "Main Menu";
        private const string VILLAGE_SCENE = "Village";
        
        public void CreateMainMenu() => CreateScene(MAIN_MENU_SCENE);
        
        public void CreateVillage() => CreateScene(VILLAGE_SCENE);
        
        private void CreateScene(string targetScene) => StartCoroutine(_loader.LoadSceneAsync(targetScene));
    }
}
