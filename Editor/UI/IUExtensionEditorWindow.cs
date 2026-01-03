using UnityEngine.UIElements;

namespace UExtension.Bootstrap.UI
{
    public interface IUExtensionEditorWindow
    {
        string Name { get; }
        int Order { get; }
        VisualTreeAsset VisualTreeAsset { get; }
    }
}