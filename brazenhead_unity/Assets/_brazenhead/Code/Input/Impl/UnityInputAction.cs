using UnityEngine.InputSystem;

namespace brazenhead
{
    internal class UnityInputAction<T> : InputAction<T> where T : struct
    {
        private readonly InputAction _action;

        internal override event InputEventHandler StateChanged;

        internal UnityInputAction(InputAction action)
        {
            _action = action;

            action.started += OnActionStarted;
            action.performed += OnActionPerformed;
            action.canceled += OnActionCanceled;
        }

        internal T GetValue()
        {
            return _action.ReadValue<T>();
        }

        private void OnActionStarted(InputAction.CallbackContext context)
        {
            StateChanged?.Invoke(State.Started, _action.ReadValue<T>());
        }

        private void OnActionPerformed(InputAction.CallbackContext context)
        {
            StateChanged?.Invoke(State.Performed, _action.ReadValue<T>());
        }

        private void OnActionCanceled(InputAction.CallbackContext context)
        {
            StateChanged?.Invoke(State.Canceled);
        }
    }
}
