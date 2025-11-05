#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;

namespace brazenhead
{
    [Serializable]
    internal partial class Locator : ISerializationCallbackReceiver
    {
        [SerializeField] private List<SerializableType> _resolveMapKeys = new();
        [SerializeReference] private List<InstanceByKey> _resolveMapValues = new();

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            _resolveMapKeys.Clear();
            _resolveMapValues.Clear();

            foreach (var (key, value) in _resolveMap)
            {
                _resolveMapKeys.Add(new(key));
                _resolveMapValues.Add(new(value));
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            _resolveMap.Clear();

            for (int i = 0; i < _resolveMapKeys.Count; i++)
                _resolveMap.Add(_resolveMapKeys[i].Type, new(_resolveMapValues[i]));
        }

        [Serializable]
        private class InstanceByKey : SerializedReferenceDictionary<string, object>
        {
            internal InstanceByKey(IEnumerable<KeyValuePair<string, object>> collection) : base(collection) { }
        }
    }
}
#endif
