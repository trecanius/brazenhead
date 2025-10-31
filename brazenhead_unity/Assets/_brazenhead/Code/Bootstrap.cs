using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
            await Resources.UnloadUnusedAssets();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
#endif
            Game.Initialize();
            Game.Locator.Bind<GameManager>().To(new()).Initialize();
        }
    }
}
