using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Indo.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<IndoWebModule>();
            services.AddRazorPages()
                 .AddRazorPagesOptions(options => {
                     options.Conventions.Add(new CustomHomePageRouteConvention());
                 });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.InitializeApplication();
        }
    }
}
