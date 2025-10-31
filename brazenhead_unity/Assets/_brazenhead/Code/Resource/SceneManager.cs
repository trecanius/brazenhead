using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using brazenhead.Core;

namespace brazenhead
{
    internal class SceneManager
    {
        private readonly AssetCatalog _assetCatalog;

        internal SceneManager()
        {
            _assetCatalog =
#if UNITY_EDITOR
                PlayerSettings.GetPreloadedAssets().Where(x => x is AssetCatalog).First() as AssetCatalog;
#else
                Resources.FindObjectsOfTypeAll<AssetCatalog>().First();
#endif
            Game.Locator.Bind<AssetCatalog.MaterialRefs>().To(_assetCatalog.Materials);
        }

        internal async Awaitable LoadScene(Func<AssetCatalog, AssetReferenceScene> getSceneRef)
        {
            var sceneRef = getSceneRef.Invoke(_assetCatalog);
            Debug.Log($"Loading Scene '{sceneRef.AssetGUID}'");
            var handle = Addressables.LoadSceneAsync(sceneRef, LoadSceneMode.Single);
            await handle.Task;
            Debug.Assert(handle.Status == AsyncOperationStatus.Succeeded);
        }

        internal async Awaitable LoadAsset(Func<AssetCatalog, AssetReferenceGameObject> getAssetRef)
        {
            var assetRef = getAssetRef.Invoke(_assetCatalog);
            Debug.Log($"Loading Asset '{assetRef.AssetGUID}'");
            var handle = Addressables.LoadAssetAsync<GameObject>(assetRef);
            await handle.Task;
            Debug.Assert(handle.Status == AsyncOperationStatus.Succeeded);
        }

        internal async Awaitable LoadAsset<T>(Func<AssetCatalog, AssetReferenceT<T>> getAssetRef) where T : Object
        {
            var assetRef = getAssetRef.Invoke(_assetCatalog);
            var handle = Addressables.LoadAssetAsync<T>(assetRef);
            await handle.Task;
            Debug.Assert(handle.Status == AsyncOperationStatus.Succeeded);
        }

        internal T Instantiate<T>(Func<AssetCatalog, T> getOriginal) where T : Object
        {
            var original = getOriginal.Invoke(_assetCatalog);
            return Object.Instantiate(original);
        }
    }
}
