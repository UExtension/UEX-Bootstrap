using UnityEngine.UIElements;

namespace UExtension.Bootstrap.UI
{
    public interface IUExtensionTabFactory
    {
        string Name { get; }
        int Order { get; }
        VisualTreeAsset VisualTreeAsset { get; }
    }
}