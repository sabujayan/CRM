using System;
using System.Collections.Generic;
using System.Text;
using Indo.Localization;
using Volo.Abp.Application.Services;

namespace Indo
{
    /* Inherit your application services from this class.
     */
    public abstract class IndoAppService : ApplicationService
    {
        protected IndoAppService()
        {
            LocalizationResource = typeof(IndoResource);
        }
    }
}
