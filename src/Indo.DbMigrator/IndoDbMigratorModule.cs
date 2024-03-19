using Indo.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace Indo.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(IndoEntityFrameworkCoreDbMigrationsModule),
        typeof(IndoApplicationContractsModule)
        )]
    public class IndoDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
