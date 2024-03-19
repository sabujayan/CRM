using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Indo.Abp.AspNetCore.Mvc.UI.Theme.Gojazz.Bundling;
using Indo.Abp.AspNetCore.Mvc.UI.Theme.Gojazz.Toolbars;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Indo.Abp.AspNetCore.Mvc.UI.Theme.Gojazz
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcUiThemeSharedModule),
        typeof(AbpAspNetCoreMvcUiMultiTenancyModule)
        )]
    public class AbpAspNetCoreMvcUIGojazzThemeModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpAspNetCoreMvcUIGojazzThemeModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {

            Configure<AbpThemingOptions>(options =>
            {
                options.Themes.Add<GojazzTheme>();

                if (options.DefaultThemeName == null)
                {
                    options.DefaultThemeName = GojazzTheme.Name;
                }
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAspNetCoreMvcUIGojazzThemeModule>("Indo.Abp.AspNetCore.Mvc.UI.Theme.Gojazz");
            });

            Configure<AbpToolbarOptions>(options =>
            {
                options.Contributors.Add(new GojazzThemeMainTopToolbarContributor());
            });

            Configure<AbpBundlingOptions>(options =>
            {
                options
                    .StyleBundles
                    .Add(GojazzThemeBundles.Styles.Global, bundle =>
                    {
                        bundle
                            .AddBaseBundles(StandardBundles.Styles.Global)
                            .AddContributors(typeof(GojazzThemeGlobalStyleContributor));
                    });

                options
                    .ScriptBundles
                    .Add(GojazzThemeBundles.Scripts.Global, bundle =>
                    {
                        bundle
                            .AddBaseBundles(StandardBundles.Scripts.Global)
                            .AddContributors(typeof(GojazzThemeGlobalScriptContributor));
                    });
            });
        }
    }
}
