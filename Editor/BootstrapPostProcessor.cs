using System.Linq;
using UnityEditor;

namespace UExtension.Bootstrap.Editor
{
    public class BootstrapPostProcessor : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            var bootstrap = BootstrapEditor.FindBootstraps().Single();

            if (didDomainReload)
            {
                BootstrapEditor.PopulateServices(bootstrap);
                EditorUtility.SetDirty(bootstrap);
                AssetDatabase.SaveAssets();
            }
        }
    }
}