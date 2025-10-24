using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

namespace brazenhead.Core
{
    internal class Bootstrap
#if UNITY_EDITOR
        : ScriptableObject, ISerializationCallbackReceiver
#endif
    {
#if UNITY_EDITOR
        private static Bootstrap _instance;

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (_instance == null)
                _instance = this;
        }

        private void OnEnable()
        {
            // only initialize after a domain reload, not if just created
            if (_instance == this)
                Initialize();
            else if (_instance == null)
                _instance = this;
        }
#endif

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static async void Initialize()
        {
#if UNITY_EDITOR
            UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false;
            // the presence of an instance allows us to automatically reinitialize after a domain reload 
            if (_instance == null)
                CreateInstance<Bootstrap>();
            Game.Terminate();
            await SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
            await Resources.UnloadUnusedAssets();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
#endif
            Game.Initialize();

            var assetCatalog = Resources.FindObjectsOfTypeAll<AssetCatalog>().First();
            Game.Locator.Bind<AssetCatalog>().To(assetCatalog);

            Game.Locator.Bind<ConfigManager>().To(new());
            Game.Locator.Bind<AssetManager>().To(new());
            Game.Locator.Bind<GraphicsManager>().To(new());
            Game.Locator.Bind<PhysicsManager>().To(new());
            Game.Locator.Bind<InputManager>().To(new());

            Game.EventBus.Invoke<Game.Start>();

            // load main scene
            var assetSystem = Game.Locator.Resolve<AssetManager>();
            await assetSystem.LoadScene(assetCatalog.Addressable.Scenes.Main);

            // instantiate camera
            var camera = Instantiate(assetCatalog.Prefab.Camera);
            Game.Locator.Bind<Camera>(Ids.Main).To(camera);

            // instantiate player
            var player = Instantiate(assetCatalog.Prefab.PlayerEntity);
            Game.Locator.Bind<PlayerEntity>().To(player);

            // bind camera to player
            camera.transform.SetParent(player.CameraHolder, false);

            player.Initialize();
        }
    }
}
