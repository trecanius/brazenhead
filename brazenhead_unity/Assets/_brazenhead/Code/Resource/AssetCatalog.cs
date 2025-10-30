using System;
using UnityEngine;

namespace brazenhead
{
    [CreateAssetMenu(fileName = "AssetCatalog", menuName = "brazenhead/AssetCatalog")]
    internal class AssetCatalog : ScriptableObject
    {
        [field: SerializeField] internal PrefabRefs Prefab { get; private set; }
        [field: SerializeField] internal AddressableRefs Addressable { get; private set; }

        [Serializable]
        internal class PrefabRefs
        {
            [field: SerializeField] internal Camera Camera { get; private set; }
            [field: SerializeField] internal EntitySceneObject PlayerEntity { get; private set; }
        }

        [Serializable]
        internal class AddressableRefs
        {
            [field: SerializeField] internal SceneRefs Scenes { get; private set; }

            [Serializable]
            internal class SceneRefs
            {
                [field: SerializeField] internal AssetReferenceScene Main { get; private set; }
            }
        }
    }
}
