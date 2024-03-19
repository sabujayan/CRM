using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Indo.Abp.AspNetCore.Mvc.UI.Theme.Gojazz.Themes.Gojazz.Components.Header
{
    public class HeaderViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Themes/Gojazz/Components/Header/Default.cshtml");
        }
    }
}
