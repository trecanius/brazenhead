using UnityEngine;

namespace brazenhead.Core
{
    internal static class ServiceCreator
    {
        internal static async void Initialize()
        {
            Game.Locator.Bind<System_Config>().To(new());
            Game.Locator.Bind<System_Rendering>().To(new());
            Game.Locator.Bind<System_Physics>().To(new());
            Game.Locator.Bind<System_Input>().To(new());
            Game.Locator.Bind<System_Assets>().To(new());

            var assetSystem = Game.Locator.Resolve<System_Assets>();
            await assetSystem.LoadAssetCatalog();
            await assetSystem.LoadScene(assetSystem.Catalog.Scenes.Main);
        }
    }
}
