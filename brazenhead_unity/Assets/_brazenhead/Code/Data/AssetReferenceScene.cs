using UnityEngine;
using UnityEditor;
using UnityEngine.AddressableAssets;

[System.Serializable]
public class AssetReferenceScene : AssetReference
{
    public AssetReferenceScene(string guid) : base(guid)
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
