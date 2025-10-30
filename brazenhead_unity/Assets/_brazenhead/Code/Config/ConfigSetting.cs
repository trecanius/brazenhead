namespace brazenhead
{
    internal class ConfigSetting<T> where T : struct
    {
        private readonly ConfigData _data;
        private readonly string _key;
        private readonly T _defaultValue;
        private T? _cachedValue;

        internal delegate void SettingEventHandler(in T value);
        internal event SettingEventHandler ValueSet;

        internal ConfigSetting(in ConfigData data, in string key, in T defaultValue)
        {
            _data = data;
            _key = key;
            _defaultValue = defaultValue;

            if (data.TryGetValue<T>(key, out var value))
                _cachedValue = value;
            else
                SetValue(defaultValue);
        }

        internal T GetValue()
        {
            if (_cachedValue.HasValue)
                return _cachedValue.Value;
            return (T)(_cachedValue = _data.GetValue(_key, _defaultValue));
        }

        internal void SetValue(in T value)
        {
            _cachedValue = value;
            _data.SetValue(_key, value);
            ValueSet?.Invoke(value);
        }
    }
}
