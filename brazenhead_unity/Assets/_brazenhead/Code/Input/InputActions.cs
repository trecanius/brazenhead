using UnityEngine;

namespace brazenhead
{
    internal abstract class InputActions
    {
        internal InputAction<Vector2> Move { get; private protected set; }
        internal InputAction<float> Attack { get; private protected set; }
    }
}
