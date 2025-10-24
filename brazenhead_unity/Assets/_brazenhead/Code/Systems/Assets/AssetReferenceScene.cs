using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

[System.Serializable]
internal class AssetReferenceScene : AssetReference
{
    internal AssetReferenceScene(in string guid) : base(guid)
    {
    }

#if UNITY_EDITOR
    public override bool ValidateAsset(Object obj)
    {
        return obj is SceneAsset;
    }

    public override bool ValidateAsset(string path)
    {
        return ValidateAsset(AssetDatabase.LoadAssetAtPath<SceneAsset>(path));
    }
#endif
}
