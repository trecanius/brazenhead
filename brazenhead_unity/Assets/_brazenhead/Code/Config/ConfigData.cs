using System;

namespace brazenhead
{
    [Serializable]
    internal abstract class ConfigData
    {
        private protected BoolValueMap BoolValues { get; } = new();
        private protected IntValueMap IntValues { get; } = new();
        private protected FloatValueMap FloatValues { get; } = new();
        private protected StringValueMap StringValues { get; } = new();
        private protected bool IsDirty { get; set; }

        internal bool TryGetValue(in string key, out bool value) => BoolValues.TryGetValue(key, out value);
        internal bool TryGetValue(in string key, out int value) => IntValues.TryGetValue(key, out value);
        internal bool TryGetValue(in string key, out float value) => FloatValues.TryGetValue(key, out value);
        internal bool TryGetValue(in string key, out string value) => StringValues.TryGetValue(key, out value);

        internal bool GetValue(in string key, in bool defaultValue = false) => TryGetValue(key, out bool value) ? value : defaultValue;
        internal int GetValue(in string key, in int defaultValue = 0) => TryGetValue(key, out int value) ? value : defaultValue;
        internal float GetValue(in string key, in float defaultValue = 0f) => TryGetValue(key, out float value) ? value : defaultValue;
        internal string GetValue(in string key, in string defaultValue = "") => TryGetValue(key, out string value) ? value : defaultValue;

        internal void SetValue(in string key, in bool value)
        {
            BoolValues[key] = value;
            IsDirty = true;
        }

        internal void SetValue(in string key, in int value)
        {
            IntValues[key] = value;
            IsDirty = true;
        }

        internal void SetValue(in string key, in float value)
        {
            FloatValues[key] = value;
            IsDirty = true;
        }

        internal void SetValue(in string key, in string value)
        {
            StringValues[key] = value;
            IsDirty = true;
        }

        [Serializable] private protected class BoolValueMap : SerializedDictionary<string, bool> { }
        [Serializable] private protected class IntValueMap : SerializedDictionary<string, int> { }
        [Serializable] private protected class FloatValueMap : SerializedDictionary<string, float> { }
        [Serializable] private protected class StringValueMap : SerializedDictionary<string, string> { }
    }
}
