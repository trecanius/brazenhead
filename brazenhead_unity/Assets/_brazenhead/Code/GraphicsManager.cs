using brazenhead.Core;
using UnityEngine;

namespace brazenhead
{
    internal class GraphicsManager : IListener<Game.Start>
    {
        internal GraphicsManager()
        {
            Game.EventBus.AddListener(this);
        }

        void IListener<Game.Start>.OnEvent(in Game.Start param)
        {
            var settings = Game.Locator.Resolve<ConfigManager>().Settings;

            settings.MaxFPS.ValueSet += OnMaxFPSValueSet;

            OnMaxFPSValueSet(settings.MaxFPS.GetValue());
        }

        private void OnMaxFPSValueSet(in int value)
        {
            //int maxRefreshRate = (int)Screen.mainWindowDisplayInfo.refreshRate.value;
            Application.targetFrameRate = value == -1 ? value : Mathf.Max(value, 30);// Mathf.Clamp(value, 30, maxRefreshRate);
        }
    }
}
