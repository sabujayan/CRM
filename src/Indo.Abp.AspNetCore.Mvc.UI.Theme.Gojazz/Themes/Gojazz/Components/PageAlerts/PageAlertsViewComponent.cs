using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Alerts;

namespace Indo.Abp.AspNetCore.Mvc.UI.Theme.Gojazz.Themes.Gojazz.Components.PageAlerts
{
    public class PageAlertsViewComponent : AbpViewComponent
    {
        private readonly IAlertManager _alertManager;

        public PageAlertsViewComponent(IAlertManager alertManager)
        {
            _alertManager = alertManager;
        }

        public IViewComponentResult Invoke(string name)
        {
            return View("~/Themes/Gojazz/Components/PageAlerts/Default.cshtml", _alertManager.Alerts);
        }
    }
}
