using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Indo.Abp.AspNetCore.Mvc.UI.Theme.Gojazz.Themes.Gojazz.Components.Brand
{
    public class BrandViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Themes/Gojazz/Components/Brand/Default.cshtml");
        }
    }
}
