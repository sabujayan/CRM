using Indo.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Indo.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class IndoController : AbpController
    {
        protected IndoController()
        {
            LocalizationResource = typeof(IndoResource);
        }
    }
}