using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace UExtension.Bootstrap.UI.Views
{
    [UxmlElement]
    public partial class NavigationTabView : VisualElement
    {
        private static IUExtensionEditorWindow[] _cachedUExtensions;
        public TabView TabView { get; }

        public NavigationTabView()
        {
            AddToClassList("navigation-tab-view");

            TabView = new TabView();
            TabView.AddToClassList("navigation-tab-view__tab-view");
            Add(TabView);

            _cachedUExtensions ??= AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(IUExtensionEditorWindow).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .Select(t => (IUExtensionEditorWindow)ScriptableObject.CreateInstance(t))
                .OrderBy(t => t.Order)
                .ToArray();

            foreach (var uExtension in _cachedUExtensions)
            {
                if (uExtension.VisualTreeAsset == null) continue;

                foreach (var sheet in uExtension.VisualTreeAsset.stylesheets)
                {
                    styleSheets.Add(sheet);
                }

                var tab = new Tab(uExtension.Name);
                tab.AddToClassList("navigation-tab");
                uExtension.VisualTreeAsset.CloneTree(tab);
                TabView.Add(tab);
            }
        }
    }
}