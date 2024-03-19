using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Indo.Web
{
    [Dependency(ReplaceServices = true)]
    public class IndoBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "CRM PRO";
    }
}
