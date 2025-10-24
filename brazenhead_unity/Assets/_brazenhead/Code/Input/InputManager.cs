using brazenhead.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace brazenhead
{
    internal class InputManager : IListener<Game.Start>, IListener<Game.Stop>, ITickable<InputManager.ILoop>
    {
        private readonly UnityInputActions _actions = new();

        internal InputManager()
        {
            InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsManually;

            Game.EventBus.AddListener<Game.Start>(this);
            Game.EventBus.AddListener<Game.Stop>(this);
            Game.Loops.AddLoop<ILoop>(0f, -10).AddTickable(this, int.MinValue);
        }

        void IListener<Game.Start>.OnEvent(in Game.Start param)
        {
            _actions.Enable();
            Game.Locator.Bind<InputActions>().To(new InputActions(_actions));
        }

        void IListener<Game.Stop>.OnEvent(in Game.Stop param)
        {
            _actions.Disable();
            _actions.Dispose();
        }

        void ITickable<ILoop>.OnTick(in float deltaTime, in float alpha)
        {
            InputSystem.Update();
        }

        private interface ILoop { }
    }
}
