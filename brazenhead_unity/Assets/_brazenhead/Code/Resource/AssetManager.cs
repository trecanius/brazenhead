using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace brazenhead
{
    internal class AssetManager
    {
        internal async Awaitable LoadScene(AssetReferenceScene sceneRef)
        {
            Debug.Log($"Loading Scene '{sceneRef.AssetGUID}'");
            var handle = Addressables.LoadSceneAsync(sceneRef, LoadSceneMode.Single);
            await handle.Task;
            Debug.Assert(handle.Status == AsyncOperationStatus.Succeeded);
        }

        internal async Awaitable LoadAsset(AssetReferenceGameObject assetRef)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(assetRef);
            await handle.Task;
            Debug.Assert(handle.Status == AsyncOperationStatus.Succeeded);
        }

        internal async Awaitable LoadAsset<T>(AssetReferenceT<T> assetRef) where T : Object
        {
            var handle = Addressables.LoadAssetAsync<T>(assetRef);
            await handle.Task;
            Debug.Assert(handle.Status == AsyncOperationStatus.Succeeded);
        }
    }
}
