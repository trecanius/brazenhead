using brazenhead;
using UnityEngine;

namespace brazenhead
{
    internal class CameraManager : IListener<GameSession.Start>, IListener<GameSession.Stop>
    {
        internal Camera MainCamera { get; private set; }

        internal CameraManager()
        {
            GameSession.Instance.EventBus.AddListener<GameSession.Start>(this);
            GameSession.Instance.EventBus.AddListener<GameSession.Stop>(this);
        }

        void IListener<GameSession.Start>.OnEvent(in GameSession.Start param)
        {
            //MainCamera = GameSession.Locator.Resolve<SceneManager>().Instantiate(catalog => catalog.Prefab.Camera);
            Object.DontDestroyOnLoad(MainCamera);
        }

        void IListener<GameSession.Stop>.OnEvent(in GameSession.Stop param)
        {
            Object.Destroy(MainCamera);
        }
    }
}
