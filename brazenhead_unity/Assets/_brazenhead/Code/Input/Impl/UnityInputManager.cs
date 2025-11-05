using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace brazenhead
{
    [Serializable]
    internal class UnityInputManager : InputManager, IListener<GameSession.Initialize>, IListener<GameSession.Start>, IListener<GameSession.Stop>, ITickable<UnityInputManager.ILoop>
    {
        private InputSystemActions _actions;

        [field: SerializeReference] internal override InputActions Actions { get; private protected set; }

        void IListener<GameSession.Initialize>.OnEvent(in GameSession.Initialize param)
        {
            Actions = new UnityInputActions();

            InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsManually;

            GameSession.Instance.Loops.Add<ILoop>(0f, -10).AddTickable(this, int.MinValue);
        }

        void IListener<GameSession.Start>.OnEvent(in GameSession.Start param)
        {
            _actions = new();
            _actions.Enable();

            (Actions as UnityInputActions).Initialize(_actions);
        }

        void IListener<GameSession.Stop>.OnEvent(in GameSession.Stop param)
        {
            _actions.Disable();
            _actions.Dispose();
        }

        void ITickable<ILoop>.OnTick(in float deltaTime)
        {
            Debug.Log(Time.frameCount + " - InputSystem tick");
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
