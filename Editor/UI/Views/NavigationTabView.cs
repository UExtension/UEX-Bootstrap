using System;
using System.Linq;
using UnityEngine.UIElements;

namespace UExtension.Bootstrap.UI.Views
{
    [UxmlElement]
    public partial class NavigationTabView : VisualElement
    {
        private static IUExtensionTabFactory[] _cachedUExtensions;
        public TabView TabView { get; }

        public NavigationTabView()
        {
            AddToClassList("navigation-tab-view");

            TabView = new TabView();
            TabView.AddToClassList("navigation-tab-view__tab-view");
            Add(TabView);

            _cachedUExtensions ??= AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(IUExtensionTabFactory).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .Select(t => (IUExtensionTabFactory)Activator.CreateInstance(t))
                .OrderBy(t => t.Order)
                .ToArray();

            foreach (var uExtension in _cachedUExtensions)
            {
                TabView.Add(uExtension.Create());
            }
        }
    }
}