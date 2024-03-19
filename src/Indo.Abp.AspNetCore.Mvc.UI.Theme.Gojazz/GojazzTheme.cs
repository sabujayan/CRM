using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.DependencyInjection;

namespace Indo.Abp.AspNetCore.Mvc.UI.Theme.Gojazz
{
    [ThemeName(Name)]
    public class GojazzTheme : ITheme, ITransientDependency
    {
        public const string Name = "Gojazz";

        public virtual string GetLayout(string name, bool fallbackToDefault = true)
        {
            switch (name)
            {
                case StandardLayouts.Application:
                    return "~/Themes/Gojazz/Layouts/Application.cshtml";
                case StandardLayouts.Account:
                    return "~/Themes/Gojazz/Layouts/Account.cshtml";
                case StandardLayouts.Empty:
                    return "~/Themes/Gojazz/Layouts/Empty.cshtml";
                default:
                    return fallbackToDefault ? "~/Themes/Gojazz/Layouts/Application.cshtml" : null;
            }
        }
    }
}
