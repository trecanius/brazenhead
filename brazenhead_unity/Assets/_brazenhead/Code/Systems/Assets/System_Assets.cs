using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace brazenhead
{
    internal class System_Assets
    {
        internal AssetCatalog Catalog { get; private set; }

        internal async Awaitable LoadAssetCatalog()
        {
            Debug.Log($"Loading AssetCatalog");
            var handle = Addressables.LoadAssetAsync<AssetCatalog>(nameof(AssetCatalog));
            await handle.Task;
            Debug.Assert(handle.Status == AsyncOperationStatus.Succeeded);
            Catalog = handle.Result;
        }

        internal async Awaitable LoadScene(AssetReferenceScene sceneRef)
        {
            Debug.Log($"Loading Scene '{sceneRef.AssetGUID}'");
            var handle = Addressables.LoadSceneAsync(sceneRef, LoadSceneMode.Single);
            await handle.Task;
            Debug.Assert(handle.Status == AsyncOperationStatus.Succeeded);
        }
    }
}
