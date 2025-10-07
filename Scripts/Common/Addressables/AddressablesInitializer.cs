using UnityEngine.AddressableAssets;
using UnityEngine;

namespace FAS.Addressable
{
    public class AddressablesInitializer : MonoBehaviour
    {
	    private void Awake()
	    {
		    InitializeAddressables();
	    }

	    private void InitializeAddressables()
        {
            Addressables.InitializeAsync().Completed += operation => { Debug.Log("Addressables initialized!"); };
        }
    }
}
