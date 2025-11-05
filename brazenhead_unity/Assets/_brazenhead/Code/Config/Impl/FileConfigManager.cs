using System;
using UnityEngine;

namespace brazenhead
{
    [Serializable]
    internal class FileConfigManager : ConfigManager, IListener<GameSession.Initialize>, IListener<GameSession.Start>, IListener<GameSession.End>
    {
        private static readonly string _configFilePath =
#if !UNITY_EDITOR
            $"{Application.dataPath}/" +
#endif
            $"brazenhead.cfg";

        [SerializeReference] private FileConfigData _configData = new();

        [field: SerializeReference] internal override ConfigSettings Settings { get; private protected set; } = new();

        void IListener<GameSession.Initialize>.OnEvent(in GameSession.Initialize param)
        {
            _configData.ReadFromFile(Settings, _configFilePath);
            Settings.Initialize(_configData);
            _configData.WriteToFile(_configFilePath);
        }

        void IListener<GameSession.Start>.OnEvent(in GameSession.Start param)
        {
        }

        void IListener<GameSession.End>.OnEvent(in GameSession.End param)
        {
            _configData.WriteToFile(_configFilePath);
        }
    }
}
