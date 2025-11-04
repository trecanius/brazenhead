using brazenhead;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace brazenhead
{
    [Serializable]
    internal class UnityInputManager : InputManager, IListener<GameSession.Start>, IListener<GameSession.Stop>, ITickable<UnityInputManager.ILoop>
    {
        private readonly InputSystemActions _actions = new();

        internal override InputActions Actions { get; private protected set; }

        void IListener<GameSession.Start>.OnEvent(in GameSession.Start param)
        {
            Actions = new UnityInputActions(_actions);

            InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsManually;

            GameSession.Instance.Loops.Add<ILoop>(0f, -10).AddTickable(this, int.MinValue);

            _actions.Enable();
        }

        void IListener<GameSession.Stop>.OnEvent(in GameSession.Stop param)
        {
            _actions.Disable();
            _actions.Dispose();
        }

        void ITickable<ILoop>.OnTick(in float deltaTime)
        {
            InputSystem.Update();
        }

        private void OnActionTriggered(InputAction.CallbackContext context)
        {
            var action = context.action;
            Debug.Log(action.actionMap.name + " - " + action.name + " - " + context.phase);
        }

        private interface ILoop { }
    }
}
