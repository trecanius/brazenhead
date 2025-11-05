using System;
using UnityEngine;

namespace brazenhead
{

    [Serializable]
    public class SerializableType
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string AssemblyQualifiedName { get; private set; }
        [field: SerializeField] public string AssemblyName { get; private set; }

        private Type _type;
        public Type Type
        {
            get
            {
                if (_type == null)
                    _type = Type.GetType(AssemblyQualifiedName);
                return _type;
            }
        }

        public SerializableType(Type type)
        {
            _type = type;
            Name = type.Name;
            AssemblyQualifiedName = type.AssemblyQualifiedName;
            AssemblyName = type.Assembly.FullName;
        }

        public override bool Equals(object obj) => obj is SerializableType temp && Equals(temp);

        public bool Equals(SerializableType obj) => obj.Type.Equals(Type);

        public override int GetHashCode() => HashCode.Combine(Name, AssemblyQualifiedName, AssemblyName);

        public static bool operator ==(SerializableType a, SerializableType b) => ReferenceEquals(a, b) || (a is not null && b is not null && a.Equals(b));

        public static bool operator !=(SerializableType a, SerializableType b) => !(a == b);
    }
}
