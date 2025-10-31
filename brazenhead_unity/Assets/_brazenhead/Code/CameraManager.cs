using brazenhead.Core;
using UnityEngine;

namespace brazenhead
{
    internal class CameraManager : IListener<Game.Start>, IListener<Game.Stop>
    {
        internal Camera MainCamera { get; private set; }

        internal CameraManager()
        {
            Game.EventBus.AddListener<Game.Start>(this);
            Game.EventBus.AddListener<Game.Stop>(this);
        }

        void IListener<Game.Start>.OnEvent(in Game.Start param)
        {
            MainCamera = Game.Locator.Resolve<SceneManager>().Instantiate(catalog => catalog.Prefab.Camera);
            Object.DontDestroyOnLoad(MainCamera);
        }

        void IListener<Game.Stop>.OnEvent(in Game.Stop param)
        {
            Object.Destroy(MainCamera);
        }
    }
}
