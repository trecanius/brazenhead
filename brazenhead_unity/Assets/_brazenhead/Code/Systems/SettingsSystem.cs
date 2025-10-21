using UnityEngine;

namespace brazenhead
{
    internal class SettingsSystem
    {
        private static readonly string _configPath =
#if !UNITY_EDITOR
            $"{Application.dataPath}/" +
#endif
            $"brazenhead.cfg";

        internal readonly Settings settings;
        private readonly SettingsData _data;

        internal SettingsSystem()
        {
            _data = new();
            _data.ReadFromFile(_configPath);
            settings = new(_data);
            _data.WriteToFile(_configPath);
        }

        internal class Settings
        {
            internal IntSetting MaxFPS { get; }
            internal BoolSetting ShowFPS { get; }

            internal Settings(SettingsData data)
            {
                MaxFPS = new(data, "Max FPS", 60);
                ShowFPS = new(data, "Show FPS", false);
            }
        }
    }
}
