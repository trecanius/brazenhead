#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;

namespace brazenhead
{
    [Serializable]
    internal partial class LoopManager : ISerializationCallbackReceiver
    {
        [SerializeField] private List<SerializableType> _loopMapKeys = new();
        [SerializeReference] private List<Loop> _loopMapValues = new();

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            _loopMapKeys.Clear();
            _loopMapValues.Clear();

            foreach (var (key, value) in _loopByType)
            {
                _loopMapKeys.Add(new(key));
                _loopMapValues.Add(value);
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            _loopByType.Clear();

            for (int i = 0; i < _loopMapKeys.Count; i++)
                _loopByType.Add(_loopMapKeys[i].Type, _loopMapValues[i]);
        }
    }
}
#endif
