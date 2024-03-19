using Indo.Projectes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Indo.Clientes
{
    public interface IClientsRepository : IRepository<Clients, Guid>
    {
        Task<Clients> FindByNameAsync(string name);

        Task<List<Clients>> GetAllProjectsAsync(int SkipCount, int MaxResultCount, string? Sorting, string? SortingColumn, string? Filter = null, string? nameFilter = null, string? emailFilter = null, string? phoneNoFilter = null, string? countryFilter = null, string? stateFilter = null, string? cityFilter = null, string? zipFilter = null, string? clientProjectsFilter = null);
    }
}
