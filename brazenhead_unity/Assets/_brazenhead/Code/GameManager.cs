using UnityEngine;

namespace brazenhead
{
    public class GameManager : IListener<GameSession.Start>
    {
        void IListener<GameSession.Start>.OnEvent(in GameSession.Start param)
        {
            // instantiate player
            var playerSceneObj = GameSession.Instance.Locator.Resolve<SceneManager>().Instantiate(catalog => catalog.Prefab.PlayerEntity);
            var player = new PlayerEntity(playerSceneObj);
            GameSession.Instance.Locator.Bind<PlayerEntity>().To(player);
        }
    }
}
