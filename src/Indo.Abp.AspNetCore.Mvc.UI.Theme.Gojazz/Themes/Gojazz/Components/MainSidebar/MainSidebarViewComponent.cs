using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Indo.Abp.AspNetCore.Mvc.UI.Theme.Gojazz.Themes.Gojazz.Components.MainSidebar
{
    public class MainSidebarViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Themes/Gojazz/Components/MainSidebar/Default.cshtml");
        }
    }
}
