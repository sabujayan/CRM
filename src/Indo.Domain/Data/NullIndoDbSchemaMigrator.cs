using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Indo.Data
{
    /* This is used if database provider does't define
     * IIndoDbSchemaMigrator implementation.
     */
    public class NullIndoDbSchemaMigrator : IIndoDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}