using System;

namespace brazenhead
{
    [Serializable]
    internal abstract class InputActions
    {
        internal Vector2InputAction Move { get; private protected set; }
        internal FloatInputAction Attack { get; private protected set; }
    }
}
