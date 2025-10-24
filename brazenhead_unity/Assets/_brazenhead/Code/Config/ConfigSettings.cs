using UnityEngine;

namespace brazenhead
{
    internal class ConfigSettings
    {
        internal IntConfigSetting MaxFPS { get; }
        internal BoolConfigSetting ShowFPS { get; }

        internal ConfigSettings(in ConfigData configData)
        {
            MaxFPS = new(configData, "Max FPS", 60);
            ShowFPS = new(configData, "Show FPS", false);
        }
    }
}
