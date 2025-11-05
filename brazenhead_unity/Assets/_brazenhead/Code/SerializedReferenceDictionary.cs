using System;
using System.Collections.Generic;
using UnityEngine;

namespace brazenhead
{
    [Serializable]
    public abstract class SerializedReferenceDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeReference] private List<TKey> _keys = new();
        [SerializeReference] private List<TValue> _values = new();

        public SerializedReferenceDictionary() : base() { }

        public SerializedReferenceDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection) : base(collection) { }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            _keys.Clear();
            _values.Clear();

            foreach (var (key, value) in this)
            {
                _keys.Add(key);
                _values.Add(value);
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Clear();

            for (int i = 0; i < _keys.Count; i++)
                Add(_keys[i], _values[i]);
        }
    }
}
