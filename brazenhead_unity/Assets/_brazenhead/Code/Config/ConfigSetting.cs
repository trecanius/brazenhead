using System;
using UnityEngine;

namespace brazenhead
{
    [Serializable]
    internal abstract class ConfigSetting
    {
        private protected string Key { get; private set; }

        internal ConfigSetting(in string key)
        {
            Key = key;
        }

        internal abstract void Initialize(in ConfigData data);
    }

    [Serializable]
    internal abstract class ConfigSetting<T> : ConfigSetting
    {
        private protected T DefaultValue { get; private set; }
        [field: SerializeReference] private protected ConfigData Data { get; private set; }

        private bool _hasCachedValue;
        private T _cachedValue;

        internal delegate void SettingEventHandler(in T value);
        internal event SettingEventHandler ValueSet;

        internal ConfigSetting(in string key, in T defaultValue) : base(key)
        {
            DefaultValue = defaultValue;
        }

        internal override void Initialize(in ConfigData data)
        {
            Data = data;

            if (TryGetTypedValue(out var value))
                SetCachedValue(value);
            else
                SetValue(DefaultValue);
        }

        internal T GetValue()
        {
            if (!_hasCachedValue)
                SetCachedValue(GetTypedValue());
            return _cachedValue;
        }

        internal void SetValue(in T value)
        {
            SetCachedValue(value);
            SetTypedValue(value);
            ValueSet?.Invoke(value);
        }

        private void SetCachedValue(in T value)
        {
            _hasCachedValue = true;
            _cachedValue = value;
        }

        private protected abstract bool TryGetTypedValue(out T value);
        private protected abstract T GetTypedValue();
        private protected abstract void SetTypedValue(T value);
    }

    [Serializable]
    internal class BoolConfigSetting : ConfigSetting<bool>
    {
        internal BoolConfigSetting(in string key, in bool defaultValue) : base(key, defaultValue) { }

        private protected override bool TryGetTypedValue(out bool value) => Data.TryGetValue(Key, out value);
        private protected override bool GetTypedValue() => Data.GetValue(Key, DefaultValue);
        private protected override void SetTypedValue(bool value) => Data.SetValue(Key, value);
    }

    [Serializable]
    internal class IntConfigSetting : ConfigSetting<int>
    {
        internal IntConfigSetting(in string key, in int defaultValue) : base(key, defaultValue) { }

        private protected override bool TryGetTypedValue(out int value) => Data.TryGetValue(Key, out value);
        private protected override int GetTypedValue() => Data.GetValue(Key, DefaultValue);
        private protected override void SetTypedValue(int value) => Data.SetValue(Key, value);
    }

    [Serializable]
    internal class FloatConfigSetting : ConfigSetting<float>
    {
        internal FloatConfigSetting(in string key, in float defaultValue) : base(key, defaultValue) { }

        private protected override bool TryGetTypedValue(out float value) => Data.TryGetValue(Key, out value);
        private protected override float GetTypedValue() => Data.GetValue(Key, DefaultValue);
        private protected override void SetTypedValue(float value) => Data.SetValue(Key, value);
    }

    [Serializable]
    internal class StringConfigSetting : ConfigSetting<string>
    {
        internal StringConfigSetting(in string key, in string defaultValue) : base(key, defaultValue) { }

        private protected override bool TryGetTypedValue(out string value) => Data.TryGetValue(Key, out value);
        private protected override string GetTypedValue() => Data.GetValue(Key, DefaultValue);
        private protected override void SetTypedValue(string value) => Data.SetValue(Key, value);
    }
}
