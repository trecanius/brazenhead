using brazenhead.Core;
using UnityEngine;

namespace brazenhead
{
    internal class System_Physics : ITickable<System_Physics.ILoop>
    {
        private const float SimulateInterval = 1f / 30f;

        internal System_Physics()
        {
            Physics.simulationMode = SimulationMode.Script;

            Game.Loops.AddLoop<ILoop>(SimulateInterval, -1).AddTickable(this, int.MinValue);
        }

        void ITickable<ILoop>.OnTick(in float deltaTime, in float alpha)
        {
            // only simulate whole steps
            if (alpha >= 1f)
                Physics.Simulate(SimulateInterval);
        }

        /// <summary>Every <see cref="SimulateInterval"/></summary>
        internal interface ILoop { }
    }
}
