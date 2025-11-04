using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace brazenhead
{
    public class GameSession : ScriptableObject
    {
        public static GameSession Instance { get; private set; }

        [field: SerializeReference] public Locator Locator { get; private set; }
        [field: SerializeReference] public EventBus EventBus { get; private set; }
        [field: SerializeReference] public LoopManager Loops { get; private set; }

        private void Awake()
        {
            Locator = new();
            EventBus = new();
            Loops = new();
        }

        private void OnEnable()
        {
            Instance = this;
            InjectUpdateLoop();

            Application.focusChanged += OnApplicationFocusChanged;
            Application.quitting += OnApplicationQuitting;

#if UNITY_EDITOR
            if (EditorApplication.isUpdating)
                EventBus.Invoke<Start>();
#endif
        }

        private void OnDisable()
        {
            RemoveUpdateLoop();

            Application.focusChanged -= OnApplicationFocusChanged;
            Application.quitting -= OnApplicationQuitting;

#if UNITY_EDITOR
            if (EditorApplication.isUpdating)
                EventBus.Invoke<Stop>();
#endif
        }

        private void OnDestroy()
        {
            Locator = null;
            EventBus = null;
            Loops = null;
        }

        private void InjectUpdateLoop()
        {
            var playerLoop = PlayerLoop.GetCurrentPlayerLoop();
            for (int i = 0; i < playerLoop.subSystemList.Length; i++)
            {
                var subSystem = playerLoop.subSystemList[i];
                if (subSystem.type == typeof(Update))
                {
                    var subSystemList = new PlayerLoopSystem[subSystem.subSystemList.Length + 1];
                    subSystemList[0] = new()
                    {
                        type = typeof(GameSession),
                        updateDelegate = Update
                    };
                    Array.Copy(subSystem.subSystemList, 0, subSystemList, 1, subSystem.subSystemList.Length);
                    subSystem.subSystemList = subSystemList;
                    playerLoop.subSystemList[i] = subSystem;
                    break;
                }
            }
            PlayerLoop.SetPlayerLoop(playerLoop);
        }

        private void RemoveUpdateLoop()
        {
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
        }

        private static void Update()
        {
            Instance.Loops.OnUpdate(Time.deltaTime);
        }

        private void OnApplicationFocusChanged(bool hasFocus)
        {
            EventBus.Invoke<FocusChange>(new(hasFocus));
        }

        private void OnApplicationQuitting()
        {
            EventBus.Invoke<Terminate>();
        }

        public readonly struct FocusChange : IEvent
        {
            public readonly bool hasFocus;

            public FocusChange(in bool hasFocus) => this.hasFocus = hasFocus;
        }

        public readonly struct Initialize : IEvent { }
        public readonly struct Start : IEvent { }
        public readonly struct Stop : IEvent { }
        public readonly struct Terminate : IEvent { }
    }
}
