using brazenhead.Core;
using UnityEngine;

namespace brazenhead
{
    internal class ConfigManager : IListener<Game.Start>, IListener<Game.Stop>
    {
        private static readonly string _configPath =
#if !UNITY_EDITOR
            $"{Application.dataPath}/" +
#endif
            $"brazenhead.cfg";

        private readonly ConfigData _configData = new();

        internal ConfigManager()
        {
            Game.EventBus.AddListener<Game.Start>(this);
            Game.EventBus.AddListener<Game.Stop>(this);
        }

        void IListener<Game.Start>.OnEvent(in Game.Start param)
        {
            _configData.ReadFromFile(_configPath);
            Game.Locator.Bind<ConfigSettings>().To(new ConfigSettings(_configData));
            _configData.WriteToFile(_configPath);
        }

        void IListener<Game.Stop>.OnEvent(in Game.Stop param)
        {
            _configData.WriteToFile(_configPath);
        }
    }
}
