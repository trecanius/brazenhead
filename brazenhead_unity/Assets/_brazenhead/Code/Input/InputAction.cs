using UnityEngine;

namespace brazenhead
{
    internal abstract class InputAction<T> where T : struct
    {
        internal enum State
        {
            Started,
            Performed,
            Canceled
        }

        internal delegate void InputEventHandler(State state, T value = default);
        internal abstract event InputEventHandler StateChanged;
    }
}
