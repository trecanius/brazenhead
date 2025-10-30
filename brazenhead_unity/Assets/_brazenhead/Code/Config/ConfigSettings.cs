namespace brazenhead
{
    internal class ConfigSettings
    {
        internal ConfigSetting<int> MaxFPS { get; private protected set; }
        internal ConfigSetting<bool> ShowFPS { get; private protected set; }
        internal ConfigSetting<bool> PhysicsVisualizer { get; private protected set; }
        internal ConfigSetting<float> PhysicsTickRate { get; private protected set; }

        internal ConfigSettings(in ConfigData configData)
        {
            MaxFPS = new(configData, "Max FPS", 60);
            ShowFPS = new(configData, "Show FPS", false);
            PhysicsVisualizer = new(configData, "Physics Visualizer", false);
            PhysicsTickRate = new(configData, "Physics Tick Rate", 30f);
        }
    }
}
