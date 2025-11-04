using System;

namespace brazenhead
{
    [Serializable]
    internal abstract class ConfigData
    {
        internal abstract bool TryGetValue<T>(in string key, out T value);

        internal abstract T GetValue<T>(in string key, in T defaultValue = default);

        internal abstract void SetValue<T>(in string key, in T value);
    }
}
