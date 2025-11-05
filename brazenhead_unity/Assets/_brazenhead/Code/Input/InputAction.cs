using System;
using UnityEngine;

namespace brazenhead
{
    [Serializable]
    internal abstract class InputAction<T> where T : struct
    {
        internal enum State
        {
            Started,
            Performed,
            Canceled
        }

        internal delegate void InputEventHandler(State state, T value = default);
        internal event InputEventHandler StateChanged;

        private protected void InvokeStateChanged(State state, T value = default) => StateChanged?.Invoke(state, value);
    }

    [Serializable]
    internal class Vector2InputAction : InputAction<Vector2>
    {
    }

    [Serializable]
    internal class FloatInputAction : InputAction<float>
    {
    }
}
