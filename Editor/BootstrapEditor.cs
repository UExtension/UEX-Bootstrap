using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace UExtension.Bootstrap.Editor
{
    [CustomEditor(typeof(Bootstrap))]
    public class BootstrapEditor : UnityEditor.Editor, IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var serviceProperty = serializedObject.FindProperty("<ServiceGroups>k__BackingField");

            DrawPropertiesExcluding(serializedObject, "m_Script", serviceProperty.name);

            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(serviceProperty, new GUIContent("ServiceGroups"));
            }

            if (GUILayout.Button($"Populate with {nameof(IBootstrapService)}s"))
            {
                PopulateServices(target as Bootstrap);
            }

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Load all found Bootstrap services when entering play mode to simulate pre-loaded assets when built
        /// </summary>
        [InitializeOnEnterPlayMode]
        private static void HandlePlayModeStateChanged()
        {
            var assets = LoadBootstraps();

            if (assets.Count > 1)
            {
                throw new Exception("More than one Bootstrap found !");
            }

            assets.ForEach(PopulateServices);
        }

        private static List<Bootstrap> LoadBootstraps()
        {
            return AssetDatabase
                .FindAssets($"t:{nameof(Bootstrap)}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(path =>
                {
                    Debug.Log($"Loading bootstrap => {path}");
                    return path;
                })
                .Select(AssetDatabase.LoadAssetAtPath<Bootstrap>)
                .ToList();
        }

        [MenuItem("UExtension/Bootstrap/Toggle Logging")]
        public static void ToggleLogging()
        {
            var asset = AssetDatabase
                .FindAssets($"t:{nameof(Bootstrap)}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<Bootstrap>)
                .Single();

            asset.ActiveLogging = !asset.ActiveLogging;
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();

            Debug.Log($"{nameof(Bootstrap)} Logging: {(asset.ActiveLogging ? "ON" : "OFF")}");
        }

        #region Creation

        /// <summary>
        /// Editor method to create an asset instance
        /// </summary>
        /// <returns>newly created asset instance</returns>
        [MenuItem("Assets/Create/UExtension/Bootstrap")]
        private static void CreateAsset()
        {
            var guids = AssetDatabase.FindAssets($"t:{nameof(Bootstrap)}");
            if (guids.Length > 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                Selection.activeObject = AssetDatabase.LoadAssetAtPath<Bootstrap>(path);
                throw new Exception("Bootstrap already exists");
            }

            var asset = CreateInstance<Bootstrap>();
            ProjectWindowUtil.CreateAsset(asset, "Bootstrap.asset");

            Selection.activeObject = asset;
            EditorUtility.FocusProjectWindow();

            PopulateServices(asset);
        }

        /// <summary>
        /// Populates Bootstrap services with all <see cref="IBootstrapService"/>
        /// </summary>
        /// <returns>this</returns>
        private static void PopulateServices(Bootstrap instance)
        {
            var serviceGroups = AssetDatabase.FindAssets($"t:{nameof(ScriptableObject)}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<ScriptableObject>)
                .Where(asset => asset is IBootstrapService)
                .Select(asset => (so: asset, service: asset as IBootstrapService))
                .GroupBy(tuple => tuple.service.Priority)
                .Select(group => new ServiceGroup
                {
                    Priority = group.Key,
                    Services = group.Select(g => g.so).ToList()
                })
                .ToList();

            instance.ServiceGroups.Clear();
            instance.ServiceGroups.AddRange(serviceGroups);

            EditorUtility.SetDirty(instance);
            AssetDatabase.SaveAssets();
        }

        #endregion


        #region Build

        public void OnPreprocessBuild(BuildReport report)
        {
            var bootstraps = LoadBootstraps();

            switch (bootstraps.Count)
            {
                case 0:
                    return;
                case 1:
                    AddToPreloadedAssets(bootstraps.First());
                    break;
                default:
                    throw new BuildFailedException("More than one Bootstrap found !");
            }
        }


        private static void AddToPreloadedAssets(Bootstrap bootstrap)
        {
            var preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();

            if (preloadedAssets.Contains(bootstrap)) return;

            preloadedAssets.Add(bootstrap);

            PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
        }

        #endregion
    }
}