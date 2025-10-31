using System.Diagnostics;
using UnityEditor;

namespace brazenhead.Editor
{
    internal static class MiscTools
    {
        [MenuItem("brazenhead/Open .cfg file")]
        internal static void OpenConfigFile()
        {
            using Process fileOpener = new();
            fileOpener.StartInfo.FileName = "explorer";
            fileOpener.StartInfo.Arguments = "\"brazenhead.cfg\"";
            fileOpener.Start();
        }
    }
}
