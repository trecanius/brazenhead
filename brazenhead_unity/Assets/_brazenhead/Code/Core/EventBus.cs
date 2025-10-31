using System;
using System.Collections.Generic;
using System.Linq;

namespace brazenhead.Core
{
    public class EventBus
    {
        readonly Dictionary<Type, List<object>> _listenersByType = new();
        readonly Dictionary<Type, object> _triggeredInitEvents = new();

        public void Invoke<T>() where T : IEvent, new() => Invoke(new T());

        public void Invoke<T>(in T eventArgs) where T : IEvent
        {
            if (!eventArgs.IsInitEvent || _triggeredInitEvents.TryAdd(typeof(T), eventArgs))
                if (_listenersByType.TryGetValue(typeof(T), out var listeners) && listeners.Count != 0)
                    foreach (var listener in listeners.Cast<IListener<T>>())
                        listener.OnEvent(eventArgs);
        }

        public void AddListener<T>(in IListener<T> listener) where T : IEvent
        {
            if (_triggeredInitEvents.TryGetValue(typeof(T), out var eventArgs))
                listener.OnEvent((T)eventArgs);
            else
            {
                if (!_listenersByType.TryGetValue(typeof(T), out var listeners))
                {
                    listeners = new(1);
                    _listenersByType.Add(typeof(T), listeners);
                }
                listeners.Add(listener);
            }
        }

        public void RemoveListener<T>(in IListener<T> listener) where T : IEvent
        {
            if (_listenersByType.TryGetValue(typeof(T), out var listeners))
                listeners.Remove(listener);
        }
    }
}
