using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UExtension.Bootstrap.Editor
{
    public static class BootstrapPlayModeStateChanged
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            var bootstraps = AssetDatabase.FindAssets($"t:{nameof(Bootstrap)}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<Bootstrap>).ToList();
        }
    }
}