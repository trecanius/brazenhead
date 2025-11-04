using System;

namespace brazenhead
{
    [Serializable]
    internal class ConfigSettings
    {
        internal IntConfigSetting MaxFPS { get; private protected set; }
        internal BoolConfigSetting ShowFPS { get; private protected set; }
        internal BoolConfigSetting PhysicsDebug { get; private protected set; }
        internal FloatConfigSetting PhysicsTickRate { get; private protected set; }

        internal ConfigSettings(in ConfigData configData)
        {
            MaxFPS = new(configData, "Max FPS", 60);
            ShowFPS = new(configData, "Show FPS", false);
            PhysicsDebug = new(configData, "Physics Debug", false);
            PhysicsTickRate = new(configData, "Physics Tick Rate", 30f);
        }
    }
}
