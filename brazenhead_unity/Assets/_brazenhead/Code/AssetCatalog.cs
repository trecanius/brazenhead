using System;
using UnityEngine;

namespace brazenhead
{
    [CreateAssetMenu(fileName = "AssetCatalog", menuName = "brazenhead/AssetCatalog")]
    internal class AssetCatalog : ScriptableObject
    {
        [field: SerializeField] internal SceneRefs Scenes { get; private set; }

        [Serializable]
        internal class SceneRefs
        {
            [field: SerializeField] internal AssetReferenceScene Main { get; private set; }
        }
    }
}
