using System.Threading.Tasks;

namespace Indo.Data
{
    public interface IIndoDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
