using UnityEngine.InputSystem;

namespace brazenhead
{
    internal class InputAction<T> where T : struct
    {
        private readonly InputAction _action;

        internal delegate void InputEventHandler(in T value);
        internal event InputEventHandler Performed;

        internal InputAction(InputAction action)
        {
            _action = action;
            action.performed += OnActionPerformed;
        }

        internal T GetValue() => _action.ReadValue<T>();

        private void OnActionPerformed(InputAction.CallbackContext context)
        {
            Performed?.Invoke(context.ReadValue<T>());
        }
    }
}
