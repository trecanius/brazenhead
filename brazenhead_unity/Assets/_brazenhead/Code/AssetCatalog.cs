using System;
using UnityEngine;

namespace brazenhead
{
    [CreateAssetMenu(fileName = "AssetCatalog", menuName = "brazenhead/AssetCatalog")]
    public class AssetCatalog : ScriptableObject
    {
        [field: SerializeField] public SceneRefs Scenes { get; private set; }

        [Serializable]
        public class SceneRefs
        {
            [field: SerializeField] public AssetReferenceScene Main { get; private set; }
        }
    }
}
