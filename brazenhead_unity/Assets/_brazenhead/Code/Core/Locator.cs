using System;
using System.Collections.Generic;
using UnityEngine;

namespace brazenhead
{
    internal partial class Locator
    {
        private static readonly string _defaultKey = "";

        private readonly Dictionary<Type, Dictionary<string, object>> _resolveMap = new();

        public Binding<T> Bind<T>(in string key = null) where T : class
        {
            return new Binding<T>(this, key ?? _defaultKey);
        }

        public T Resolve<T>(in string key = null) where T : class
        {
            if (!_resolveMap.TryGetValue(typeof(T), out var instanceByKey))
            {
                Debug.LogError($"Failed to resolve type '{typeof(T).Name}' to an instance type!");
                return null;
            }
            return instanceByKey[key ?? _defaultKey] as T;
        }

        public bool TryResolve<T>(out T value) where T : class
        {
            bool result = _resolveMap.TryGetValue(typeof(T), out var instanceByKey);
            value = result ? instanceByKey[_defaultKey] as T : default;
            return result;
        }

        public bool TryResolve<T>(in string key, out T value) where T : class
        {
            bool result = _resolveMap.TryGetValue(typeof(T), out var instanceByKey);
            value = result ? instanceByKey[key ?? _defaultKey] as T : default;
            return result;
        }

        public List<T> GetInstancesOfType<T>() where T : class
        {
            var list = new List<T>();
            GetInstancesOfType(list);
            return list;
        }

        public void GetInstancesOfType<T>(in IList<T> list) where T : class
        {
            foreach (var instanceByKey in _resolveMap.Values)
                foreach (var instance in instanceByKey.Values)
                    if (instance is T typedInstance)
                        list.Add(typedInstance);
        }

        private void Bind(in Type resolveType, in string key, in object instance)
        {
            if (!_resolveMap.TryGetValue(resolveType, out var instanceByKey))
            {
                instanceByKey = new(1);
                _resolveMap.Add(resolveType, instanceByKey);
            }
            instanceByKey[key] = instance;
        }

        public readonly struct Binding<T> where T : class
        {
            private readonly Locator _registry;
            private readonly string _key;

            public Binding(in Locator registry, in string key)
            {
                _registry = registry;
                _key = key;
            }

            public readonly T To(in T instance)
            {
                if (instance == null)
                {
                    Debug.LogError("Tried to bind null instance!");
                    return default;
                }
                _registry.Bind(typeof(T), _key, instance);
                return instance;
            }
        }
    }
}
