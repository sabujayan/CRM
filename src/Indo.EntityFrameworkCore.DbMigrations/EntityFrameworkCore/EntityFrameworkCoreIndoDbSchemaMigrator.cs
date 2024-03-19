using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Indo.Data;
using Volo.Abp.DependencyInjection;

namespace Indo.EntityFrameworkCore
{
    public class EntityFrameworkCoreIndoDbSchemaMigrator
        : IIndoDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCoreIndoDbSchemaMigrator(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            await _serviceProvider
                .GetRequiredService<IndoMigrationsDbContext>()
                .Database
                .MigrateAsync();
        }
    }
}