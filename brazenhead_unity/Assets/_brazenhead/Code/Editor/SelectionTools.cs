using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace brazenhead.Editor
{
    internal static class SelectionTools
    {
        [MenuItem("brazenhead/Force Reserialize Assets/All")]
        internal static void ReserialzeAll()
        {
            if (!EditorUtility.DisplayDialog("Force ReserializeAssets", "Force reserialize all assets?", "OK", "Cancel"))
                return;

            EditorUtility.DisplayProgressBar("Reserializing assets", "Reserializing assets...", 0.00f);
            AssetDatabase.ForceReserializeAssets();
            EditorUtility.DisplayProgressBar("Reserializing assets", "Saving assets...", 0.33f);
            AssetDatabase.SaveAssets();
            EditorUtility.DisplayProgressBar("Reserializing assets", "Refreshing asset database...", 0.66f);
            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
        }

        [MenuItem("Assets/Force Reserialize", priority = 200)]
        internal static void ReserializeSelection()
        {
            var paths = new HashSet<string>();
            foreach (var guid in Selection.assetGUIDs)
                AddPathsRecursively(AssetDatabase.GUIDToAssetPath(guid), paths);

            EditorUtility.DisplayProgressBar("Reserializing assets", "Reserializing assets...", 0.00f);
            AssetDatabase.ForceReserializeAssets(paths, ForceReserializeAssetsOptions.ReserializeAssetsAndMetadata);
            EditorUtility.DisplayProgressBar("Reserializing assets", "Saving assets...", 0.33f);
            AssetDatabase.SaveAssets();
            EditorUtility.DisplayProgressBar("Reserializing assets", "Refreshing asset database...", 0.66f);
            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();

            static void AddPathsRecursively(string path, HashSet<string> paths)
            {
                paths.Add(path);
                if (Directory.Exists(path))
                    foreach (var fileOrDir in Directory.EnumerateFileSystemEntries(path).Where(x => !x.EndsWith(".meta")))
                        AddPathsRecursively(fileOrDir, paths);
            }
        }

        [MenuItem("GameObject/Make first sibling", priority = 5000)]
        internal static void MakeFirstSibling()
        {
            Undo.SetCurrentGroupName("Make first sibling");

            foreach (var item in Selection.transforms)
            {
                Undo.SetTransformParent(item, null, null);
                item.SetSiblingIndex(0);
            }

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }

        [MenuItem("GameObject/Reset transform (alter parent)", priority = 5000)]
        internal static void ResetTransformAlterParent()
        {
            Undo.SetCurrentGroupName("Reset local position");

            foreach (var item in Selection.transforms)
            {
                Undo.RecordObject(item.parent, null);
                item.parent.SetPositionAndRotation(item.position, item.rotation);
                Undo.RecordObject(item, null);
                item.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            }

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }

        [MenuItem("GameObject/Reset transform (freeze children)", priority = 5000)]
        internal static void ResetTransformFreezeChildren()
        {
            var parent = Selection.activeTransform;
            if (parent == null)
                return;

            Undo.SetCurrentGroupName("Reset transform without affecting childs");

            var childCount = parent.childCount;
            var childPosition = new Vector3[childCount];
            var childRotations = new Quaternion[childCount];
            var childScales = new Vector3[childCount];

            for (int i = 0; i < childCount; ++i)
            {
                var child = parent.GetChild(i);
                childPosition[i] = child.position;
                childRotations[i] = child.rotation;
                childScales[i] = child.lossyScale;
            }

            Undo.RecordObject(parent, null);
            parent.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            parent.localScale = Vector3.one;

            for (int i = 0; i < childCount; ++i)
            {
                var child = parent.GetChild(i);
                Undo.RecordObject(child, null);
                child.SetPositionAndRotation(childPosition[i], childRotations[i]);
                child.localScale = new Vector3(childScales[i].x / parent.lossyScale.x, childScales[i].y / parent.lossyScale.y, childScales[i].z / parent.lossyScale.z);
            }

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }

        [MenuItem("GameObject/Select parent transform", priority = 5000)]
        internal static void SelectParentTransform()
        {
            var transforms = Selection.transforms;
            if (transforms.Length == 0)
                return;

            var parents = new HashSet<Transform>();
            foreach (var child in transforms)
            {
                var parent = child.parent;
                if (parent != null)
                    parents.Add(parent);
            }

            Selection.objects = parents.ToArray();
        }
    }
}
