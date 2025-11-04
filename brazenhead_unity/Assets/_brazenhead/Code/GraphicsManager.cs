using brazenhead;
using UnityEngine;

namespace brazenhead
{
    internal class GraphicsManager : IListener<GameSession.Start>
    {
        internal GraphicsManager()
        {
            GameSession.Instance.EventBus.AddListener(this);
        }

        void IListener<GameSession.Start>.OnEvent(in GameSession.Start param)
        {
            var settings = GameSession.Instance.Locator.Resolve<ConfigManager>().Settings;

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
