using System;

namespace brazenhead
{
    [Serializable]
    internal abstract class ConfigManager
    {
        internal abstract ConfigSettings Settings { get; private protected set; }
    }
}
