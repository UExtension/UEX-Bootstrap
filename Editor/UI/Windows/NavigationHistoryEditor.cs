using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UExtension.Bootstrap.UI.Windows
{
    public class NavigationHistoryEditor : EditorWindow
    {
        [SerializeField] private VisualTreeAsset visualTreeAsset;

        [MenuItem("UExtension/Navigation/History")]
        public static void ShowExample()
        {
            var wnd = GetWindow<NavigationHistoryEditor>();
            wnd.titleContent = new GUIContent("Navigation History");
        }

        public void CreateGUI()
        {
            rootVisualElement.Clear();
            visualTreeAsset.CloneTree(rootVisualElement);
        }
    }
}