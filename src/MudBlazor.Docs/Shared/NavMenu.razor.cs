using System.ComponentModel.Design;
using System.Linq;
using Microsoft.AspNetCore.Components;
using MudBlazor.Docs.Extensions;
using MudBlazor.Docs.Models;
using MudBlazor.Docs.Services;

namespace MudBlazor.Docs.Shared
{
    public partial class NavMenu
    {
        [Inject] IMenuService MenuService { get; set; }
        [Inject] NavigationManager NavMan { get; set; }

        //sections are "getting-started","components", "api", ...
        string _section;

        //component links are the part of the url that tells us what component is featured
        string _componentLink;

        protected override void OnInitialized()
        {
            Refresh();
            base.OnInitialized();
        }

        public void Refresh()
        {
            _section = NavMan.GetSection();
            _componentLink = NavMan.GetComponentLink(false);
            StateHasChanged();
        }

        bool IsSubGroupExpanded(params MudComponent[] items)
        {
            #region comment about is subgroup expanded
            //if the route contains any of the links of the subgroup, then the subgroup
            //should be expanded
            //Example:
            //subgroup: form inputs & controls
            //the subgroup "form inputs & controls" should be open if the current page has in the route
            //a component included in the subgroup elements, that in this case are autocomplete, form, field,
            //radio, select...
            //this route `/components/autocomplete` should open the subgroup "form inputs..."
            #endregion

            var prefix = string.Join('/', items.Select(x => x.Link).Where(x => x != null));
            foreach (var group in items.Last().GroupComponents)
            {
                if (group.IsNavGroup)
                {
                    foreach (var subGroup in group.GroupComponents)
                    {
                        if (subGroup.IsNavGroup)
                        {
                            foreach (var subGroupPage in subGroup.GroupComponents)
                            {
                                var link = new[] { prefix, group.Link, subGroup.Link, subGroupPage.Link };
                                if (string.Join('/', link.Where(x => !string.IsNullOrEmpty(x))) == _componentLink)
                                    return true;
                            }
                        }
                        else
                        {
                            var link = new[] { prefix, group.Link, subGroup.Link };
                            if (string.Join('/', link.Where(x => !string.IsNullOrEmpty(x))) == _componentLink)
                                return true;
                        }
                    }
                }
                else
                {
                    var link = new[] { prefix, group.Link };
                    if (string.Join('/', link.Where(x => !string.IsNullOrEmpty(x))) == _componentLink)
                        return true;
                }
            }
            return false;
        }
    }
}
