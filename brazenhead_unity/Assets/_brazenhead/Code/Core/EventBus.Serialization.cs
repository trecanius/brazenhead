#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace brazenhead
{
    [Serializable]
    internal partial class EventBus : ISerializationCallbackReceiver
    {
        [SerializeField] private List<SerializableType> _listenerMapKeys = new();
        [SerializeReference] private List<ListenerList> _listenerMapValues = new();

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            _listenerMapKeys.Clear();
            _listenerMapValues.Clear();

            foreach (var (key, value) in _listenersByType)
            {
                _listenerMapKeys.Add(new(key));
                _listenerMapValues.Add(new(value));
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            _listenersByType.Clear();

            for (int i = 0; i < _listenerMapKeys.Count; i++)
                _listenersByType.Add(_listenerMapKeys[i].Type, new(_listenerMapValues[i]));
        }

        [Serializable]
        private class ListenerList : IList<object>
        {
            [SerializeReference] private List<object> _listeners;

            public ListenerList(IEnumerable<object> collection) => _listeners = new(collection);

            public object this[int index] { get => ((IList<object>)_listeners)[index]; set => ((IList<object>)_listeners)[index] = value; }

            public int Count => ((ICollection<object>)_listeners).Count;

            public bool IsReadOnly => ((ICollection<object>)_listeners).IsReadOnly;

            public void Add(object item) => ((ICollection<object>)_listeners).Add(item);

            public void Clear() => ((ICollection<object>)_listeners).Clear();

            public bool Contains(object item) => ((ICollection<object>)_listeners).Contains(item);

            public void CopyTo(object[] array, int arrayIndex) => ((ICollection<object>)_listeners).CopyTo(array, arrayIndex);

            public IEnumerator<object> GetEnumerator() => ((IEnumerable<object>)_listeners).GetEnumerator();

            public int IndexOf(object item) => ((IList<object>)_listeners).IndexOf(item);

            public void Insert(int index, object item) => ((IList<object>)_listeners).Insert(index, item);

            public bool Remove(object item) => ((ICollection<object>)_listeners).Remove(item);

            public void RemoveAt(int index) => ((IList<object>)_listeners).RemoveAt(index);

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_listeners).GetEnumerator();
        }
    }
}
#endif
