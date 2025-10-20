using System;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace brazenhead.Core
{
    public static class Game
    {
        public static bool IsInitialized { get; private set; }
        public static Locator Locator { get; private set; }
        public static EventBus EventBus { get; private set; }
        public static LoopManager Loops { get; private set; }

        public static void Initialize()
        {
            if (IsInitialized)
                return;

            Debug.Log($"Initializing Game");
            IsInitialized = true;
            Locator = new();
            EventBus = new();
            Loops = new();

            var playerLoop = PlayerLoop.GetCurrentPlayerLoop();
            for (int i = 0; i < playerLoop.subSystemList.Length; i++)
            {
                var subSystem = playerLoop.subSystemList[i];
                if (subSystem.type == typeof(Update))
                {
                    var subSystemList = new PlayerLoopSystem[subSystem.subSystemList.Length + 1];
                    subSystemList[0] = new()
                    {
                        type = typeof(Game),
                        updateDelegate = Update
                    };
                    Array.Copy(subSystem.subSystemList, 0, subSystemList, 1, subSystem.subSystemList.Length);
                    subSystem.subSystemList = subSystemList;
                    playerLoop.subSystemList[i] = subSystem;
                    break;
                }
            }
            PlayerLoop.SetPlayerLoop(playerLoop);
            // TODO move these outside of Game class?
            Application.focusChanged += OnFocusChanged;
            Application.quitting += OnQuitting;
        }

        public static void Terminate()
        {
            if (!IsInitialized)
                return;

            Debug.Log("Terminating Game");
            IsInitialized = false;
            Locator = null;
            EventBus = null;
            Loops = null;

#if UNITY_EDITOR
            // changes to the PlayerLoop persist when exiting Play Mode (until a domain reload)
            // only remove the subsystem we added; not sure if setting the value to PlayerLoop.GetDefaultPlayerLoop might break some other Unity subsystem
            var playerLoop = PlayerLoop.GetCurrentPlayerLoop();
            for (int i = 0; i < playerLoop.subSystemList.Length; i++)
            {
                var subSystem = playerLoop.subSystemList[i];
                if (subSystem.type == typeof(Update))
                {
                    var subSystemList = new PlayerLoopSystem[subSystem.subSystemList.Length - 1];
                    Array.Copy(subSystem.subSystemList, 1, subSystemList, 0, subSystemList.Length);
                    subSystem.subSystemList = subSystemList;
                    playerLoop.subSystemList[i] = subSystem;
                    break;
                }
            }
            PlayerLoop.SetPlayerLoop(playerLoop);
            Application.focusChanged -= OnFocusChanged;
            Application.quitting -= OnQuitting;
#endif
        }

        private static void Update()
        {
            Loops.OnUpdate(Time.deltaTime);
        }

        private static void OnFocusChanged(bool hasFocus)
        {
            EventBus.Invoke<FocusChange>(new(hasFocus));
        }

        private static void OnQuitting()
        {
            EventBus.Invoke<Quit>();
            Terminate();
        }

        public readonly struct FocusChange
        {
            public readonly bool hasFocus;

            public FocusChange(in bool hasFocus) => this.hasFocus = hasFocus;
        }

        public readonly struct Quit { }
    }
}
