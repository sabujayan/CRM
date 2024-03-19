using Indo.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Indo.Web.Pages
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class IndoPageModel : AbpPageModel
    {
        protected IndoPageModel()
        {
            LocalizationResourceType = typeof(IndoResource);
        }
    }
}