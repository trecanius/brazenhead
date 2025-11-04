using System;

namespace brazenhead
{
    [Serializable]
    internal abstract class InputManager
    {
        internal abstract InputActions Actions { get; private protected set; }
    }
}
