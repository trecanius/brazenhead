using brazenhead.Core;
using UnityEngine.InputSystem;

namespace brazenhead
{
    internal class System_Input : ITickable<System_Input.ILoop>
    {
        public System_Input()
        {
            InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsManually;

            Game.Loops.AddLoop<ILoop>(0f, -10).AddTickable(this, int.MinValue);
        }

        void ITickable<ILoop>.OnTick(in float deltaTime, in float alpha)
        {
            InputSystem.Update();
        }

        private interface ILoop { }
    }
}
