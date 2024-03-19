using Volo.Abp.Modularity;

namespace Indo
{
    [DependsOn(
        typeof(IndoApplicationModule),
        typeof(IndoDomainTestModule)
        )]
    public class IndoApplicationTestModule : AbpModule
    {

    }
}