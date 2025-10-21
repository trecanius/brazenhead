using UnityEngine;

namespace brazenhead.Core
{
    internal static class ServiceCreator
    {
        internal static async void Initialize()
        {
            Game.Locator.Bind<SettingsSystem>().To(new());

            var assetSystem = new AssetSystem();
            Game.Locator.Bind<AssetSystem>().To(assetSystem);

            await assetSystem.LoadAssetCatalog();
            await assetSystem.LoadScene(assetSystem.Catalog.Scenes.Main);
        }
    }
}
