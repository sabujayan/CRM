using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Indo.EntityFrameworkCore
{
    public class IndoMigrationsDbContextFactory : IDesignTimeDbContextFactory<IndoMigrationsDbContext>
    {
        public IndoMigrationsDbContext CreateDbContext(string[] args)
        {
            IndoEfCoreEntityExtensionMappings.Configure();

            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<IndoMigrationsDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Default"));

            return new IndoMigrationsDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Indo.DbMigrator/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
