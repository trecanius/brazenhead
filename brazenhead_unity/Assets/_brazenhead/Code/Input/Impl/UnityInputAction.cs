using System;
using UnityEngine.InputSystem;

namespace brazenhead
{
    [Serializable]
    internal class UnityInputAction<T> : InputAction<T> where T : struct
    {
        private readonly InputAction _action;

        internal UnityInputAction(InputAction action)
        {
            _action = action;

            action.started += OnActionStarted;
            action.performed += OnActionPerformed;
            action.canceled += OnActionCanceled;
        }

        private void OnActionStarted(InputAction.CallbackContext context)
        {
            InvokeStateChanged(State.Started, _action.ReadValue<T>());
        }

        private void OnActionPerformed(InputAction.CallbackContext context)
        {
            InvokeStateChanged(State.Performed, _action.ReadValue<T>());
        }

        private void OnActionCanceled(InputAction.CallbackContext context)
        {
            InvokeStateChanged(State.Canceled);
        }
    }
}
