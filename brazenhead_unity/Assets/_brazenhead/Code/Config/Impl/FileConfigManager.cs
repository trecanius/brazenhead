using brazenhead.Core;
using UnityEngine;

namespace brazenhead.Impl
{
    internal class FileConfigManager : ConfigManager, IListener<Game.Stop>
    {
        private static readonly string _configFilePath =
#if !UNITY_EDITOR
            $"{Application.dataPath}/" +
#endif
            $"brazenhead.cfg";

        private readonly FileConfigData _configData = new();

        internal override ConfigSettings Settings { get; }

        internal FileConfigManager()
        {
            _configData.ReadFromFile(_configFilePath);
            Settings = new ConfigSettings(_configData);
            _configData.WriteToFile(_configFilePath);

            Game.EventBus.AddListener(this);
        }

        void IListener<Game.Stop>.OnEvent(in Game.Stop param)
        {
            _configData.WriteToFile(_configFilePath);
        }
    }
}
