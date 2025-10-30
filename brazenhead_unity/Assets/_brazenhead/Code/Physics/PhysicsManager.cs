using brazenhead.Core;
using UnityEngine;

namespace brazenhead
{
    internal class PhysicsManager : IListener<Game.Start>, ITickable<PhysicsManager.ILoop>
    {
        private float _simulateInterval = 1f / 30f;

        internal PhysicsManager()
        {
            //Physics.simulationMode = SimulationMode.Script;

            //Game.EventBus.AddListener(this);

            
        }

        void IListener<Game.Start>.OnEvent(in Game.Start param)
        {
            Game.Loops.Add<ILoop>(0f, -1).AddTickable(this, int.MinValue);

            var tickRateSetting = Game.Locator.Resolve<ConfigManager>().Settings.PhysicsTickRate;
            tickRateSetting.ValueSet += OnTickRateValueSet;
            OnTickRateValueSet(tickRateSetting.GetValue());
        }

        void ITickable<ILoop>.OnTick(in float deltaTime, in float alpha)
        {
            // only simulate whole steps
            Debug.Log("OnTick: " + deltaTime);
            if (alpha >= 1f)
                Physics.Simulate(_simulateInterval);
        }

        private void OnTickRateValueSet(in float value)
        {
            _simulateInterval = 1f / value;

            Debug.Log("OnTickRateValueSet: " + value);

            Game.Loops.Get<ILoop>().SetTickInterval(_simulateInterval);
        }

        internal interface ILoop { }
    }
}
