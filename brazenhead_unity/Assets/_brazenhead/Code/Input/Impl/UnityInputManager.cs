using brazenhead.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace brazenhead.Impl
{
    internal class UnityInputManager : InputManager, IListener<Game.Start>, IListener<Game.Stop>, ITickable<UnityInputManager.ILoop>
    {
        private readonly InputSystemActions _actions = new();

        internal override InputActions Actions { get; }

        internal UnityInputManager()
        {
            Actions = new UnityInputActions(_actions);

            InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsManually;

            Game.EventBus.AddListener<Game.Start>(this);
            Game.EventBus.AddListener<Game.Stop>(this);
            Game.Loops.Add<ILoop>(0f, -10).AddTickable(this, int.MinValue);
        }

        void IListener<Game.Start>.OnEvent(in Game.Start param)
        {
            _actions.Enable();
        }

        void IListener<Game.Stop>.OnEvent(in Game.Stop param)
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
