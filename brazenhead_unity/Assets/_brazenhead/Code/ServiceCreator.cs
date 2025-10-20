using UnityEngine;

namespace brazenhead.Core
{
    internal static class ServiceCreator
    {
        internal static async void Initialize()
        {
            var assetSystem = new AssetSystem();
            Game.Locator.Bind<AssetSystem>().To(assetSystem);

            await assetSystem.LoadAssetCatalog();
            await assetSystem.LoadScene(assetSystem.Catalog.Scenes.Main);
        }
    }
}
