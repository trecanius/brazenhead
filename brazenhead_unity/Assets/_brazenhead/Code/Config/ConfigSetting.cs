using System.Globalization;

namespace brazenhead
{
    internal abstract class ConfigSetting<T>
    {
        private protected readonly ConfigData data;
        private protected readonly string key;
        private protected readonly T defaultValue;

        internal delegate void SettingEventHandler(in T value);
        internal event SettingEventHandler ValueSet;

        internal ConfigSetting(in ConfigData data, in string key, in T defaultValue)
        {
            this.data = data;
            this.key = key;
            this.defaultValue = defaultValue;

            if (!data.HasValue(key))
                SetTypedValue(defaultValue);
        }

        internal T GetValue() => GetTypedValue();

        internal void SetValue(in T value)
        {
            SetTypedValue(value);
            ValueSet?.Invoke(value);
        }

        private protected abstract T GetTypedValue();

        private protected abstract void SetTypedValue(in T value);
    }

    internal class StringConfigSetting : ConfigSetting<string>
    {
        internal StringConfigSetting(in ConfigData configData, in string key, in string defaultValue) : base(configData, key, defaultValue) { }

        private protected override string GetTypedValue() => data.GetValue(key, defaultValue);

        private protected override void SetTypedValue(in string value) => data.SetValue(key, value);
    }

    internal class BoolConfigSetting : ConfigSetting<bool>
    {
        internal BoolConfigSetting(in ConfigData configData, in string key, in bool defaultValue) : base(configData, key, defaultValue) { }

        private protected override bool GetTypedValue() => bool.TryParse(data.GetValue(key), out var value) ? value : defaultValue;

        private protected override void SetTypedValue(in bool value) => data.SetValue(key, value.ToString(CultureInfo.InvariantCulture).ToLowerInvariant());
    }

    internal class IntConfigSetting : ConfigSetting<int>
    {
        internal IntConfigSetting(in ConfigData configData, in string key, in int defaultValue) : base(configData, key, defaultValue) { }

        private protected override int GetTypedValue() => int.TryParse(data.GetValue(key), NumberStyles.Any, CultureInfo.InvariantCulture, out var value) ? value : defaultValue;

        private protected override void SetTypedValue(in int value) => data.SetValue(key, value.ToString(CultureInfo.InvariantCulture));
    }

    internal class FloatConfigSetting : ConfigSetting<float>
    {
        internal FloatConfigSetting(in ConfigData configData, in string key, in float defaultValue) : base(configData, key, defaultValue) { }

        private protected override float GetTypedValue() => float.TryParse(data.GetValue(key), NumberStyles.Any, CultureInfo.InvariantCulture, out var value) ? value : defaultValue;

        private protected override void SetTypedValue(in float value) => data.SetValue(key, value.ToString(CultureInfo.InvariantCulture));
    }
}
