using System;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace brazenhead
{
    [DefaultExecutionOrder(-1)]
    internal class GameSession : ScriptableObject
    {
        internal static GameSession Instance { get; private set; }

        [field: SerializeReference] internal Locator Locator { get; private set; }
        [field: SerializeReference] internal EventBus EventBus { get; private set; }
        [field: SerializeReference] internal LoopManager Loops { get; private set; }

        private void Awake()
        {
            Locator = new();
            EventBus = new();
            Loops = new();
        }

        private void OnEnable()
        {
            Instance = this;

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

        private void OnDisable()
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

        private void OnDestroy()
        {
            Locator = null;
            EventBus = null;
            Loops = null;
        }

        private static void Update()
        {
            Instance.Loops.OnUpdate(Time.deltaTime);
        }

        internal static T Resolve<T>(in string key = null) where T : class => Instance.Locator.Resolve<T>(key);

        internal readonly struct FocusChange : IEvent
        {
            internal readonly bool hasFocus;

            internal FocusChange(in bool hasFocus) => this.hasFocus = hasFocus;
        }

        internal readonly struct Initialize : IEvent { }
        internal readonly struct Start : IEvent { }
        internal readonly struct Stop : IEvent { }
        internal readonly struct End : IEvent { }
    }
}
