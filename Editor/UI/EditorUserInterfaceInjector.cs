using UnityEngine.UIElements;

namespace UExtension.Bootstrap.UI
{
    public interface IUExtensionTabFactory
    {
        int Order { get; }
        public Tab Create();
    }
}