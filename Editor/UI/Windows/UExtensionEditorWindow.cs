using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UExtension.Bootstrap.UI.Windows
{
    public class UExtensionEditorWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset visualTreeAsset;

        [MenuItem("UExtension/Editor Window")]
        public static void ShowExample()
        {
            var wnd = GetWindow<UExtensionEditorWindow>();
            wnd.titleContent = new GUIContent("UExtension");
        }

        public void CreateGUI()
        {
            rootVisualElement.Clear();
            visualTreeAsset.CloneTree(rootVisualElement);
        }
    }
}