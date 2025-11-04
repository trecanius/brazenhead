using System.Linq;
using System.Reflection;
using UnityEngine;

namespace brazenhead
{
    internal class GameStarter : MonoBehaviour
    {
        private static GameSession _gameSession;

        private async void Start()
        {
            if (_gameSession != null)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            _gameSession = ScriptableObject.CreateInstance<GameSession>();
            _gameSession.Locator.Bind<ConfigManager>().To(new FileConfigManager());
            //_gameSession.Locator.Bind<InputManager>().To(new UnityInputManager());
            //GameSession.Instance.Locator.Bind<GraphicsManager>().To(new());
            _gameSession.Locator.Bind<SceneManager>().To(new());
            //GameSession.Instance.Locator.Bind<PhysicsManager>().To(new());
            //GameSession.Instance.Locator.Bind<CameraManager>().To(new());

            // add all event listeners to event bus through reflection
            foreach (var instance in _gameSession.Locator.GetInstancesOfType<object>())
                foreach (var type in instance.GetType().GetInterfaces())
                    if (type != typeof(IListener) && typeof(IListener).IsAssignableFrom(type))
                        typeof(EventBus).GetMethod(nameof(EventBus.AddListener), BindingFlags.Instance | BindingFlags.Public)
                            .MakeGenericMethod(type.GetGenericArguments().FirstOrDefault())
                            .Invoke(_gameSession.EventBus, new object[] { instance });

            _gameSession.EventBus.Invoke<GameSession.Initialize>();

#if UNITY_EDITOR
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Main")
#endif
                // load main scene
                await _gameSession.Locator.Resolve<SceneManager>().LoadScene(catalog => catalog.Addressable.Scenes.Main);

            _gameSession.EventBus.Invoke<GameSession.Start>();

            //// instantiate player
            //var playerSceneObj = GameSession.Instance.Locator.Resolve<SceneManager>().Instantiate(catalog => catalog.Prefab.PlayerEntity);
            //var player = new PlayerEntity(playerSceneObj);
            //GameSession.Instance.Locator.Bind<PlayerEntity>().To(player);
        }
    }
}
