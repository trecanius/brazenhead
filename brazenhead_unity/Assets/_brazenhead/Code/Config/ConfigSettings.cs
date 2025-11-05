using System;
using System.Reflection;
using UnityEngine;

namespace brazenhead
{
    [Serializable]
    internal class ConfigSettings
    {
        [field: SerializeReference] internal IntConfigSetting MaxFPS { get; private set; } = new("Max FPS", 60);
        [field: SerializeReference] internal BoolConfigSetting ShowFPS { get; private protected set; } = new("Show FPS", false);
        [field: SerializeReference] internal BoolConfigSetting PhysicsDebug { get; private protected set; } = new("Physics Debug", false);
        [field: SerializeReference] internal FloatConfigSetting PhysicsTickRate { get; private protected set; } = new("Physics Tick Rate", 30f);

        internal void Initialize(in ConfigData configData)
        {
            var initializeMethod = typeof(ConfigSetting).GetMethod(nameof(ConfigSetting.Initialize), BindingFlags.Instance | BindingFlags.NonPublic);
            var param = new object[] { configData };
            foreach (var propertyInfo in typeof(ConfigSettings).GetProperties(BindingFlags.Instance | BindingFlags.NonPublic))
                initializeMethod.Invoke(propertyInfo.GetValue(this), param);
        }
    }
}
