using Indo.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Indo
{
    [DependsOn(
        typeof(IndoEntityFrameworkCoreTestModule)
        )]
    public class IndoDomainTestModule : AbpModule
    {

    }
}