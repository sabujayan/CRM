using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Indo.EntityFrameworkCore
{
    [DependsOn(
        typeof(IndoEntityFrameworkCoreModule)
        )]
    public class IndoEntityFrameworkCoreDbMigrationsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<IndoMigrationsDbContext>();
        }
    }
}
