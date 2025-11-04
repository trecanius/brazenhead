using System;
using UnityEngine;

namespace brazenhead
{
    [Serializable]
    internal abstract class ConfigSetting<T> where T : struct
    {
        [field: SerializeReference] private protected ConfigData Data { get; private set; }
        private protected string Key { get; private set; }
        private protected T DefaultValue { get; private set; }

        private T? _cachedValue;

        internal delegate void SettingEventHandler(in T value);
        internal event SettingEventHandler ValueSet;

        internal ConfigSetting(in ConfigData data, in string key, in T defaultValue)
        {
            Data = data;
            Key = key;
            DefaultValue = defaultValue;

            if (data.TryGetValue<T>(key, out var value))
                _cachedValue = value;
            else
                SetValue(defaultValue);
        }

        internal T GetValue()
        {
            if (_cachedValue.HasValue)
                return _cachedValue.Value;
            return (T)(_cachedValue = GetTypedValue());
        }

        internal void SetValue(in T value)
        {
            _cachedValue = value;
            SetTypedValue(value);
            ValueSet?.Invoke(value);
        }

        private protected abstract T GetTypedValue();
        private protected abstract void SetTypedValue(T value);
    }

    [Serializable]
    internal class BoolConfigSetting : ConfigSetting<bool>
    {
        internal BoolConfigSetting(in ConfigData data, in string key, in bool defaultValue) : base(data, key, defaultValue)
        {
        }

        private protected override bool GetTypedValue()
        {
            return Data.GetValue(Key, DefaultValue);
        }

        private protected override void SetTypedValue(bool value)
        {
            Data.SetValue(Key, value);
        }
    }

    [Serializable]
    internal class IntConfigSetting : ConfigSetting<int>
    {
        internal IntConfigSetting(in ConfigData data, in string key, in int defaultValue) : base(data, key, defaultValue)
        {
        }

        private protected override int GetTypedValue()
        {
            return Data.GetValue(Key, DefaultValue);
        }

        private protected override void SetTypedValue(int value)
        {
            Data.SetValue(Key, value);
        }
    }

    [Serializable]
    internal class FloatConfigSetting : ConfigSetting<float>
    {
        internal FloatConfigSetting(in ConfigData data, in string key, in float defaultValue) : base(data, key, defaultValue)
        {
        }

        private protected override float GetTypedValue()
        {
            return Data.GetValue(Key, DefaultValue);
        }

        private protected override void SetTypedValue(float value)
        {
            Data.SetValue(Key, value);
        }
    }
}
