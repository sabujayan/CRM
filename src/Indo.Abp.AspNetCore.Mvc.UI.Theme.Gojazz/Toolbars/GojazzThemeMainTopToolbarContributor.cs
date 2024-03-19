using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Indo.Abp.AspNetCore.Mvc.UI.Theme.Gojazz.Themes.Gojazz.Components.Toolbar.LanguageSwitch;
using Indo.Abp.AspNetCore.Mvc.UI.Theme.Gojazz.Themes.Gojazz.Components.Toolbar.UserMenu;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.Localization;
using Volo.Abp.Users;

namespace Indo.Abp.AspNetCore.Mvc.UI.Theme.Gojazz.Toolbars
{
    public class GojazzThemeMainTopToolbarContributor : IToolbarContributor
    {
        public async Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
        {
            if (context.Toolbar.Name != StandardToolbars.Main)
            {
                return;
            }

            if (!(context.Theme is GojazzTheme))
            {
                return;
            }

            var languageProvider = context.ServiceProvider.GetService<ILanguageProvider>();

            //TODO: This duplicates GetLanguages() usage. Can we eleminate this?
            var languages = await languageProvider.GetLanguagesAsync();
            if (languages.Count > 1)
            {
                context.Toolbar.Items.Add(new ToolbarItem(typeof(LanguageSwitchViewComponent)));
            }

            if (context.ServiceProvider.GetRequiredService<ICurrentUser>().IsAuthenticated)
            {
                context.Toolbar.Items.Add(new ToolbarItem(typeof(UserMenuViewComponent)));
            }
        }
    }
}
