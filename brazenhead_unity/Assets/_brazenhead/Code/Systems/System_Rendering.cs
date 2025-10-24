using brazenhead.Core;
using UnityEngine;

namespace brazenhead
{
    internal class System_Rendering
    {
        internal System_Rendering()
        {
            var settings = Game.Locator.Resolve<System_Config>().Elements;

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
