using brazenhead.Core;
using brazenhead.Impl;
using UnityEngine;

namespace brazenhead
{
    internal class GameManager
    {
        internal GameManager()
        {
        }

        internal async void Initialize()
        {
            Game.Locator.Bind<ConfigManager>().To(new FileConfigManager());
            Game.Locator.Bind<InputManager>().To(new UnityInputManager());
            Game.Locator.Bind<GraphicsManager>().To(new());
            Game.Locator.Bind<SceneManager>().To(new());
            Game.Locator.Bind<PhysicsManager>().To(new());
            Game.Locator.Bind<CameraManager>().To(new());

            // load main scene
            await Game.Locator.Resolve<SceneManager>().LoadScene(catalog => catalog.Addressable.Scenes.Main);

            Game.EventBus.Invoke<Game.Start>();

            // instantiate player
            var playerSceneObj = Game.Locator.Resolve<SceneManager>().Instantiate(catalog => catalog.Prefab.PlayerEntity);
            var player = new PlayerEntity(playerSceneObj);
            Game.Locator.Bind<PlayerEntity>().To(player);
        }
    }
}
