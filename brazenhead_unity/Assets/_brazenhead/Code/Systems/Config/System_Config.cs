using brazenhead.Core;
using UnityEngine;

namespace brazenhead
{
    internal class System_Config : ITerminatable
    {
        private static readonly string _configPath =
#if !UNITY_EDITOR
            $"{Application.dataPath}/" +
#endif
            $"brazenhead.cfg";

        internal readonly ConfigElements Elements;
        private readonly ConfigData _data;

        internal System_Config()
        {
            _data = new();
            _data.ReadFromFile(_configPath);
            Elements = new(_data);
            //_data.WriteToFile(_configPath);
        }

        bool ITerminatable.Terminate(out Awaitable awaitable)
        {
            awaitable = null;
            _data.WriteToFile(_configPath);
            return true;
        }

        internal class ConfigElements
        {
            internal ConfigElement_Int MaxFPS { get; }
            internal ConfigElement_Bool ShowFPS { get; }

            internal ConfigElements(ConfigData data)
            {
                MaxFPS = new(data, "Max FPS", 60);
                ShowFPS = new(data, "Show FPS", false);
            }
        }
    }
}
