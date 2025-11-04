using System;
using UnityEngine;

namespace brazenhead
{

    [Serializable]
    public class SerializedType
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string AssemblyQualifiedName { get; private set; }
        [field: SerializeField] public string AssemblyName { get; private set; }

        private Type _systemType;
        public Type SystemType
        {
            get
            {
                if (_systemType == null)
                    _systemType = Type.GetType(AssemblyQualifiedName);
                return _systemType;
            }
        }

        public SerializedType(Type type)
        {
            _systemType = type;
            Name = type.Name;
            AssemblyQualifiedName = type.AssemblyQualifiedName;
            AssemblyName = type.Assembly.FullName;
        }

        public override bool Equals(object obj) => obj is SerializedType temp && Equals(temp);

        public bool Equals(SerializedType obj) => obj.SystemType.Equals(SystemType);

        public override int GetHashCode() => HashCode.Combine(Name, AssemblyQualifiedName, AssemblyName);

        public static bool operator ==(SerializedType a, SerializedType b) => ReferenceEquals(a, b) || (a is not null && b is not null && a.Equals(b));

        public static bool operator !=(SerializedType a, SerializedType b) => !(a == b);

        public static implicit operator Type(SerializedType type) => type.SystemType;
    }
}
