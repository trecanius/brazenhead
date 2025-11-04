using System;
using UnityEngine;

namespace brazenhead
{
    [Serializable]//
    internal class FileConfigManager : ConfigManager, IListener<GameSession.Initialize>, IListener<GameSession.Start>, IListener<GameSession.Terminate>
    {
        private static readonly string _configFilePath =
#if !UNITY_EDITOR
            $"{Application.dataPath}/" +
#endif
            $"brazenhead.cfg";

        [SerializeReference] private FileConfigData _configData = new();

        [field: SerializeReference] internal override ConfigSettings Settings { get; private protected set; }

        void IListener<GameSession.Initialize>.OnEvent(in GameSession.Initialize param)
        {
            _configData.ReadFromFile(_configFilePath);
            Settings = new ConfigSettings(_configData);
            _configData.WriteToFile(_configFilePath);
        }

        void IListener<GameSession.Start>.OnEvent(in GameSession.Start param)
        {
            Debug.Log(Settings.MaxFPS.GetValue());
        }

        void IListener<GameSession.Terminate>.OnEvent(in GameSession.Terminate param)
        {
            _configData.WriteToFile(_configFilePath);
        }
    }
}
