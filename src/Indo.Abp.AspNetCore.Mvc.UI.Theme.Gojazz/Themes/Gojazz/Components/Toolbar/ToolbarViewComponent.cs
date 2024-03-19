using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;

namespace Indo.Abp.AspNetCore.Mvc.UI.Theme.Gojazz.Themes.Gojazz.Components.Toolbar
{
    public class ToolbarViewComponent : AbpViewComponent
    {
        private readonly IToolbarManager _toolbarManager;

        public ToolbarViewComponent(IToolbarManager toolbarManager)
        {
            _toolbarManager = toolbarManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var toolbar = await _toolbarManager.GetAsync(StandardToolbars.Main);
            return View("~/Themes/Gojazz/Components/Toolbar/Default.cshtml", toolbar);
        }
    }
}
