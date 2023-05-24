using System;
using System.Collections.Generic;
using System.Linq;

namespace MudBlazor.Docs.Models
{
    public class DocsComponents
    {
        private List<MudComponent> _mudComponents = new();

        public DocsComponents AddItem(string name, Type component, params Type[] childcomponents)
        {
            var componentItem = new MudComponent
            {
                Name = name,
                Link = name.ToLowerInvariant().Replace(" ", ""),
                Type = component,
                ChildTypes = childcomponents,
                IsNavGroup = false
            };
            _mudComponents.Add(componentItem);
            return this;
        }

        public DocsComponents AddNavGroup(string name, bool expanded, DocsComponents groupItems)
        {
            return AddNavGroup(name, false, expanded, groupItems);
        }

        public DocsComponents AddNavGroup(string name, bool addLink, bool expanded, DocsComponents groupItems)
        {
            var componentItem = new MudComponent
            {
                Name = name,
                NavGroupExpanded = expanded,
                Link = addLink ? name.ToLowerInvariant().Replace(" ", "") : null,
                GroupComponents = groupItems.GetComponentsSortedByName(),
                IsNavGroup = true
            };
            _mudComponents.Add(componentItem);
            return this;
        }

        internal List<MudComponent> Components => _mudComponents;

        internal List<MudComponent> GetComponentsSortedByName()
        {
            return _mudComponents.OrderBy(e => e.Name).ToList();
        }
    }
}
