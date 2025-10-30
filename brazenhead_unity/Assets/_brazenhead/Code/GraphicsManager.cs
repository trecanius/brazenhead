using brazenhead.Core;
using System;
using UnityEngine;

namespace brazenhead
{
    internal class GraphicsManager : IListener<Game.Start>, ITickable<GraphicsManager.ILoop>
    {
        private bool _debugPhysics;

        internal GraphicsManager()
        {
            Game.EventBus.AddListener(this);

            Game.Loops.Add<ILoop>(0f, 10).AddTickable(this);
        }

        void IListener<Game.Start>.OnEvent(in Game.Start param)
        {
            var settings = Game.Locator.Resolve<ConfigManager>().Settings;

            settings.MaxFPS.ValueSet += OnMaxFPSValueSet;
            settings.PhysicsVisualizer.ValueSet += OnDebugPhysicsValueSet;

            OnMaxFPSValueSet(settings.MaxFPS.GetValue());
            OnDebugPhysicsValueSet(settings.PhysicsVisualizer.GetValue());
        }

        void ITickable<ILoop>.OnTick(in float deltaTime, in float alpha)
        {
            if (_debugPhysics)
            {
            }
        }

        private void OnMaxFPSValueSet(in int value)
        {
            //int maxRefreshRate = (int)Screen.mainWindowDisplayInfo.refreshRate.value;
            Application.targetFrameRate = value == -1 ? value : Mathf.Max(value, 30);// Mathf.Clamp(value, 30, maxRefreshRate);
        }

        private void OnDebugPhysicsValueSet(in bool value)
        {
        }

        private interface ILoop { }
    }
}
